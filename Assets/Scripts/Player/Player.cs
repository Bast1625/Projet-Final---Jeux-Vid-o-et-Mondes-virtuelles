using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IInteractor, ISavable
{
    public string Id { get => $"{GetType()}_{name}"; }

    public Head Head;
    public Hand Hand;

    public bool IsInteracting { get; private set; }
    public bool IsDriving { get; private set; }

    [SerializeField] GameObject playerController;
    [SerializeField] GameObject cartController;

    private CharacterController characterController;
    private PlayerInput playerInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        Head.Initialize(this);
        Hand.Initialize(this);
    }

    public void Update()
    {
        Head.Check(this);
    }

    public void Interact(InputAction interactionInput)
    {
        IsInteracting = false;

        if(Head.Selection.IsInvalid)    
            return;

        if(interactionInput.phase == InputActionPhase.Performed)
            IsInteracting = true;
      
        Head.Selection.Interactable.Interact(this, interactionInput.phase);
    }

    public void Drop(InputAction interactionInput)
    {
        if(Hand.IsFull)
            Hand.Drop();
    }

    public void Pad(InputAction interactionInput)
    {
        if(Hand.Holdable != null)
            return;
            
        if(Hand.Pad.IsOut)
            Hand.Pad.Hide();
        else
            Hand.Pad.Show();
    }

    public Transform GetTransform() => Head.transform;
    public Vector3 GetInputs() => Head.GetMouseInputs();

    public void SetPositionTo(Vector3 newPosition)
    {
        characterController.enabled = false;

        transform.position = newPosition;

        characterController.enabled = true;
    }
    public void SetRotationTo(Quaternion newRotation)
    {
        characterController.enabled = false;

        transform.rotation = newRotation;

        characterController.enabled = true;
    }

    public void SwitchToCartState()
    {
        characterController.enabled = false;
        
        playerController.SetActive(false);
        cartController.SetActive(true);

        playerInput.SwitchCurrentActionMap("Cart");

        characterController.enabled = true;

        IsDriving = true;
    }

    public void SwitchToPlayerState()
    {
        characterController.enabled = false;
        
        cartController.SetActive(false);
        playerController.SetActive(true);
        
        playerInput.SwitchCurrentActionMap("Player");
        
        characterController.enabled = true;

        IsDriving = false;
    }

    public void Save(GameData gameData)
    {
        PlayerData data = new PlayerData { 
            Position = transform.position,
            Rotation = transform.rotation,

            HeadPosition = Head.transform.localPosition,
            HeadRotation = Head.transform.localRotation,

            CameraPosition = Head.Camera.transform.position,
            CameraRotation = Head.Camera.transform.rotation,

            IsDriving = IsDriving
        };

        gameData.PlayerData = data;
    }

    public void Load(GameData gameData)
    {
        PlayerData data = gameData.PlayerData;

        characterController.enabled = false;

        transform.position = data.Position;
        transform.rotation = data.Rotation;

        Head.transform.localPosition = data.HeadPosition;
        Head.transform.localRotation = data.HeadRotation;

        Head.Camera.transform.position = data.CameraPosition;
        Head.Camera.transform.rotation = data.CameraRotation;

        IsDriving = data.IsDriving;
        
        if(IsDriving)
            SwitchToCartState();
        else
            SwitchToPlayerState();

        characterController.enabled = true;
    }
}
