using System.Collections.Generic;
using DimensionUtility;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TMP : MonoBehaviour
{
    public Room room;

    private void Awake()
    {
        RoomManager.OnRoomEnter += (room) => {
            Debug.Log(room.Name);
        };  
    }

    private void OnDrawGizmos()
    {
        room.floor.SetDimensions();

        Dimensions dimensions = room.floor.Dimensions; 

        Gizmos.DrawCube(dimensions.Center, dimensions.ScaleAbs * 2f);
    }

    /*public GameObject go;

    private void OnDrawGizmos()
    {
        Dimensions d = DimensionsScanner.Instance.PerformScanOn(go);

        Gizmos.DrawSphere(d.TopFace, 0.05f);
        Gizmos.DrawSphere(d.BottomFace, 0.05f);
        Gizmos.DrawSphere(d.LeftFace, 0.05f);
        Gizmos.DrawSphere(d.RightFace, 0.05f);
        Gizmos.DrawSphere(d.FrontFace, 0.05f);
        Gizmos.DrawSphere(d.BackFace, 0.05f);
    }*/
}