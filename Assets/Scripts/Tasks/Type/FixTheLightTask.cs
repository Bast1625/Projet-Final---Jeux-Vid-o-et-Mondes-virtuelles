using System.Linq;

public class FixTheLight : Task
{
    private Lightbulb lightbulb;

    public override bool CanExist(TaskManager taskManager)
    {
        Lightbulb[] lightbulbs = taskManager.FindAvailableTaskables<Lightbulb>()
        .Where(lightbulb => lightbulb.IsOn && lightbulb.IsFixed)
        .ToArray();

        return lightbulbs.Any();
    }

    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);

        Lightbulb[] lightbulbs = taskManager.FindAvailableTaskables<Lightbulb>()
        .Where(lightbulb => lightbulb.IsOn && lightbulb.IsFixed)
        .ToArray();

        lightbulb = Utility.SelectAtRandom(lightbulbs);
    }

    public override void Activate()
    {
        Room room = RoomManager.GetRoomOf(lightbulb.gameObject);
        
        Name = "Fix the light";
        Location = $"Laboratory {room.Name}";

        lightbulb.OnFix += Complete;

        lightbulb.Break();
    }

    public override void Cancel()
    {
        lightbulb.OnFix -= Complete;

        lightbulb.Fix();
        
        taskManager.CancelTask(this);
    }

    public override void Complete()
    {
        lightbulb.OnFix -= Complete; 

        taskManager.CompleteTask(this);
    }

    public override ITaskable GetTaskable() => lightbulb;

    public FixTheLightData Save()
    {
        FixTheLightData data = new FixTheLightData
        {
            Id = Id,

            Name = Name,
            Location = Location,

            LightbulbId = lightbulb.Id
        };

        return data;
    }

    public void Load(TaskManager taskManager, FixTheLightData data)
    {   
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        lightbulb = taskManager.FindTaskable<Lightbulb>(data.LightbulbId);

        lightbulb.OnFix += Complete;
    }
}
