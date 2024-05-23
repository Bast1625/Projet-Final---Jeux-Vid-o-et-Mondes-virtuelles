using UnityEngine;

public class Holder : Interactable, IHolder
{
    [SerializeField] private Transform view;
    public Holdable Holdable { get; private set; }

    public bool IsEmpty { get => Holdable == null; }
    public bool IsFull { get => !IsEmpty; }

    protected override void InteractionStart(Player interactor)
    {
        if(IsEmpty)
            ExchangeWith(interactor.Hand);
    }

    public void Grab(Holdable holdable)
    {
        Holdable = holdable;

        Holdable.collider.isTrigger = true;
        Holdable.rigidbody.isKinematic = true;

        holdable.AttachTo(this, view);
    }
    public Holdable Drop()
    {
        Holdable.Detach();

        Holdable.collider.isTrigger = false;
        Holdable.rigidbody.isKinematic = false;
        
        Holdable holdableToDrop = Holdable;
        Holdable = null;

        return holdableToDrop;
    }

    public void ExchangeWith(IHolder previousHolder)
    {
        Holdable holdableToGrab = previousHolder.Drop();

        Grab(holdableToGrab);
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if (interactor.Hand.IsEmpty)
            return InteractionFeedback.None;
        if (IsFull)
            return InteractionFeedback.None;

        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"Deposit {interactor.Hand.Holdable.name}"
        };
    }
}