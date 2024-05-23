using UnityEngine;
using System.Linq;

public class ReplaceTheChair : Task
{
    [SerializeField] GameObject chairOutline;

    private Chair chair;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public override bool CanExist(TaskManager taskManager)
    {
        Chair[] chairs = taskManager.FindAvailableTaskables<Chair>()
        .Where(chair => RoomManager.IsInRoom(chair.gameObject)).ToArray();

        return chairs.Any();
    }
    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);
        
        Chair[] chairs = taskManager.FindAvailableTaskables<Chair>()
        .Where(chair => RoomManager.IsInRoom(chair.gameObject)).ToArray();;

        chair = Utility.SelectAtRandom(chairs);
    }
    public override void Activate()
    {
        initialPosition = ChairManager.GetInitialPosition(chair);
        initialRotation = ChairManager.GetInitialRotation(chair);

        Name = "Replace a chair";
        Location = RoomManager.GetRoomOf(chair.gameObject).Name;

        chair.OnHold += ShowOutline;
        chair.OnRelease += HideOutline;
        chair.OnRelease += CheckForRightSpot;

        chairOutline.transform.position = initialPosition;
        chairOutline.transform.rotation = initialRotation;

        RoomManager.GetRoomOf(chair.gameObject).SelectPositionAtRandom(chair.gameObject, out Vector3 randomPosition);

        randomPosition.y = initialPosition.y;
        
        chair.transform.position = randomPosition;
    }

    public override void Complete()
    {
        chair.OnHold -= ShowOutline;
        chair.OnRelease -= HideOutline;
        chair.OnRelease -= CheckForRightSpot;

        chair.transform.position = initialPosition;
        chair.transform.rotation = initialRotation;

        taskManager.CompleteTask(this);
    }

    public override void Cancel()
    {
        chair.OnHold -= ShowOutline;
        chair.OnRelease -= HideOutline;
        chair.OnRelease -= CheckForRightSpot;

        chair.transform.position = initialPosition;
        chair.transform.rotation = initialRotation;

        taskManager.CancelTask(this);
    }

    public void ShowOutline() => chairOutline.SetActive(true);
    public void HideOutline() => chairOutline.SetActive(false);
    
    private void CheckForRightSpot()
    {
        float distance = Vector3.Distance(initialPosition, chair.transform.position);
        if(distance < 1f)
            Complete();
    }

    public override ITaskable GetTaskable() => chair;

    public ReplaceTheChairData Save()
    {
        ReplaceTheChairData data = new ReplaceTheChairData 
        {
            Id = Id,

            Name = Name,
            Location = Location,
            
            ChairId = chair.Id
        };

        return data; 
    }

    public void Load(TaskManager taskManager, ReplaceTheChairData data)
    {
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        chair = taskManager.FindTaskable<Chair>(data.ChairId);

        chair.OnHold += ShowOutline;
        chair.OnRelease += HideOutline;
        chair.OnRelease += CheckForRightSpot;
    }
}