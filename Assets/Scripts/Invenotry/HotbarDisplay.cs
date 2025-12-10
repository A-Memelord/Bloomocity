using UnityEngine;

public class HotbarDisplay : StaticInventoryDisplay
{
    private int _maxIndexSize;
    private int _currentIndex;

    protected override void Start()
    {
        base.Start();

        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("❌ HotbarDisplay: slots[] is EMPTY! Did you assign the slots in the Inspector?");
            return;
        }

        _currentIndex = 0;
        _maxIndexSize = slots.Length - 1;

        slots[_currentIndex].ToggleHighlight();
        Debug.Log("✅ HotbarDisplay started. Slots found: " + slots.Length);
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

        Debug.Log("Scrolling value: " + Input.GetAxis("Mouse ScrollWheel"));
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0.1f) ChangeIndex(1);
        if (scroll < -0.1f) ChangeIndex(-1);
    }

    private void CheckUseItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var slot = slots[_currentIndex].AssignedInventorySlot;

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

            Debug.Log("▶ Using item: " + slot.ItemData.name);
            slot.ItemData.UseItem();
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
