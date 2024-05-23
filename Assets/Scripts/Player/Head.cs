using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Head : PlayerComponent
{
    [SerializeField] public Camera Camera;

    [SerializeField] private Image cursor;
    [SerializeField] private TextMeshProUGUI selecting;

    public Selection Selection { get; private set; }

    private StarterAssetsInputs assetsInputs; 
    
    private void Awake()
    {
        assetsInputs = player.GetComponent<StarterAssetsInputs>();
    }

    public void Check(Player player)
    {
        if(player.IsInteracting)
            return;
        
        if(player.IsDriving)
        {
            Unselect();
            
            return;
        }

        if(player.Hand.Pad.IsOut)
        {
            Unselect();
            
            return;
        }

        Physics.Raycast(transform.position, transform.forward, out RaycastHit raycast, 10f);
        if(raycast.collider == null)
        {
            Unselect();

            return;
        }

        raycast.collider.TryGetComponent(out IInteractable interactable);
        if(interactable == null)
        {
            Unselect();

            return;
        }

        Select(interactable);
    }

    private void Select(IInteractable interactable)
    {
        InteractionFeedback feedback = interactable.Feedback(player);

        Selection = new Selection {
            Interactable = interactable,
            Feedback = feedback
        };
        
        UpdateCursor();
    }

    private void Unselect()
    {
        Selection = Selection.None;

        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if(!Selection.IsValid)
        {
            selecting.text = "Nothing to interact with...";
            
            cursor.color = Color.white;
        }
        else
        if(Selection.Feedback.Status == InteractionStatus.Valid)
        {
            selecting.text = Selection.Feedback.Message;

            cursor.color = Color.green;
        }
        else
        if(Selection.Feedback.Status == InteractionStatus.Invalid)
        {
            selecting.text = Selection.Feedback.Message;

            cursor.color = Color.red;
        }
    }

    public Vector2 GetMouseInputs() => assetsInputs.look;

    public RaycastHit GetMousePosition() 
    {
        LayerMask floorLayer = LayerMask.GetMask("Floor");
        Ray fromMouseToTerrain = Camera.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(fromMouseToTerrain, out RaycastHit raycastHit, 10f, floorLayer);

        return raycastHit;
    }
}
