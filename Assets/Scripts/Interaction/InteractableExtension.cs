using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableExtension : MonoBehaviour, IInteractable
{
    [SerializeField] Interactable interactable;

    public void Interact(Player interactor, InputActionPhase interactionPhase) => interactable.Interact(interactor, interactionPhase);
    public InteractionFeedback Feedback(Player interactor) => interactable.Feedback(interactor);
}