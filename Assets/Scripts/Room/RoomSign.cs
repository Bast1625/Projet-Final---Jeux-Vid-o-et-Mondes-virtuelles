using TMPro;
using UnityEngine;

public class RoomSign : MonoBehaviour
{
    [SerializeField] Room room;

    [SerializeField] TextMeshPro label;

    private void Awake()
    {
        label.text = room.Name;
    }
}