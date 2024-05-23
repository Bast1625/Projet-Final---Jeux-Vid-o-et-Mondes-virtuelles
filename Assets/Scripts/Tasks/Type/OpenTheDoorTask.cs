using System;
using System.Linq;
using UnityEngine;

public class OpenTheDoor : Task
{
    private Door door;

    public override bool CanExist(TaskManager taskManager)
    {
        Door[] doors = taskManager.FindAvailableTaskables<Door>()
        .Where(door => door.IsClosed)
        .ToArray();

        return doors.Any();
    }

    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);

        Door[] doors = taskManager.FindAvailableTaskables<Door>().ToArray()
        .Where(door => door.IsClosed)
        .ToArray();

        door = Utility.SelectAtRandom(doors);

        Name = "Open the door";
        Location = door.name;
    }

    public override void Activate()
    {
        door.OnOpen += Complete;
    }

    public override void Cancel()
    {
        door.OnOpen -= Complete;

        taskManager.CancelTask(this);
    }

    public override void Complete()
    {   
        door.OnOpen -= Complete;

        taskManager.CompleteTask(this);
    }

    public override ITaskable GetTaskable() => door;

    public OpenTheDoorData Save()
    {
        OpenTheDoorData data = new OpenTheDoorData
        {
            Id = Id,

            Name = Name,
            Location = Location,

            DoorId = door.Id
        };

        return data;
    }

    public void Load(TaskManager taskManager, OpenTheDoorData data)
    {   
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        door = taskManager.FindTaskable<Door>(data.DoorId);

        door.OnOpen += Complete;
    }
}