using System;
using System.Collections;
using UnityEngine;

public class PushDoor : Door, ISavable
{
    public event Action OnOpening;

    public event Action OnPlayerHold;
    public event Action OnPlayerRelease;

    [SerializeField] float pushForce;
    [SerializeField] float openingAcceleration;
    [SerializeField] float closingAcceleration;

    public bool IsOpening { get; private set; }
    public bool IsHeld { get; private set; }

    private float opening;
    private float velocity = 0f;
    private float acceleration = 0f;

    private Coroutine currentSequence;

    private void Update()
    {
        opening = Opening;
    }

    protected override void InteractionStart(Player interactor)
    {
        if(IsLocked)
        {
            TryOpen();

            return;
        }

        if(!IsOpening)
            velocity = pushForce;
        else
            velocity = pushForce / 2f;
        
        if(velocity < 0)
            velocity = 0f;

        acceleration = openingAcceleration;
        
        StartSequence();

        OnPlayerHold?.Invoke();
    }
    protected override void InteractionEnd(Player interactor)
    {
        acceleration = closingAcceleration;
        
        OnPlayerRelease?.Invoke();
    }

    private IEnumerator MovementSequence()
    {
        float previousOpening;

        if(IsClosed)
            Open();

        while(true)
        {
            previousOpening = Opening;

            float nextMovement = velocity * Time.fixedDeltaTime;

            AddOpening(nextMovement);

            velocity += acceleration * Time.fixedDeltaTime;

            OnOpening?.Invoke();

            if(Opening == 0f && previousOpening > 0f)
            {
                Close();

                break;
            }
            
            if(Opening == 100f && previousOpening < 100f)
                velocity = 0f;
            
            if(Opening == 100f && acceleration > 0f)
                velocity = 0f;

            if(Opening < 50f && previousOpening > 50f && acceleration < 0f)
            {
                velocity = Math.Clamp(velocity, -3f, 0f);
                acceleration = 0f;
            }

            yield return null;
        }

        StopSequence();
    }

    private void StartSequence()
    {
        IsOpening = true;

        if(currentSequence != null)
            StopCoroutine(currentSequence);

        currentSequence = StartCoroutine(nameof(MovementSequence));   
    }
    private void StopSequence()
    {
         IsOpening = false;

        if(currentSequence != null)
        {
            StopCoroutine(currentSequence);
            
            currentSequence = null;
        }

        velocity = 0f;
        acceleration = 0f;
    }

    public void Save(GameData gameData)
    {
        DoorData doorData = new DoorData 
        {
            Id = Id,
            Position = transform.position,
            Rotation = transform.rotation,

            IsOpened = IsOpened,
            IsLocked = IsLocked,

            IsOpening = IsOpening,
            Opening = Opening,
            Velocity = velocity,
            Acceleration = acceleration
        };

        gameData.DoorsData.Add(doorData);
    }

    public void Load(GameData gameData)
    {
        DoorData doorData = gameData.DoorsData.Find(data => data.Id == Id);

        transform.position = doorData.Position;
        transform.rotation = doorData.Rotation;

        IsOpened = doorData.IsOpened;
        IsLocked = doorData.IsLocked;

        IsOpening = doorData.IsOpening;

        velocity = doorData.Velocity;
        acceleration = doorData.Acceleration == 0 ? 0 : closingAcceleration;
    
        SetOpening(doorData.Opening);

        if(IsOpening)
            StartSequence();
    }
}