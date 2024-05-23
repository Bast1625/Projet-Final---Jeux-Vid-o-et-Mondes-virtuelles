using System;
using UnityEngine;

public class LockerBore : Holder
{
    [SerializeField] LockerDoor lockerDoor;

    protected override void InteractionStart(Player interactor)
    {
        if(IsEmpty)
        {
            ExchangeWith(interactor.Hand);

            lockerDoor.Lock();
        }
        else 
        if(Holdable is Padlock)
        {
            if(interactor.Hand.Holdable is Tool)
            {
                Drop();

                lockerDoor.Unlock();
            }
            else
                lockerDoor.TryUnlock();
        }
        
        
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if(lockerDoor.IsOpened)
            return InteractionFeedback.None;

        if(IsEmpty)
            if(interactor.Hand.Holdable is Padlock)
                return new InteractionFeedback 
                {
                    Status = InteractionStatus.Valid,
                    Message = $"Lock {lockerDoor.name}"
                };

        if(Holdable is Padlock)
            if(interactor.Hand.Holdable is Tool)
                return new InteractionFeedback
                {
                    Status = InteractionStatus.Valid,
                    Message = $"Break {lockerDoor.name}"
                };
            else
                return new InteractionFeedback
                {
                    Status = InteractionStatus.Invalid,
                    Message = $"Break {lockerDoor.name}"
                };

        return InteractionFeedback.None;
    }
}