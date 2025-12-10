using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionRadius = 1.2f;
    public float endInteractionDistance = 2f;

    private IInteractable currentInteractable;
    private IInteractable hoveredInteractable;

    public bool IsInteracting { get; private set; }

    private void Update()
    {
        DetectClosestInteractable();

        // Press E to interact OR close
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // If already interacting with something -> close it
            if (IsInteracting)
            {
                EndInteraction();
                return;
            }

            // If not interacting, but have a chest in range -> open it
            if (hoveredInteractable != null)
            {
                StartInteraction(hoveredInteractable);
            }
        }

        // Auto-close if player walks away
        if (IsInteracting && currentInteractable != null)
        {
            float dist = Vector3.Distance(
                interactionPoint.position,
                (currentInteractable as MonoBehaviour).transform.position
            );

            if (dist > endInteractionDistance)
            {
                EndInteraction();
            }
        }
    }


    private void DetectClosestInteractable()
    {
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionRadius, interactionLayer);

        if (colliders.Length == 0)
        {
            hoveredInteractable = null;
            return;
        }

        hoveredInteractable = colliders
            .Select(c => c.GetComponent<IInteractable>())
            .Where(i => i != null)
            .OrderBy(i =>
                Vector3.Distance(
                    interactionPoint.position,
                    (i as MonoBehaviour).transform.position
                )
            )
            .FirstOrDefault();
    }

    public void StartInteraction(IInteractable interactable)
    {
        if (IsInteracting && currentInteractable == interactable)
            return; // prevent spam / double-open

        currentInteractable = interactable;
        interactable.Interact(this, out bool success);

        if (success)
            IsInteracting = true;
    }

    public void EndInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.EndInteraction(); // close UI
            currentInteractable = null;
        }

        IsInteracting = false;
    }
}
