using System;
using System.Linq;
using UnityEngine;

public class EmptyTheTrashcan : Task
{
    private Trashcan trashcan;

    public override bool CanExist(TaskManager taskManager)
    {
        //Trashcan[] trashcans = taskManager.FindAvailableTaskables<Trashcan>();

        //return trashcans.Any();

        return false;
    }
    public override void Initialize(TaskManager taskManager)
    {
        base.Initialize(taskManager);
        //Trashcan[] trashcans = taskManager.FindAvailableTaskables<Trashcan>();

        //trashcan = trashcans[UnityEngine.Random.Range(0, trashcans.Length)];
    }

    public override void Activate()
    {
        //trashcan.OnEmpty += Complete;

        //trashcan.Fill();

        //Name = "Empty the trashcan";
        //Location = "Laboratory G252";

        //Debug.Log("The trash is full!");
    }

    public override void Complete()
    {
        //Debug.Log("The trashcan has been emptied!");

        //trashcan.OnEmpty -= Complete;

        //OnCompletion?.Invoke(this);
    }

    public override void Cancel()
    {
        //trashcan.OnEmpty -= Complete;
    }

    public override ITaskable GetTaskable() => trashcan;
}