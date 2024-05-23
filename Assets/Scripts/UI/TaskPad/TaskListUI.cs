using System.Collections.Generic;
using UnityEngine;

public class TaskListUI : PadDocumentPage
{
    [SerializeField] GameObject taskEntryPrefab;
    [SerializeField] GameObject noTaskLabel;

    private List<TaskEntryUI> taskEntries = new List<TaskEntryUI>();

    public void Initialize()
    {
        for(int i = 0; i < 7; i++)
        {
            TaskEntryUI newTaskEntry = Instantiate(taskEntryPrefab).GetComponent<TaskEntryUI>();

            newTaskEntry.transform.SetParent(transform, false);
            newTaskEntry.transform.localPosition = Vector3.zero;

            taskEntries.Add(newTaskEntry);
        }
    }

    public void Refresh(TaskManager taskManager, TaskDocumentUI taskPadUI)
    {
        Task[] ongoingTasks = taskManager.OngoingTasks;

        noTaskLabel.SetActive(ongoingTasks.Length == 0);

        taskEntries.ForEach(taskEntry => taskEntry.gameObject.SetActive(false));

        int firstTaskOnPageIndex = (taskPadUI.CurrentPageNumber - 1) * 7;
        
        int lastTaskOnPageIndex = firstTaskOnPageIndex + 7;
        if(lastTaskOnPageIndex > taskManager.OngoingCount)
            lastTaskOnPageIndex = taskManager.OngoingCount;
        
        int numberOfTasksToDisplay = lastTaskOnPageIndex - firstTaskOnPageIndex;        

        for(int taskEntryIndex = 0; taskEntryIndex < numberOfTasksToDisplay; taskEntryIndex++)
        {
            int currentTaskIndex = taskEntryIndex + firstTaskOnPageIndex;

            Task currentTask = ongoingTasks[currentTaskIndex];
            TaskEntryUI currentTaskEntry = taskEntries[taskEntryIndex];

            currentTaskEntry.SetTaskName(currentTask.Name);
            currentTaskEntry.SetTaskLocation(currentTask.Location);

            currentTaskEntry.transform.localPosition = Vector3.up * (-100 * taskEntryIndex + 300);

            currentTaskEntry.gameObject.SetActive(true);
        }
    }
}