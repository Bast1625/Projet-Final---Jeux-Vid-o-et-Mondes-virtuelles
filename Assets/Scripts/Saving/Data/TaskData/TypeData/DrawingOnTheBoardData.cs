using System;
using UnityEngine;

[Serializable]
public class DrawingOnTheBoardData : TaskData
{
    public string WhiteboardId;

    public Vector3 DrawingPosition;
    public Quaternion DrawingRotation;
    public Vector3 DrawingScale;
    public string DrawingTextureName;
    public float DrawingOpacity;
}