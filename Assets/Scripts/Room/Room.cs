using System.Linq;
using DimensionUtility;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string Name;
    
    [SerializeField] public Floor floor;

    public void Initialize()
    {
        floor.Initialize(this);   
    }

    public bool SelectPositionAtRandom(GameObject toPlace, out Vector3 randomPosition)
    {
        GameObject[] floorTiles = floor.Tiles;
        Dimensions floorDimensions = floor.Dimensions;

        Dimensions toPlaceDimensions = DimensionsScanner.Instance.PerformScanOn(toPlace);

        Vector3 tryRandomPosition;

        bool areThereObstacles;
        bool isOverflowing;

        int numberOfTries = 0;
        do
        {
            tryRandomPosition = Utility.RandomVectorIn(floorDimensions) + toPlaceDimensions.Height;

            areThereObstacles = CollisionChecker.Instance.PerformCheckOn(toPlace, tryRandomPosition, floorTiles);
            isOverflowing = CollisionChecker.Instance.IsOverflowing(toPlace, tryRandomPosition, floorTiles);

            numberOfTries++;

            if(numberOfTries == 100)
            {
                randomPosition = Vector3.zero;

                return false;
            }
        }
        while(areThereObstacles || isOverflowing);

        randomPosition = tryRandomPosition;

        return true;
    }

    public override string ToString()
    {
        return "Room " + Name;
    }
}