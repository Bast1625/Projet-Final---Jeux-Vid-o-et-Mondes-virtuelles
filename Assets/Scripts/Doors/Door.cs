using System;
using UnityEngine;

public class Door : Interactable, ITaskable
{
    public event Action OnOpen;
    public event Action OnClose;
    public event Action OnLock;
    public event Action OnUnlock;
    public event Action OnTryOpen;
    public event Action OnTryClose;
    public event Action OnTryLock;
    public event Action OnTryUnlock;

    public string Id { get => $"{GetType()}_{name}"; }
    
    [SerializeField] Room room;

    [SerializeField] Transform hinge;

    public bool IsOpened { get; protected set; }
    public bool IsClosed { get => !IsOpened; }

    public bool IsLocked { get; protected set; }
    public bool IsUnlocked { get => !IsLocked; }
    
    public float Opening { get; private set; }

    public virtual void Open() { IsOpened = true;  OnOpen?.Invoke(); }
    public virtual void Close() { IsOpened = false; OnClose?.Invoke(); }
    public virtual void Unlock() { IsLocked = false; OnUnlock?.Invoke(); }
    public virtual void Lock() { IsLocked = true; OnLock?.Invoke(); }
    public virtual void TryOpen() { OnTryOpen?.Invoke(); }
    public virtual void TryClose() { OnTryClose?.Invoke(); }
    public virtual void TryLock() { OnTryLock?.Invoke(); }
    public virtual void TryUnlock() { OnTryUnlock?.Invoke(); }

    protected void SetOpening(float newOpening)
    {
        float previousOpening = Opening;

        Opening = Mathf.Clamp(newOpening, 0, 100);
        UpdateRotation();

        if(previousOpening == 0  && Opening > 0)
            Open();
        if(previousOpening > 0 && Opening == 0)
            Close();
    }
    protected void AddOpening(float toAddOpening)
    {
        SetOpening(Opening + toAddOpening);
    }

    private void UpdateRotation()
    {
        hinge.localEulerAngles = new Vector3(
            hinge.eulerAngles.x,
            Opening * (-90 - 0) / 100,
            hinge.eulerAngles.z
        );
    }

    protected override void InteractionStart(Player interactor)
    {
        if (IsLocked)
        {
            TryOpen();

            return;
        }

        if (IsOpened)
            Close();
        else
            Open();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        if (IsLocked)
            return new InteractionFeedback
            {
                Status = InteractionStatus.Invalid,
                Message = $"{GetName()} (Locked)"
            };

        return new InteractionFeedback
        {
            Status = InteractionStatus.Valid,
            Message = $"{(IsOpened ? "Close" : "Open")} {GetName()}"
        };
    }

    public override string GetName() => room.Name;
}