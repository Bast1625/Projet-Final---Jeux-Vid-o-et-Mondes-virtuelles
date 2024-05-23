using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Manager, ISavable
{
    public static TaskManager Instance { get; private set; }

    public static Action<TaskManager> OnTaskAssignation;
    public static Action<TaskManager> OnTaskCancelation;
    public static Action<TaskManager> OnTaskCompletion;

    public static ITaskable[] Taskables;

    public string Id { get => "Task Manager"; }

    public Task[] AllTasks { get => tasks.Concat(endTasks).ToArray(); }
    private Task[] tasks;
    private Task[] endTasks;

    private List<Task> ongoingTasks = new List<Task>();
    public Task[] OngoingTasks { get => ongoingTasks.ToArray(); }
    
    public int OngoingCount { get => ongoingTasks.Count; }
    public int CompletedCount { get; private set; }
    
    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        Taskables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ITaskable>().ToArray();
        
        tasks = Resources.LoadAll<Task>("Tasks");

        endTasks = Resources.LoadAll<Task>("EndGameTasks");
    }

    public void Activate()
    {
        for(int i = 0; i < 8; i++)
            Invoke(nameof(TaskLoop), 1f);
            
        InvokeRepeating(nameof(TaskLoop), 8f, 8f);   
    }
    public void Deactivate()
    {
        CancelInvoke(nameof(TaskLoop));
    }

    public void TaskLoop()
    {
        Task newTask = GenerateTask();

        if(newTask == null)
            return;

        AssignTask(newTask);
    }

    public Task GenerateTask()
    {
        Task[] tasksThatCanExist = tasks.Where(task => task.CanExist(this)).ToArray();

        if( !tasksThatCanExist.Any() )
            return null;

        Task randomTask = Utility.SelectAtRandom(tasksThatCanExist);

        Task newTask = Instantiate(randomTask);

        newTask.Initialize(this);

        return newTask;
    }

    public void AssignTask(Task taskToAssign)
    {
        ongoingTasks.Add(taskToAssign);

        taskToAssign.Activate();

        Debug.Log(taskToAssign.AssignationMessage);
            
        OnTaskAssignation?.Invoke(this);
    }

    public void CancelTask(Task taskToCancel)
    {   
        ongoingTasks.Remove(taskToCancel);
        Destroy(taskToCancel.gameObject);

        Debug.Log(taskToCancel.CancelationMessage);

        OnTaskCancelation?.Invoke(this);
    }

    public void CompleteTask(Task taskToComplete)
    {
        ongoingTasks.Remove(taskToComplete);
        Destroy(taskToComplete.gameObject);
        
        CompletedCount++;
        
        Debug.Log(taskToComplete.CompletionMessage);

        OnTaskCompletion?.Invoke(this);
    }
    
    public void CompleteAllTasks()
    {
        foreach(Task ongoingTask in OngoingTasks)
            ongoingTask.Complete();
    }
    public void CancelAllTasks()
    {
        foreach(Task ongoingTask in OngoingTasks)
            ongoingTask.Cancel();
    }

    public void EndGame()
    {
        Task[] endTasksThatCanExist = endTasks.Where(task => task.CanExist(this)).ToArray();

        if( !endTasksThatCanExist.Any() )
            return;

        foreach(Task endTask in endTasksThatCanExist)
        {
            while(endTask.CanExist(this))
            {
                Task newTask = Instantiate(endTask);

                newTask.Initialize(this);
                
                AssignTask(newTask);
            }
        }
    }

    public void Save(GameData gameData)
    {
        TaskManagerData taskManagerData = new TaskManagerData
        {
            Id = Id,

            CompletedCount = CompletedCount,

            PickupTheTrashTasks = ongoingTasks
            .Where(task => task is PickupTheTrash)
            .Select(task => ((PickupTheTrash) task).Save()).ToArray(),

            DrawingOnTheBoardTasks = ongoingTasks
            .Where(task => task is DrawingOnTheBoard)
            .Select(task => ((DrawingOnTheBoard) task).Save()).ToArray(),

            LockTheDoorTasks = ongoingTasks
            .Where(task => task is LockTheDoor)
            .Select(task => ((LockTheDoor) task).Save()).ToArray(),

            ReplaceTheChairTasks = ongoingTasks.
            Where(task => task is ReplaceTheChair)
            .Select(task => ((ReplaceTheChair) task).Save()).ToArray(),

            OpenTheDoorTasks = ongoingTasks
            .Where(task => task is OpenTheDoor)
            .Select(task => ((OpenTheDoor) task).Save()).ToArray(),

            FixTheLightTasks = ongoingTasks
            .Where(task => task is FixTheLight)
            .Select(task => ((FixTheLight) task).Save()).ToArray()
        };

        gameData.TaskManagerData = taskManagerData;
    }
    public void Load(GameData gameData)
    {
        CancelAllTasks();
        
        TaskManagerData taskManagerData = gameData.TaskManagerData;

        CompletedCount = taskManagerData.CompletedCount;

        ongoingTasks.AddRange(taskManagerData.PickupTheTrashTasks.Select(taskData => {
            PickupTheTrash taskToLoad = (PickupTheTrash) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }).ToList());

        ongoingTasks.AddRange(taskManagerData.DrawingOnTheBoardTasks.Select(taskData => {
            DrawingOnTheBoard taskToLoad = (DrawingOnTheBoard) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }).ToList());

        ongoingTasks.AddRange(taskManagerData.LockTheDoorTasks.Select(taskData => {
            LockTheDoor taskToLoad = (LockTheDoor) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }));

        ongoingTasks.AddRange(taskManagerData.ReplaceTheChairTasks.Select(taskData => {
            ReplaceTheChair taskToLoad = (ReplaceTheChair) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }));

        ongoingTasks.AddRange(taskManagerData.OpenTheDoorTasks.Select(taskData => {
            OpenTheDoor taskToLoad = (OpenTheDoor) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }));

        ongoingTasks.AddRange(taskManagerData.FixTheLightTasks.Select(taskData => {
            FixTheLight taskToLoad = (FixTheLight) Instantiate(AllTasks.ToList().Find(task => task.Id == taskData.Id));

            taskToLoad.Load(this, taskData);

            return (Task) taskToLoad;
        }));
    }

    #region Helper Methods
    public TTaskable FindTaskable<TTaskable>(string taskableID) where TTaskable : ITaskable
    {
        return Taskables.OfType<TTaskable>().ToList().Find(taskable => taskable.Id == taskableID);
    }
    public TTaskable[] FindAllTaskables<TTaskable>() where TTaskable : ITaskable
    {
        return Taskables.OfType<TTaskable>().ToArray();
    }
    public TTaskable[] FindAvailableTaskables<TTaskable>() where TTaskable : ITaskable
    {
        TTaskable[] allTTaskables = FindAllTaskables<TTaskable>();

        return allTTaskables
            .Where(taskable => !ongoingTasks.Any(ongoingTask => ongoingTask.GetTaskable() == (ITaskable) taskable))
            .ToArray();
    }
    #endregion
}
