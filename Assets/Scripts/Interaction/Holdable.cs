using System;
using UnityEngine;

public class Holdable : Interactable
{
    public event Action OnInitialize;
    public event Action OnGrab;
    //public event Action OnDrop;

    public IHolder Holder { get; private set; }
    public bool IsHeld { get => Holder != null; }

    public new Collider collider { get; private set; }
    public new Rigidbody rigidbody { get; private set; }

    protected override void Initialize()
    {
        base.Initialize();

        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();

        OnInitialize?.Invoke();
    }

    protected override void InteractionStart(Player interactor)
    {
        Grab(interactor.Hand);
    }

    public void Grab(IHolder holder)
    {
        if (IsHeld)
            holder.ExchangeWith(Holder);
        else
            holder.Grab(this);

        OnGrab?.Invoke();
    }

    public void AttachTo(IHolder holder, Transform view)
    {
        Holder = holder;

        transform.SetParent(view.transform, true);
        transform.position = view.position;
        transform.rotation = view.rotation;
    }
    public void Detach()
    {
        Holder = null;

        transform.SetParent(null);
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if (interactor.Hand.IsFull)
            return InteractionFeedback.None;

        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"{(interactor.Hand.IsHolding(this) ? "Drop" : "Grab")}"
        };
    }

   
}