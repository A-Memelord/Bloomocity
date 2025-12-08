using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;
    public float InteractEndDistance = 1.5f;

    private IInteractable currentInteractable;

    public bool IsInteracting { get; private set; }

    private void Update()
    {
        // Detect interactables nearby
        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        // Press E to interact
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                {
                    StartInteraction(interactable);
                    return;
                }
            }
        }

        // Auto-close if player moves too far from the chest
        if (IsInteracting && currentInteractable != null)
        {
            float distance = Vector3.Distance(
                InteractionPoint.position,
                (currentInteractable as MonoBehaviour).transform.position
            );

            if (distance > InteractEndDistance)
            {
                EndInteraction();
            }
        }
    }

    public void StartInteraction(IInteractable interactable)
    {
        currentInteractable = interactable;
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    public void EndInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.EndInteraction(); // tell the chest to close its UI
            currentInteractable = null;
        }

        IsInteracting = false;
    }
}
