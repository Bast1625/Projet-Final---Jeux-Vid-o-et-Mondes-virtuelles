using UnityEngine;

public class Hand : PlayerComponent, IHolder
{
    [SerializeField] Transform rightHandView;
    [SerializeField] Transform bothHandsView;

    public Pad Pad;

    public Holdable Holdable { get; private set; }
    public bool IsEmpty { get => Holdable == null; }
    public bool IsFull { get => !IsEmpty; }

    public void Grab(Holdable holdable)
    {
        Holdable = holdable;

        Holdable.collider.enabled = false;
        Holdable.rigidbody.isKinematic = true;

        if(holdable is Disposable)
            holdable.AttachTo(this, bothHandsView);
        else
            holdable.AttachTo(this, rightHandView);
    }
    public Holdable Drop()
    {
        Holdable holdableToDrop = Holdable;

        Holdable.Detach();

        Holdable.collider.enabled = true;
        Holdable.rigidbody.isKinematic = false;

        Holdable = null;

        return holdableToDrop;
    }

    public void ExchangeWith(IHolder previousHolder)
    {
        Holdable holdableToGrab = previousHolder.Drop();

        Grab(holdableToGrab);
    }

    public bool IsHolding(Holdable holdable) => Holdable == holdable;
}