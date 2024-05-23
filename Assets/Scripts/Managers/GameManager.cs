using System;
using DimensionUtility;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnGameStart;
    public static event Action OnGameWin;
    public static event Action OnGameOver;
    public static event Action OnGameEnd;

    public static event Action OnEndGameStart;
    public static event Action OnEndGameEnd;

    [SerializeField] DimensionsScanner dimensionsScanner;
    [SerializeField] CollisionChecker collisionChecker;

    public UIManager UIManager;
    public TaskManager TaskManager;

    public RoomManager RoomManager;

    public DoorManager DoorManager;
    public ChairManager ChairManager;
    public DrawingManager DrawingManager;

    public Player Player;

    [SerializeField] int howManyCompletedToWin;
    [SerializeField] int howManyOngoingToLose;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        dimensionsScanner.Initialize();
        collisionChecker.Initialize();

        UIManager.Initialize(this);
        TaskManager.Initialize(this);

        RoomManager.Initialize(this);

        DoorManager.Initialize(this);
        ChairManager.Initialize(this);
        DrawingManager.Initialize(this);

        RoomManager.OnRoomExit += GameStartCondition;
    }

    public void GameStart()
    {
        RoomManager.OnRoomExit -= GameStartCondition;

        TaskManager.OnTaskCompletion += GameWinCondition;
        TaskManager.OnTaskAssignation += GameOverCondition;

        TaskManager.Activate();

        OnGameStart?.Invoke();
    }
    public void GameWin()
    {
        TaskManager.OnTaskCompletion -= GameWinCondition;
        TaskManager.OnTaskAssignation -= GameOverCondition;

        TaskManager.Deactivate();
        TaskManager.CancelAllTasks();

        Debug.Log("You are good at your job. Don't forget to close everything off before leaving.");

        EndGameStart();

        OnGameWin?.Invoke();
    }
    public void GameOver()
    {
        TaskManager.OnTaskCompletion -= GameWinCondition;
        TaskManager.OnTaskAssignation -= GameOverCondition;

        TaskManager.Deactivate();

        Debug.Log("You have proven to be inefficient at your job. Get out of here.");

        OnGameOver?.Invoke();
    }

    public void EndGameStart()
    {
        TaskManager.EndGame();
        
        TaskManager.OnTaskCompletion += EndGameEndCondition;
        
        Debug.Log("You completed every tasks. Turn off the lights and lock the doors.");

        OnEndGameStart?.Invoke();
    }
    public void EndGameEnd()
    {   
        RoomManager.OnRoomEnter += GameEndCondition;
        
        Debug.Log("Thank you for your work. You are allowed to leave.");

        OnEndGameEnd?.Invoke();

        if(RoomManager.GetRoomOf(Player.gameObject).Name == "Main Corridor")
            GameEnd();
    }

    public void GameEnd()
    {
        RoomManager.OnRoomEnter -= GameEndCondition;

        Debug.Log("You take a good night rest, waiting patiently for your next shift...");

        OnGameEnd?.Invoke();
    }

    private void GameStartCondition(Room room)
    {
        if(room.Name == "Lobby")
            GameStart();
    }

    private void GameWinCondition(TaskManager taskManager)
    {
        if(taskManager.CompletedCount >= howManyCompletedToWin)
                GameWin();
    }
    private void GameOverCondition(TaskManager taskManager)
    {
        if(taskManager.OngoingCount >= howManyOngoingToLose)
                GameOver();
    }

    private void EndGameEndCondition(TaskManager taskManager)
    {
        if(taskManager.OngoingCount == 0)
            EndGameEnd();
    }   

    private void GameEndCondition(Room room)
    {
        if(room.Name == "Main Corridor")
            GameEnd();
    }
}
