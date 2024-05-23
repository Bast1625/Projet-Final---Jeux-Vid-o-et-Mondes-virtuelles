using System;
using Unity.VisualScripting;
using UnityEngine;

public class TaskDocumentUI : PadDocument
{
    [SerializeField] TaskHeaderUI taskHeaderUI;
    [SerializeField] TaskListUI taskListUI;
    
    private TaskManager taskManager;
    
    public override void Initialize(GameManager gameManager)
    {
        taskManager = gameManager.TaskManager;

        TaskManager.OnTaskAssignation += (taskManager) => Refresh();
        TaskManager.OnTaskCompletion += (taskManager) => Refresh();
        TaskManager.OnTaskCancelation += (taskManager) => Refresh();

        taskHeaderUI.Initialize();
        taskListUI.Initialize();
    }

    public override int GetTotalPage() 
    {
        int totalPage = (int) Math.Ceiling(taskManager.OngoingCount / 7f);

        if(totalPage < 1)
            totalPage = 1;
        
        return totalPage;
    }
    public override void SetPage(int pageNumber)
    {
        CurrentPageNumber = pageNumber;
    }

    public override void Refresh()
    {
        taskHeaderUI.Refresh(taskManager, this);
        taskListUI.Refresh(taskManager, this);
    }
}
