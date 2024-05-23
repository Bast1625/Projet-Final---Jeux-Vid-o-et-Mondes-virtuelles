using UnityEngine;

public abstract class Task : MonoBehaviour, IIdentifiable
{
    public string Id { get => GetType().Name; }
    public string Name;
    public string Location { get; protected set; }
    
    [TextArea] public string AssignationMessage = "";
    [TextArea] public string CompletionMessage = "";
    public string CancelationMessage { get => $"{Name} in {Location} has been canceled."; }

    protected TaskManager taskManager;

    public abstract bool CanExist(TaskManager taskManager);

    public virtual void Initialize(TaskManager taskManager) => this.taskManager = taskManager;

    public abstract void Activate();
    public abstract void Cancel();
    public abstract void Complete();

    public abstract ITaskable GetTaskable();
}