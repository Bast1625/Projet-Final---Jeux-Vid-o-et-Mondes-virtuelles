using System;

[Serializable]
public class TaskManagerData : BaseData
{
    public int CompletedCount;

    public PickupTheTrashData[] PickupTheTrashTasks;
    public DrawingOnTheBoardData[] DrawingOnTheBoardTasks;
    public LockTheDoorData[] LockTheDoorTasks;
    public ReplaceTheChairData[] ReplaceTheChairTasks;
    public OpenTheDoorData[] OpenTheDoorTasks;
    public FixTheLightData[] FixTheLightTasks;
}