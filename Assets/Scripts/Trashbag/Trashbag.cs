using System;

public class Trashbag : Disposable, ITaskable
{
    public string Id { get => $"{GetType()}_{name}"; } 
}