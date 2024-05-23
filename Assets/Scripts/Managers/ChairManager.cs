using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChairManager : Manager
{
    public static ChairManager Instance { get; private set; }

    public static Chair[] Chairs { get; private set; }
    
    private static Vector3[] InitialPositions;
    private static Quaternion[] InitialRotations;

    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        Chairs = FindObjectsByType<Chair>(FindObjectsSortMode.None);

        InitialPositions = Chairs.Select(chair => chair.transform.position).ToArray();
        InitialRotations = Chairs.Select(chair => chair.transform.rotation).ToArray();
    } 

    public static Vector3 GetInitialPosition(Chair chair)
    {
        int chairIndex = Array.IndexOf(Chairs, chair);

        return InitialPositions[chairIndex];
    }

    public static Quaternion GetInitialRotation(Chair chair)
    {
        int chairIndex = Array.IndexOf(Chairs, chair);

        return InitialRotations[chairIndex];
    }
}