using System.Linq;

public class TurnOffTheLights : Task
{
    private Lightswitch lightSwitch;

    public override bool CanExist(TaskManager taskManager)
    {
        Lightswitch[] lightSwitches = taskManager.FindAvailableTaskables<Lightswitch>()
        .Where(lightSwitch => lightSwitch.IsOn)
        .ToArray();

        return lightSwitches.Any();
    }

    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);

        Lightswitch[] lightSwitches = taskManager.FindAvailableTaskables<Lightswitch>()
        .Where(lightSwitch => lightSwitch.IsOn)
        .ToArray();

        lightSwitch = Utility.SelectAtRandom(lightSwitches);
    }

    public override void Activate()
    {
        Room room = RoomManager.GetRoomOf(lightSwitch.gameObject);

        Name = "Turn off the lights";
        Location = room.Name;

        lightSwitch.OnTurnOff += Complete;
    }

    public override void Cancel()
    {
        lightSwitch.OnTurnOff -= Complete;

        taskManager.CancelTask(this);
    }

    public override void Complete()
    {   
        lightSwitch.OnTurnOff -= Complete;

        taskManager.CompleteTask(this);
    }

    public override ITaskable GetTaskable() => lightSwitch; 
}