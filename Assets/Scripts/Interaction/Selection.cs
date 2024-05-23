public class Selection
{
    public static Selection None = new Selection {
        Interactable = null,
        Feedback = InteractionFeedback.None
    };

    public IInteractable Interactable;
    public InteractionFeedback Feedback;
    
    public bool IsValid { get => Interactable != null && Feedback != InteractionFeedback.None; }
    public bool IsInvalid { get => !IsValid; }
}