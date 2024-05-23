using System;

public class Disposable : Holdable
{
    public event Action OnDispose;

    public void Dispose()
    {
        Destroy(gameObject);

        OnDispose?.Invoke();
    }
}