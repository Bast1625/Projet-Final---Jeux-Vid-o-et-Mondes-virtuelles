using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Manager
{
    public static UIManager Instance { get; private set; }

    [SerializeField] Pad pad;

    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        pad.Initialize(gameManager);
        
        pad.Show();
    }
}