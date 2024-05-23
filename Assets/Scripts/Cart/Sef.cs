using UnityEngine;

public class Sef : MonoBehaviour
{
    public bool IsPlayerBehind { get; private set; }

    private void OnTriggerEnter()
    {
        IsPlayerBehind = true;
    }

    private void OnTriggerExit()
    {
        IsPlayerBehind = false;
    }
}