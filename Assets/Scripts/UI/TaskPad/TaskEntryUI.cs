using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TaskEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskNameLabel;
    [SerializeField] private TextMeshProUGUI taskLocationLabel;
    [SerializeField] private Image checkbox;

    public string Name;
    public string Location;

    public void SetTaskName(string taskName)
    {
        Name = taskName;
        taskNameLabel.text = taskName;
    }
    public void SetTaskLocation(string taskLocation)
    {
        Location = taskLocation;
        taskLocationLabel.text = $"--- {taskLocation} ---";
    }
}
