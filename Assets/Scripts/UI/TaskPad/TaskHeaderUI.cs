using UnityEngine;
using TMPro;

public class TaskHeaderUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ongoingTasksCountLabel;
    [SerializeField] TextMeshProUGUI completedTasksCountLabel;
    [SerializeField] TextMeshProUGUI pageLabel;

    public void Initialize()
    {
        SetOngoingCount(0);
        SetCompletedCount(0);
        SetPageNumber(1, 1);
    }

    public void Refresh(TaskManager taskManager, TaskDocumentUI taskDocument)
    {
        SetOngoingCount(taskManager.OngoingCount);
        SetCompletedCount(taskManager.CompletedCount);
        SetPageNumber(taskDocument.CurrentPageNumber, taskDocument.TotalPageNumber);
    }

    public void SetOngoingCount(int ongoingCount)
    {
        ongoingTasksCountLabel.text = $"Ongoing \n {ongoingCount}";
    }
    public void SetCompletedCount(int completedCount)
    {
        completedTasksCountLabel.text = $"Completed \n {completedCount}";
    }
    public void SetPageNumber(int currentPageNumber, int pageCount)
    {
        pageLabel.text = $"Page ({currentPageNumber}/{pageCount})";
    }
}