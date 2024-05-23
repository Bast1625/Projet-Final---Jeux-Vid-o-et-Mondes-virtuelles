using DimensionUtility;
using System;
using UnityEngine;

public static class Utility
{
    public static float CalculateDotProductPosition(Transform first, Transform second)
    {
        Vector3 firstToForward = first.TransformDirection(Vector3.forward);
        Vector3 firstToSecond = Vector3.Normalize(second.transform.position - first.position);

        return Vector3.Dot(firstToForward, firstToSecond);
    }   
    public static float CalculateDotProductRotation(Transform first, Transform second)
    {
        Quaternion firstToForward = first.rotation;
        Quaternion firstToSecond = second.rotation;

        return Quaternion.Dot(firstToForward, firstToSecond);
    }

    public static T SelectAtRandom<T>(T[] array)
    {
        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        
        return array[randomIndex];
    }

    public static Vector3 Abs(Vector3 vector3)
    {
        return new Vector3(
            Math.Abs(vector3.x),
            Math.Abs(vector3.y),
            Math.Abs(vector3.z)
        );
    }
    public static bool AreEquals(Vector3 vectorA, Vector3 vectorB)
    {
        return Math.Abs((vectorA - vectorB).sqrMagnitude) < 0.01f;
    }

    public static Vector3 RandomVector3(Vector3 vectorA, Vector3 vectorB)
    {     
        float ratio = UnityEngine.Random.Range(0f, 1f);

        Vector3 difference = vectorB - vectorA;
        Vector3 test = ratio * difference;
        
        Vector3 randomVector = vectorA + test;
        
        return randomVector;
    }
    public static Vector3 RandomVectorIn(Dimensions dimensions)
    {
        Vector3 minWidth = -dimensions.Width;
        Vector3 maxWidth = dimensions.Width;
        Vector3 minHeight = dimensions.Height;
        Vector3 maxHeight = dimensions.Height;
        Vector3 minDepth = -dimensions.Depth;
        Vector3 maxDepth = dimensions.Depth;

        Vector3 randomX = RandomVector3(minWidth, maxWidth);
        Vector3 randomY = RandomVector3(minHeight, maxHeight);
        Vector3 randomZ = RandomVector3(minDepth, maxDepth);

        return randomX + randomY + randomZ + dimensions.Center;
    }

    public static Vector3 GetRandomPositionOn(GameObject location, GameObject toPlace)
    {
        Dimensions locationDimensions = DimensionsScanner.Instance.PerformScanOn(location);
        Dimensions toPlaceDimensions = DimensionsScanner.Instance.PerformScanOn(toPlace);

        Dimensions validDimensions = new Dimensions
        {
            Axes = locationDimensions.Axes,
            Center = locationDimensions.Center,

            RawWidth = locationDimensions.RawWidth - toPlaceDimensions.RawWidth,
            RawHeight = locationDimensions.RawHeight - toPlaceDimensions.RawHeight,
            RawDepth = locationDimensions.RawDepth + toPlaceDimensions.RawDepth,
        };

        Vector3 randomPosition;
        bool areThereObstacles;
        bool isOverflowing;

        int numberOfTries = 0;
        do
        {
            Vector3 randomWidth = RandomVector3(-validDimensions.Width, validDimensions.Width);
            Vector3 randomHeight = RandomVector3(-validDimensions.Height, validDimensions.Height);
            Vector3 randomDepth = validDimensions.Depth;

            randomPosition = randomWidth + randomHeight + randomDepth + validDimensions.Center;

            areThereObstacles = CollisionChecker.Instance.PerformCheckOn(toPlace, randomPosition, location);
            isOverflowing = CollisionChecker.Instance.IsOverflowing(toPlace, randomPosition, location);
            
            numberOfTries++;
            if(numberOfTries >= 10)
            {
                randomPosition = Vector3.positiveInfinity;
                
                break;
            }
        }
        while(areThereObstacles || isOverflowing);

        return randomPosition;
    }
    public static bool GetRandomPositionOn(GameObject location, GameObject toPlace, out Vector3 randomPosition)
    {
        randomPosition = GetRandomPositionOn(location, toPlace);

        return randomPosition != Vector3.positiveInfinity;
    }
}