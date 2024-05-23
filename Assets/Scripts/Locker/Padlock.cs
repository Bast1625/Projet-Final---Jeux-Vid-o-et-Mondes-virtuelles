using Unity.VisualScripting;
using UnityEngine;

public class Padlock : Holdable
{
    [SerializeField] Tool tool;

    public override InteractionFeedback Feedback(Player interactor)
    {
        if(Holder is LockerBore)
            return InteractionFeedback.None;
            
        return base.Feedback(interactor);
    }
}