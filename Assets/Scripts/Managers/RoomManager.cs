using System;
using System.Linq;
using UnityEngine;

public class RoomManager : Manager
{
    public static event Action<Room> OnRoomEnter;
    public static event Action<Room> OnRoomExit;

    public static RoomManager Instance { get; private set; }

    public static Room[] Rooms { get; private set; } = Array.Empty<Room>();

    private static LayerMask floorLayer;

    private Player player;

    private Room playerRoom;
    private Room previousPlayerRoom;

    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        player = gameManager.Player;

        floorLayer = LayerMask.GetMask("Floor");

        Rooms = FindObjectsByType<Room>(FindObjectsSortMode.None);
        foreach(Room room in Rooms)
            room.Initialize();
    }

    private void Update()
    {
        playerRoom = GetRoomOf(player.gameObject);
        
        if(previousPlayerRoom != playerRoom)
        {
            if(previousPlayerRoom != null)
                OnRoomExit?.Invoke(previousPlayerRoom);
            
            if(playerRoom != null)
                OnRoomEnter?.Invoke(playerRoom);
        }

        previousPlayerRoom = playerRoom;
    }

    public static Room SelectAtRandom() => Utility.SelectAtRandom(Rooms);
    public static Room GetRoomOf(GameObject toGetRoomOf)
    {
        Physics.Raycast(toGetRoomOf.transform.position, Vector3.down, out RaycastHit hitInfo, 5f, floorLayer);

        Room room = null;

        if(hitInfo.collider != null)
        {
            Floor floorBelow = hitInfo.collider.GetComponentInParent<Floor>();

            room = floorBelow.Room;
        }

        return room;
    }

    public static bool IsInRoom(GameObject gameObject) => GetRoomOf(gameObject) != null;
}