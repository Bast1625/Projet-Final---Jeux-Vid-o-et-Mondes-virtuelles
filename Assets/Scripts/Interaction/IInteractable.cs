using UnityEngine.InputSystem;

public interface IInteractable
{
    public void Interact(Player interactor, InputActionPhase interactionPhase);
    public InteractionFeedback Feedback(Player interactor);
}