using System;
using System.Linq;
using UnityEngine;
using DimensionUtility;
using Unity.VisualScripting;

public class CollisionChecker : MonoBehaviour
{
    public static CollisionChecker Instance { get; private set; }

    public void Initialize()
    {
        if (Instance == null)
            Instance = this;
        else
            throw new NotSupportedException("CollisionChecker must only be instantiated once.");
    }

    public bool PerformCheckOn(GameObject toCheck, Vector3 atPosition, GameObject toIgnore = null)
    {
        GameObject[] obstacles = Check(toCheck, atPosition);

        return obstacles.Where(obstacle => obstacle != toIgnore).Any();
    }
    public bool PerformCheckOn(GameObject toCheck, Vector3 atPosition, GameObject[] toIgnore)
    {
        GameObject[] obstacles = Check(toCheck, atPosition);

        return obstacles.Where(obstacle => !toIgnore.Contains(obstacle)).Any();
    }

    private GameObject[] Check(GameObject toCheck, Vector3 atPosition)
    {
        Dimensions toCheckDimensions = DimensionsScanner.Instance.PerformScanOn(toCheck);
        
        Vector3 toCheckScale = toCheckDimensions.ScaleAbs;
        Quaternion toCheckRotation = toCheck.transform.rotation;

        Collider[] obstacles = Physics.OverlapBox(atPosition, toCheckScale, toCheckRotation);

        return obstacles.Select(obstacle => obstacle.gameObject).ToArray();
    }

    public bool IsOverflowing(GameObject toCheck)
    {
        GameObject[] below = OverflowCheck(toCheck, toCheck.transform.position);

        return below.Any(gameObject => gameObject == null);
    }
    public bool IsOverflowing(GameObject toCheck, GameObject location)
    {
        return IsOverflowing(toCheck, toCheck.transform.position, location);
    }
    public bool IsOverflowing(GameObject toCheck, GameObject[] locations)
    {
        return IsOverflowing(toCheck, toCheck.transform.position, locations);
    }
    public bool IsOverflowing(GameObject toCheck, Vector3 atPosition, GameObject location)
    {
        GameObject[] below = OverflowCheck(toCheck, atPosition);

        return below.Any(gameObject => gameObject != location);
    }
    public bool IsOverflowing(GameObject toCheck, Vector3 atPosition, GameObject[] locations)
    {
        GameObject[] below = OverflowCheck(toCheck, atPosition);

        return below.Any(gameObject => !locations.Contains(gameObject));
    }

    private GameObject[] OverflowCheck(GameObject toCheck, Vector3 atPosition)
    {
        Dimensions toCheckDimensions = DimensionsScanner.Instance.PerformScanOn(toCheck);
        toCheckDimensions.Center = atPosition;

        Vector3[] corners = {
            toCheckDimensions.BottomLeftFront,
            toCheckDimensions.BottomRightFront,
            toCheckDimensions.BottomLeftBack,
            toCheckDimensions.BottomRightBack
        };

        GameObject[] below = new GameObject[corners.Length];

        for(int cornerID = 0; cornerID < below.Length; cornerID++)
        {
            Vector3 corner = corners[cornerID];
            Physics.Raycast(corner, Vector3.down, out RaycastHit hitInfo, 10f);

            below[cornerID] = hitInfo.collider == null ? null : hitInfo.collider.gameObject;
        }

        return below;
    }
}