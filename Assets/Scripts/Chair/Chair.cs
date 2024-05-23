using System;
using System.Collections;
using UnityEngine;

public class Chair : Interactable, ITaskable, ISavable
{   
    public event Action OnHold;
    public event Action OnRelease;
    public event Action OnDrag;

    public string Id { get => $"{GetType()}_{name}"; }

    [SerializeField] private Transform topStructure;
    [SerializeField] private Transform bottomStructure;

    private Coroutine draggingSequence;

    protected override void InteractionStart(Player interactor)
    {
        if(draggingSequence != null)
            StopCoroutine(draggingSequence);

        draggingSequence = StartCoroutine(DraggingSequence(interactor.Head));

        OnHold?.Invoke();
    }
    protected override void InteractionEnd(Player interactor)
    {
        if(draggingSequence != null)
                StopCoroutine(draggingSequence);

        OnRelease?.Invoke();
    }

    private IEnumerator DraggingSequence(Head head)
    {
        while(true)
        {   
            RaycastHit fromMouseToWorld = head.GetMousePosition();
          
            Vector3 mousePosition = fromMouseToWorld.point;
            Vector3 position = new Vector3(
                mousePosition.x,
                transform.position.y,
                mousePosition.z
            );

            transform.position = Vector3.Lerp(transform.position, position, 10f * Time.deltaTime);

            topStructure.rotation = Quaternion.Lerp(
                topStructure.rotation,
                Quaternion.Euler(
                    topStructure.eulerAngles.x,
                    head.transform.eulerAngles.y,
                    topStructure.eulerAngles.z
                ), 10f * Time.deltaTime 
            );

            topStructure.localEulerAngles = new Vector3(
                    0,
                    topStructure.localEulerAngles.y,
                    0
                );

            OnDrag?.Invoke();
            
            yield return null;
        }
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        return new InteractionFeedback {
            Status = InteractionStatus.Valid,
            Message = ""
        };
    }

    public void Save(GameData gameData)
    {
        ChairData chairData = new ChairData {
            Id = Id,

            Position = transform.position,
            Rotation = transform.rotation,

            TopStructurePosition = topStructure.position,
            TopStructureRotation = topStructure.rotation,

            BottomStructurePosition = bottomStructure.position,
            BottomStructureRotation = bottomStructure.rotation,
        };

        gameData.ChairsData.Add(chairData);
    }
    public void Load(GameData gameData)
    {
        ChairData data = gameData.ChairsData.Find(_data => _data.Id == Id);
        
        transform.position = data.Position;
        transform.rotation = data.Rotation;

        topStructure.position = data.TopStructurePosition;
        topStructure.rotation = data.TopStructureRotation;

        bottomStructure.position = data.BottomStructurePosition;
        bottomStructure.rotation = data.BottomStructureRotation;
    }
}