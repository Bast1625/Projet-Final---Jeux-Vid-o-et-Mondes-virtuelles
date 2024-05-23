using UnityEngine;
using UnityEngine.InputSystem;

public class CartInputs : MonoBehaviour
{
    [Header("Character Input Values")]
	public Vector2 move;
    public Vector2 turn;
	public Vector2 look;
	public bool sprint;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;

    private Player player;
	private void Awake()
	{
		player = GetComponent<Player>();
	}

    public void OnCartMove(InputValue value)
	{
		MoveInput(value.Get<Vector2>());
	}
    public void OnCartTurn(InputValue value)
    {
        TurnInput(value.Get<Vector2>());
    }
    public void OnCartLook(InputValue value)
	{
		if(cursorInputForLook)
		{
			LookInput(value.Get<Vector2>());
		}
	}
    public void OnCartSprint(InputValue value)
	{
		SprintInput(value.isPressed);
	}
	public void OnCartExit(InputValue value)
    {
        player.SwitchToPlayerState();
    }

    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }
    public void TurnInput(Vector2 newTurnDirection)
    {
        turn = newTurnDirection;
    }
    public void LookInput(Vector2 newLookDirection)
	{
		look = newLookDirection;
	}
    public void SprintInput(bool newSprintState)
	{
		sprint = newSprintState;
	}

    private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState)
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}