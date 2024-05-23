using UnityEngine;

public class Bore : MonoBehaviour
{
    [SerializeField] PushDoor door;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        Invoke(nameof(t), 0f);
        
    }

    private void t()
    {
        door.OnPlayerHold += () => animator.SetTrigger("Hold");
        door.OnPlayerRelease += () => animator.SetTrigger("Release");

        door.OnTryOpen += () => animator.SetTrigger("Rattle");
    }
}