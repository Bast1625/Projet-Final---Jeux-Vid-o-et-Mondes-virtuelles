using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class DrawingManager : Manager
{
    public static DrawingManager Instance { get; private set; }
    
    [SerializeField] GameObject drawingPrefab;

    private Material baseMaterial;
    private Texture2D[] textures;

    public override void Initialize(GameManager gameManager)
    {
        if(Instance == null)
            Instance = this;
        else
            Debug.LogError($"{name} has already been instantiated.");

        baseMaterial = drawingPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        textures = Resources.LoadAll<Texture2D>("Drawings");
    }

    public Drawing GenerateDrawing()
    {
        Vector3 randomSize = SelectSizeAtRandom();
        string randomTextureName = Utility.SelectAtRandom(textures).name;

        return GenerateDrawing(randomSize, randomTextureName);
    }
    public Drawing GenerateDrawing(Vector3 size, string textureName)
    {
        Drawing newDrawing = Instantiate(drawingPrefab).GetComponent<Drawing>();

        newDrawing.transform.localScale = size;
        
        Texture2D texture = textures.ToList().Find(texture => texture.name == textureName);

        newDrawing.Texture = texture;
        Material material = new Material(baseMaterial);
        material.SetTexture(Shader.PropertyToID("_BaseMap"), texture); 

        newDrawing.SetMaterial(material);

        return newDrawing;
    }

    private Vector3 SelectSizeAtRandom()
    {
        float size = Random.Range(0.025f, 0.1f);

        return new Vector3(size, 0.01f, size);
    }
}