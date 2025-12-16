using UnityEngine;

//
// PSEUDOCODE / PLAN (detailed):
// 1. Keep existing HotbarDisplay implementation unchanged except for the color comparison bug.
// 2. Replace usage of (c - targetColor).sqrMagnitude which is invalid for UnityEngine.Color.
// 3. Compute squared color distance manually by subtracting RGBA components and summing squares.
//    - dr = c.r - targetColor.r
//    - dg = c.g - targetColor.g
//    - db = c.b - targetColor.b
//    - da = c.a - targetColor.a
//    - sqrDist = dr*dr + dg*dg + db*db + da*da
// 4. Compare sqrDist with ColorToleranceSqr to decide validity.
// 5. Return same boolean semantics as before.
// 6. Leave all other logic intact to avoid changing behavior.
// 
// Implementation note: using manual component math avoids reliance on Vector types or extension methods
// and fixes CS1061 error: 'Color' does not contain a definition for 'sqrMagnitude'.
//

public class HotbarDisplay : StaticInventoryDisplay
{
    private int _maxIndexSize;
    private int _currentIndex;


    public GameObject SeedPlacing; // Optional preview prefab assigned in Inspector. If null, will use item's PlacedPrefab as preview.
    private GameObject _seedPlacingInstance; // Runtime instance of the preview
    public Material SeedPlacingValid;
    public Material SeedPlacingNotValid;
    public Transform Player;

    // Cached renderers for quick material updates and bounds computation
    private Renderer[] _previewRenderers;

    // Tolerance for comparing colors (squared distance)
    private const float ColorToleranceSqr = 0.001f;

    protected override void Start()
    {
        base.Start();

        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("HotbarDisplay: slots[] is Empty");
            return;
        }

        _currentIndex = 0;
        _maxIndexSize = slots.Length - 1;

        slots[_currentIndex].ToggleHighlight();
        Debug.Log("✅ HotbarDisplay started. Slots found: " + slots.Length);
    }

    void OnDisable()
    {
        // Ensure preview cleaned up when this UI is disabled
        DestroySeedPreview();
    }

    void Update()
    {
        if (slots == null || slots.Length == 0) return;
        CheckNumberKeys();
        CheckScrollWheel();
        CheckUseItem();
    }

    private void CheckNumberKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetIndex(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetIndex(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetIndex(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) SetIndex(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) SetIndex(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) SetIndex(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) SetIndex(9);
    }

    private void CheckScrollWheel()
    {
        //Debug.Log("Scrolling value: " + Input.GetAxis("Mouse ScrollWheel"));
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.1f) ChangeIndex(1);
        if (scroll < -0.1f) ChangeIndex(-1);
    }

    private void CheckUseItem()
    {
        var inventory = Player.GetComponent<PlayerInventoryHolder>();

        // Validate current index and slots array
        if (slots == null || slots.Length == 0) return;
        if (_currentIndex < 0 || _currentIndex >= slots.Length)
        {
            // If nothing selected, ensure no preview is shown
            DestroySeedPreview();
            return;
        }

        var uiSlot = slots[_currentIndex];
        var slot = uiSlot?.AssignedInventorySlot;

        // Only allow preview / raycast if item is placeable
        if (slot?.ItemData?.PlacedPrefab == null)
        {
            DestroySeedPreview();   // remove preview if switching items
            return;                 // stop ALL preview logic
        }

        // Not clicking: show/move preview if applicable, OR prepare for confirming placement if clicked
        if (slot == null || slot.ItemData == null)
        {
            DestroySeedPreview();
            return;
        }

        var item = slot.ItemData;
        if (item.PlacedPrefab == null && SeedPlacing == null)
        {
            // Nothing to preview
            DestroySeedPreview();
            return;
        }

        var cam = Camera.main;
        if (cam == null)
        {
            DestroySeedPreview();
            return;
        }

        // Compute target position in world for preview
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPos;

        // Prefer physics raycast hit point
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            targetPos = hitInfo.point;
        }
        else
        {
            // Fallback: intersect with horizontal plane at y = 0
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter))
            {
                targetPos = ray.GetPoint(enter);
            }
            else
            {
                // As a final fallback place it some distance in front of the camera
                targetPos = cam.transform.position + cam.transform.forward * 10f;
            }
        }

        // Ensure preview instance exists
        GameObject previewPrefab = SeedPlacing != null ? SeedPlacing : item.PlacedPrefab;
        if (previewPrefab == null)
        {
            DestroySeedPreview();
            return;
        }

        if (_seedPlacingInstance == null)
        {
            CreateSeedPreview(previewPrefab);
        }

        if (_seedPlacingInstance != null)
        {
            // Move preview to target position every frame
            _seedPlacingInstance.transform.position = targetPos;

            // Update validity and apply material feedback
            bool valid = UpdatePreviewValidity(targetPos);
            ApplyPreviewMaterial(valid);

            // Handle left click use AFTER preview has been positioned & material applied.
            if (Input.GetMouseButtonDown(0))
            {
                if (slot == null)
                {
                    Debug.Log("❌ Slot has no inventory slot assigned.");
                    return;
                }

                if (slot.ItemData == null)
                {
                    Debug.Log("❌ No item in selected slot.");
                    return;
                }

                // Extra verification: if preview exists, require color-based verification AND physics-based verification.
                bool previewColorValid = IsPreviewValidByColor();
                bool physicsValid = UpdatePreviewValidity(targetPos); // re-check to be sure

                if (_seedPlacingInstance != null)
                {
                    if (!previewColorValid)
                    {
                        Debug.Log("⛔ Placement blocked: preview indicates INVALID placement (color mismatch).");
                        return;
                    }

                    if (!physicsValid)
                    {
                        Debug.Log("⛔ Placement blocked: physics collision detected.");
                        return;
                    }
                }

                Debug.Log("▶ Using item: " + slot.ItemData.name);
                slot.ItemData.UseItem(_seedPlacingInstance.transform, inventory);

                // After using, remove preview if present
                DestroySeedPreview();
                return;
            }
        }
    }

    // Instantiate preview and make it non-physical (disable colliders, make rigidbodies kinematic)
    private void CreateSeedPreview(GameObject prefab)
    {
        if (prefab == null) return;

        // Instantiate at origin; will be positioned in Update
        _seedPlacingInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        _seedPlacingInstance.name = prefab.name + " (Preview)";

        // Disable colliders
        foreach (var col in _seedPlacingInstance.GetComponentsInChildren<Collider>(true))
        {
            col.enabled = false;
        }

        // Make rigidbodies kinematic
        foreach (var rb in _seedPlacingInstance.GetComponentsInChildren<Rigidbody>(true))
        {
            rb.isKinematic = true;
        }

        // Cache renderers for bounds and material changes
        _previewRenderers = _seedPlacingInstance.GetComponentsInChildren<Renderer>(true);

        // Optionally disable any scripts to avoid runtime behavior (best-effort)
        foreach (var mono in _seedPlacingInstance.GetComponentsInChildren<MonoBehaviour>(true))
        {
            // Do not disable this script if the preview prefab reuses code expecting to be active.
            // We try to disable MonoBehaviours that look like they control gameplay by checking for presence of Unity lifecycle methods is not trivial,
            // so avoid disabling everything to not inadvertently break purely visual components.
            // If you face issues, assign a dedicated preview prefab to `SeedPlacing` that has no active scripts.
        }

        // Ensure preview is visible and won't be affected by scene lighting/state unexpectedly
        _seedPlacingInstance.SetActive(true);
    }

    private void DestroySeedPreview()
    {
        if (_seedPlacingInstance != null)
        {
            Destroy(_seedPlacingInstance);
            _seedPlacingInstance = null;
            _previewRenderers = null;
        }
    }

    // Check whether placing at the given world position would collide with other (non-preview) colliders.
    // Returns true if valid (no blocking collisions), false if invalid.
    private bool UpdatePreviewValidity(Vector3 targetPos)
    {
        if (_seedPlacingInstance == null) return false;

        // If we have renderers, compute a combined world-space bounds
        if (_previewRenderers != null && _previewRenderers.Length > 0)
        {
            Bounds combined = _previewRenderers[0].bounds;
            for (int i = 1; i < _previewRenderers.Length; i++)
            {
                combined.Encapsulate(_previewRenderers[i].bounds);
            }

            // If preview is not at the same location used to compute bounds, recalc by shifting combined center toward the preview position.
            // But because we move the instance before calling this, bounds should already be in correct world-space.

            Vector3 center = combined.center;
            Vector3 halfExtents = combined.extents;
            Quaternion orientation = _seedPlacingInstance.transform.rotation;

            // Guard: if extents are degenerate, fall back to a small sphere test
            if (halfExtents.sqrMagnitude < 1e-6f)
            {
                return !Physics.CheckSphere(targetPos, 0.25f, ~0, QueryTriggerInteraction.Ignore);
            }

            // OverlapBox to detect potential blocking colliders.
            Collider[] overlaps = Physics.OverlapBox(center, halfExtents, orientation, ~0, QueryTriggerInteraction.Ignore);

            foreach (var c in overlaps)
            {
                if (c == null) continue;

                // Ignore colliders that are part of the preview instance itself
                if (_seedPlacingInstance != null && c.transform.IsChildOf(_seedPlacingInstance.transform)) continue;

                // Optionally ignore common ground/terrain tags so normal ground doesn't prevent placement.
                // If your ground uses a different tag or layer, adjust accordingly.
                if (c.gameObject.CompareTag("Ground") || c.gameObject.CompareTag("Terrain")) continue;

                // If we find any other collider, placement is invalid
                return false;
            }

            // No blocking colliders found -> valid
            return true;
        }
        else
        {
            // Fallback: if no renderers found, use a small sphere overlap at the target position.
            float radius = 0.5f;
            Collider[] overlaps = Physics.OverlapSphere(targetPos, radius, ~0, QueryTriggerInteraction.Ignore);
            foreach (var c in overlaps)
            {
                if (c == null) continue;
                if (_seedPlacingInstance != null && c.transform.IsChildOf(_seedPlacingInstance.transform)) continue;
                if (c.gameObject.CompareTag("Ground") || c.gameObject.CompareTag("Terrain")) continue;
                return false;
            }

            return true;
        }
    }

    // Apply a single material to all preview renderers to indicate valid/invalid placement.
    private void ApplyPreviewMaterial(bool valid)
    {
        if (_previewRenderers == null || _previewRenderers.Length == 0) return;

        Material chosen = valid ? SeedPlacingValid : SeedPlacingNotValid;
        if (chosen == null) return;

        foreach (var r in _previewRenderers)
        {
            if (r == null) continue;
            // Keep same material count per renderer, but set all to the chosen material.
            int count = (r.sharedMaterials != null) ? r.sharedMaterials.Length : 1;
            if (count <= 0) count = 1;
            Material[] mats = new Material[count];
            for (int i = 0; i < count; i++) mats[i] = chosen;
            r.materials = mats;
        }
    }

    // Determine whether the preview's current materials indicate a "valid" placement via color comparison.
    // Returns true only if:
    //  - We have preview renderers and a SeedPlacingValid material
    //  - All renderer materials have a main color matching SeedPlacingValid within tolerance
    private bool IsPreviewValidByColor()
    {
        if (_seedPlacingInstance == null) return false;
        if (_previewRenderers == null || _previewRenderers.Length == 0) return false;
        if (SeedPlacingValid == null) return false;

        Color targetColor = GetMaterialMainColor(SeedPlacingValid, out bool gotTarget);
        if (!gotTarget) return false;

        foreach (var r in _previewRenderers)
        {
            if (r == null) continue;
            var mats = r.materials;
            if (mats == null || mats.Length == 0) return false;

            foreach (var m in mats)
            {
                if (m == null) return false;

                Color c = GetMaterialMainColor(m, out bool got);
                if (!got) return false;

                // Compute squared distance between colors manually (Color doesn't have sqrMagnitude).
                float dr = c.r - targetColor.r;
                float dg = c.g - targetColor.g;
                float db = c.b - targetColor.b;
                float da = c.a - targetColor.a;
                float sqrDist = dr * dr + dg * dg + db * db + da * da;

                if (sqrDist > ColorToleranceSqr)
                    return false;
            }
        }

        return true;
    }

    // Helper: obtain main color from material. If material doesn't expose a color, return white and got=false.
    private Color GetMaterialMainColor(Material mat, out bool got)
    {
        got = false;
        if (mat == null) return Color.white;

        // Standard property name for main tint is "_Color". Try that first.
        if (mat.HasProperty("_Color"))
        {
            got = true;
            return mat.GetColor("_Color");
        }

        // Fallback to Material.color property (maps to _Color in many cases)
        try
        {
            got = true;
            return mat.color;
        }
        catch
        {
            got = false;
            return Color.white;
        }
    }

    void ChangeIndex(int direction)
    {
        slots[_currentIndex].ToggleHighlight();
        _currentIndex += direction;

        if (_currentIndex > _maxIndexSize) _currentIndex = 0;
        if (_currentIndex < 0) _currentIndex = _maxIndexSize;

        slots[_currentIndex].ToggleHighlight();
        Debug.Log("🎯 Hotbar index = " + _currentIndex);
    }

    void SetIndex(int newIndex)
    {
        // If clicking the already selected slot → unselect it
        if (_currentIndex == newIndex)
        {
            slots[_currentIndex].ToggleHighlight(); // turn it off
            _currentIndex = -1; // nothing selected
            Debug.Log("🔹 Hotbar deselected");
            // Destroy preview when deselecting
            DestroySeedPreview();
            return;
        }

        // Turn off previous highlight
        if (_currentIndex >= 0)
            slots[_currentIndex].ToggleHighlight();

        // Apply new index
        _currentIndex = newIndex;
        slots[_currentIndex].ToggleHighlight();
        Debug.Log("🎯 Hotbar -> " + newIndex);
    }
}
