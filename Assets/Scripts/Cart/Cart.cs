using System;
using UnityEngine;

public class Cart : Interactable
{
    public event Action OnDragEnter;
    public event Action OnDragExit;

    public void DragEnter() => OnDragEnter?.Invoke();
    public void DragExit() => OnDragExit?.Invoke();
}
