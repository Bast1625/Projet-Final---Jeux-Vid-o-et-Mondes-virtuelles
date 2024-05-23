using System;

[Serializable]
public class DoorData : BaseData
{
    public bool IsOpened;
    public bool IsLocked;

    public bool IsOpening;
    public float Opening;
    public float Velocity;
    public float Acceleration;
}