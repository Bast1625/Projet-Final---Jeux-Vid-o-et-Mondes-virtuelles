using System;
using UnityEngine;

public class PickupTheTrash : Task
{
    [SerializeField] Trashbag trashbagPrefab;

    public Trashbag trashbag;

    public override bool CanExist(TaskManager taskManager)
    {
        return true;
    }

    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);
        
        trashbag = Instantiate(trashbagPrefab);
    }

    public override void Activate()
    {
        Room room = RoomManager.SelectAtRandom();

        Name = "Pickup the trash";
        Location = room.Name;
        
        bool canBePlaced = room.SelectPositionAtRandom(trashbag.gameObject, out Vector3 randomPosition); 
        
        if( !canBePlaced )
        {
            Cancel();

            return;
        }
        
        trashbag.transform.position = randomPosition;

        trashbag.OnDispose += Complete;
    }

    public override void Cancel()
    {
        trashbag.OnDispose -= Complete;

        Destroy(trashbag.gameObject);

        taskManager.CancelTask(this);
    }

    public override void Complete()
    {
        trashbag.OnDispose -= Complete;

        Destroy(trashbag.gameObject);

        taskManager.CompleteTask(this);
    }

    public override ITaskable GetTaskable() => trashbag;

    public PickupTheTrashData Save()
    {
        PickupTheTrashData data = new PickupTheTrashData()
        {
            Id = Id,

            Name = Name,
            Location = Location,

            TrashbagPosition = trashbag.transform.position,
            TrashbagRotation = trashbag.transform.rotation
        };

        return data;
    }

    public void Load(TaskManager taskManager, PickupTheTrashData data)
    {
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        trashbag = Instantiate(trashbagPrefab, data.TrashbagPosition, data.TrashbagRotation);

        trashbag.OnDispose += Complete;
    }
}