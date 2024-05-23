using System;
using UnityEngine;

[Serializable]
public class ChairData : BaseData
{
    public Vector3 TopStructurePosition;
    public Quaternion TopStructureRotation;

    public Vector3 BottomStructurePosition;
    public Quaternion BottomStructureRotation;
}