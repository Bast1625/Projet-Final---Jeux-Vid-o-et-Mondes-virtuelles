using System.Linq;

public class DrawingOnTheBoard : Task
{   
    private Whiteboard whiteboard;

    public override bool CanExist(TaskManager taskManager)
    {
        Whiteboard[] whiteboards = taskManager.FindAvailableTaskables<Whiteboard>()
        .Where(whiteboard => whiteboard.IsClean)
        .Where(whiteboard => RoomManager.IsInRoom(whiteboard.gameObject))
        .ToArray();

        return whiteboards.Any();
    }
    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);

        Whiteboard[] whiteboards = taskManager.FindAvailableTaskables<Whiteboard>()
        .Where(whiteboard => whiteboard.IsClean)
        .Where(whiteboard => RoomManager.IsInRoom(whiteboard.gameObject))
        .ToArray();
        
        whiteboard = whiteboards[UnityEngine.Random.Range(0, whiteboards.Length)];
    }

    public override void Activate()
    {
        Room room = RoomManager.GetRoomOf(whiteboard.gameObject);

        Name = "Erase the whiteboard";
        Location = $"Laboratory {room.Name}";

        whiteboard.Draw();
        whiteboard.OnErase += Complete;
    }

    public override void Complete()
    {
        whiteboard.OnErase -= Complete;
        
        taskManager.CompleteTask(this);
    }

    public override void Cancel()
    {
        whiteboard.OnErase -= Complete;

        if(whiteboard.IsDirty)
            whiteboard.Erase();
        
        taskManager.CancelTask(this);
    }

    public override ITaskable GetTaskable() => whiteboard;

    public DrawingOnTheBoardData Save()
    {
        DrawingOnTheBoardData data = new DrawingOnTheBoardData()
        {
            Id = Id,

            Name = Name,
            Location = Location,

            WhiteboardId = whiteboard.Id,

            DrawingPosition = whiteboard.Drawing.transform.position,
            DrawingRotation = whiteboard.Drawing.transform.rotation,
            DrawingScale = whiteboard.Drawing.transform.localScale,
            DrawingTextureName = whiteboard.Drawing.Texture.name,
            DrawingOpacity = whiteboard.Drawing.Opacity
        };

        return data;
    }

    public void Load(TaskManager taskManager, DrawingOnTheBoardData data)
    {
        this.taskManager = taskManager;

        Name = data.Name;
        Location = data.Location;

        whiteboard = taskManager.FindTaskable<Whiteboard>(data.WhiteboardId);

        Drawing newDrawing = DrawingManager.Instance.GenerateDrawing(
            data.DrawingScale, data.DrawingTextureName
        );

        newDrawing.transform.position = data.DrawingPosition;
        newDrawing.transform.rotation = data.DrawingRotation;

        whiteboard.SetDrawing(newDrawing);

        whiteboard.OnErase += Complete;
    }
}
