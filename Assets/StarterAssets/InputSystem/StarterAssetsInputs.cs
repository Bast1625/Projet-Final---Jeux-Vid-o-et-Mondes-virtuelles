using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
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

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void OnDrop(InputValue value)
		{
			DropInput(value.isPressed);
		}

		public void OnSave(InputValue value)
		{
			SaveInput();
		}

		public void OnLoad(InputValue value)
		{
			LoadInput();
		}

		public void OnPad(InputValue value)
		{
			PadInput();
		}
		public void OnPageNext(InputValue value)
		{
			PageNextInput();
		}
		public void OnPagePrevious(InputValue value)
		{
			PagePreviousInput();
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		public void InteractInput(bool newInteractState)
		{
            InputAction interactAction = InputSystem.ListEnabledActions().Find(action => action.name == "Interact");

            player.Interact(interactAction);
		}

		public void DropInput(bool newDropState)
		{
			InputAction dropAction = InputSystem.ListEnabledActions().Find(action => action.name == "Drop");

			player.Drop(dropAction);
		}

		public void PadInput()
		{
			InputAction padAction = InputSystem.ListEnabledActions().Find(action => action.name == "Pad");

			player.Pad(padAction);
		}
		public void PageNextInput()
		{
			player.Hand.Pad.NextPage();
		}
		public void PagePreviousInput()
		{	
			player.Hand.Pad.PreviousPage();
		}

		public void SaveInput()
		{
			SaveManager.Save();
		}

		public void LoadInput()
		{
			SaveManager.Load();
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
	
}