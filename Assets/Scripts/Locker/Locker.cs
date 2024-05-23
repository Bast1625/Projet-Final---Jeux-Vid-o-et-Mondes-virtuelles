using UnityEngine;

public class Locker : MonoBehaviour
{
    public string lockerNumber;

    [SerializeField] LockerDoor door;
    [SerializeField] Storage upperStorage;
    [SerializeField] Storage lowerStorage;

    private void Awake()
    {
        door.name = lockerNumber;
    }
}