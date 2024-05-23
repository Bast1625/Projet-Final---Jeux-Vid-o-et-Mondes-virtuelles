public class InteractionFeedback
{
    public static InteractionFeedback None = new InteractionFeedback { 
        Status = InteractionStatus.Neutral,
        Message = ""
    };

    public InteractionStatus Status;
    public string Message;
}