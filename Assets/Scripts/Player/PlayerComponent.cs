using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    protected Player player { get; private set; }
    public void Initialize(Player player) => this.player = player;
}