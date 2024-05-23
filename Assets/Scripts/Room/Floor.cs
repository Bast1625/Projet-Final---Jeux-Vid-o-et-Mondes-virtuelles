using System.Linq;
using UnityEngine;
using DimensionUtility;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class Floor : MonoBehaviour
{
    public Room Room { get; private set; }
    public GameObject[] Tiles { get; private set; }
    public Dimensions Dimensions { get; private set; }

    public void Initialize(Room room)
    {
        Room = room;
        
        SetTiles();
        SetDimensions();
    }

    private void SetTiles()
    {
        GameObject[] tiles = new GameObject[transform.childCount];
        
        for(int floorPieceID = 0; floorPieceID < tiles.Length; floorPieceID++)
            tiles[floorPieceID] = transform.GetChild(floorPieceID).gameObject; 

        Tiles = tiles;
    }
    public void SetDimensions()
    {
        Dimensions[] dimensions = Tiles.Select(x => DimensionsScanner.Instance.PerformScanOn(x.gameObject)).ToArray();

        Vector3 averageCenter = dimensions.Select(x => x.Center).Aggregate(Vector3.zero, (total, position) => total + position) / dimensions.Length;

        KeyValuePair<Dimensions, Vector3>[] leftmostPointPerDimension = dimensions.Select(x => {
            return new KeyValuePair<Dimensions, Vector3>(
                x,
                x.All.Aggregate((farthest, current) => (current.x < farthest.x) ? current : farthest)
            );
        }).ToArray();

        KeyValuePair<Dimensions, Vector3>[] rightmostPointPerDimension = dimensions.Select(x => {
            return new KeyValuePair<Dimensions, Vector3>(
                x,
                x.All.Aggregate((farthest, current) => (current.x > farthest.x) ? current : farthest)
            );
        }).ToArray();

        KeyValuePair<Dimensions, Vector3>[] topmostPointPerDimension = dimensions.Select(x => {
            return new KeyValuePair<Dimensions, Vector3>(
                x,
                x.All.Aggregate((farthest, current) => (current.z > farthest.z) ? current : farthest)
            );
        }).ToArray();

        KeyValuePair<Dimensions, Vector3>[] bottommostPointPerDimension = dimensions.Select(x => {
            return new KeyValuePair<Dimensions, Vector3>(
                x,
                x.All.Aggregate((farthest, current) => (current.z < farthest.z) ? current : farthest)
            );
        }).ToArray();

        KeyValuePair<Dimensions, Vector3> leftmostDimension = leftmostPointPerDimension.Aggregate((most, current) => {
            return (current.Value.x < most.Value.x) ? current : most;
        });
        KeyValuePair<Dimensions, Vector3> rightmostDimension = rightmostPointPerDimension.Aggregate((most, current) => {
            return (current.Value.x > most.Value.x) ? current : most;
        });
        KeyValuePair<Dimensions, Vector3> topmostDimension = topmostPointPerDimension.Aggregate((most, current) => {
            return (current.Value.z > most.Value.z) ? current : most;
        });
        KeyValuePair<Dimensions, Vector3> bottommostDimension = bottommostPointPerDimension.Aggregate((most, current) => {
            return (current.Value.z < most.Value.z) ? current : most;
        });

        Vector3 newCenterWidth = (rightmostDimension.Value + leftmostDimension.Value) / 2f;
        Vector3 newCenterDepth = (topmostDimension.Value + bottommostDimension.Value) / 2f;
        newCenterWidth.z = newCenterDepth.z;
        newCenterDepth.x = newCenterWidth.x;

        Vector3 newCenter = (newCenterWidth + newCenterDepth) / 2f;

        Dimensions = new Dimensions() {
            Axes = dimensions[0].Axes,
            Center = newCenter,

            RawWidth = (rightmostDimension.Value.x - leftmostDimension.Value.x) / 2f,
            RawHeight = dimensions[0].RawHeight,
            RawDepth = (topmostDimension.Value.z - bottommostDimension.Value.z) / 2f,
        };
    }
}