using UnityEngine;

public class CartHandle  : Interactable
{
    [SerializeField] Sef behind;
    [SerializeField] Transform position;

    protected override void InteractionStart(Player interactor)
    {
        interactor.SetPositionTo(position.position - (Vector3.up * (position.position.y - interactor.transform.position.y)));
        interactor.SetRotationTo(position.rotation);

        interactor.SwitchToCartState();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if(interactor.IsDriving)
            return InteractionFeedback.None;

        if(interactor.Hand.IsFull)
            return InteractionFeedback.None;
            
        if(behind.IsPlayerBehind)
            return new InteractionFeedback {
                Status = InteractionStatus.Valid,
                Message = "Drive"
            };

        return InteractionFeedback.None;
    }
}