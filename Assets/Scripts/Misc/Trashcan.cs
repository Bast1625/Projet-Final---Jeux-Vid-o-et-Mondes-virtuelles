public class Trashcan : Interactable, ITaskable
{
    public string Id { get => $"{GetType()}_{name}"; }
    
    protected override void InteractionStart(Player interactor)
    {
        Disposable toThrow = (Disposable) interactor.Hand.Drop();

        toThrow.Dispose();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if(interactor.Hand.Holdable is not Disposable)
            return InteractionFeedback.None;
        
        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"Throw out trashbag"
        };
    }
}