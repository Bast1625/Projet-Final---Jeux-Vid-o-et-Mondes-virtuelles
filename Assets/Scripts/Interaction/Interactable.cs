using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour, IInteractable
{
    private void Start()
    {
        Initialize();
    }
    
    protected virtual void Initialize() { }

    public virtual void Interact(Player interactor, InputActionPhase interactionPhase)
    {
        if(interactionPhase == InputActionPhase.Performed)
            InteractionStart(interactor);

        if (interactionPhase == InputActionPhase.Canceled)
            InteractionEnd(interactor);
    }

    protected virtual void InteractionStart(Player interactor) { }

    protected virtual void InteractionEnd(Player interactor) { }

    public virtual InteractionFeedback Feedback(Player interactor)
    {
        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"Interact with {GetName()}"
        };
    }

    public virtual string GetName() => name;
}