using System;
using UnityEngine;

[Serializable]
public class PlayerData : BaseData
{
    public Vector3 HeadPosition;
    public Quaternion HeadRotation;

    public Vector3 CameraPosition;
    public Quaternion CameraRotation;

    public bool IsDriving;
}