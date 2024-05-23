using UnityEngine;

public class Drawing : Interactable
{
    private Whiteboard whiteboard;

    public float Opacity { get; private set; } = 1;
    public Texture Texture { get; set; }

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetSurface(Whiteboard surface)
    {
        whiteboard = surface;
    }

    public void SetMaterial(Material newMaterial)
    {
        meshRenderer.material = newMaterial;
    }
    public void SetOpacity(float newOpacity)
    {
        Opacity = newOpacity;
    }

    public void Erase()
    {
        Opacity -= 0.25f;

        Color newColor = meshRenderer.material.color;
        newColor.a = Opacity;

        meshRenderer.material.color = newColor;
    }

    protected override void InteractionStart(Player interactor)
    {
        Erase();
        
        if(Opacity <= 0.25f)
            whiteboard.Erase();
    }

    public override InteractionFeedback Feedback(Player interactor)
    {
        return new InteractionFeedback {
                Status = InteractionStatus.Valid,
                Message = $"Erase ({ Opacity * 100 }% visible)"
            };
    }  
}