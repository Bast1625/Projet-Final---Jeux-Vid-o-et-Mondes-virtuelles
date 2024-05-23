using System.Data.Common;
using System.Diagnostics;
using System.Linq;

public class LockTheDoor : Task
{
    private Door door;

    public override bool CanExist(TaskManager taskManager)
    {
        Door[] doors = taskManager.FindAvailableTaskables<Door>()
        .Where(door => door.IsUnlocked)
        .ToArray();

        return doors.Any();
    }

    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);

        Door[] doors = taskManager.FindAvailableTaskables<Door>().ToArray()
        .Where(door => door.IsUnlocked)
        .ToArray();

        door = Utility.SelectAtRandom(doors);

        Name = "Lock the door";
        Location = door.GetName();
    }

    public override void Activate()
    {
        door.OnLock += Complete;
    }

    public override void Cancel()
    {
        door.OnLock -= Complete;

        taskManager.CancelTask(this);
    }

    public override void Complete()
    {   
        door.OnLock -= Complete;

        taskManager.CompleteTask(this);
    }

    public override ITaskable GetTaskable() => door;

    public LockTheDoorData Save()
    {
        LockTheDoorData data = new LockTheDoorData
        {
            Id = Id,

            Name = Name,
            Location = Location,

            DoorId = door.Id
        };

        return data;
    }

    public void Load(TaskManager taskManager, LockTheDoorData data)
    {   
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        door = taskManager.FindTaskable<Door>(data.DoorId);

        door.OnLock += Complete;
    }
}