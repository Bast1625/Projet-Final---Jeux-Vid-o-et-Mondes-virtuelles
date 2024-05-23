using UnityEngine;

public class Handle : Interactable
{
    [SerializeField] private Door door;
    [SerializeField] private Key key;

    protected override void Initialize()
    {
        if(key == null)
            return;
            
        key.LinkTo(door);
    }

    protected override void InteractionStart(Player interactor)
    {
        if (interactor.Hand.IsHolding(key))
        {
            if (door.IsLocked)
                door.Unlock();
            else
                door.Lock();
        }
        else
        {
            if (door.IsLocked)
                door.TryUnlock();
            else
                door.TryLock();
        }
    }

    public override InteractionFeedback Feedback(Player interactor)
    {          
        if (door.IsOpened)
            return InteractionFeedback.None;

        return new InteractionFeedback
        {
            Status = interactor.Hand.IsHolding(key) ? InteractionStatus.Valid : InteractionStatus.Invalid,
            Message = $"{(door.IsLocked ? "Unlock" : "Lock")} {door.name}"
        };
    }
}