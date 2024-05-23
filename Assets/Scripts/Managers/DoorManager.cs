using UnityEngine;

public class DoorManager : Manager
{
    public static DoorManager Instance { get; private set; }

    public static PushDoor[] Doors { get; private set; }
    
    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        Doors = FindObjectsByType<PushDoor>(FindObjectsSortMode.None);
    } 
}