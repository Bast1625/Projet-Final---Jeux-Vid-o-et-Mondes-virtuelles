using System;
using UnityEngine;
using DimensionUtility;

public class Whiteboard : MonoBehaviour, ITaskable
{
    public event Action OnDraw;
    public event Action OnErase;

    public string Id { get => $"{GetType()}_{name}"; }

    public bool IsClean { get => Drawing == null; }
    public bool IsDirty { get => !IsClean; }

    public Drawing Drawing { get; private set; }

    public void SetDrawing(Drawing newDrawing)
    {
        Drawing = newDrawing;

        Drawing.SetSurface(this);
    }

    public void Draw()
    {
        Drawing = DrawingManager.Instance.GenerateDrawing();

        Drawing.SetSurface(this);
        
        Dimensions whiteboardDimensions = DimensionsScanner.Instance.PerformScanOn(gameObject);
        Dimensions drawingDimensions = DimensionsScanner.Instance.PerformScanOn(Drawing.gameObject);

        Quaternion randomRotation = Quaternion.Euler(transform.eulerAngles + (drawingDimensions.Axes.Side * 90f));
        Drawing.transform.rotation = randomRotation;
        
        Physics.SyncTransforms();
       
        Vector3 randomPosition = SelectPositionAtRandom(gameObject, Drawing.gameObject);
        Drawing.transform.position = randomPosition;

        OnDraw?.Invoke();
    }

    public void Erase()
    {
        Destroy(Drawing.gameObject);

        Drawing = null;

        OnErase?.Invoke();
    }

    private Vector3 SelectPositionAtRandom(GameObject location, GameObject toPlace)
    {
        Dimensions locationDimensions = DimensionsScanner.Instance.PerformScanOn(location);
        Dimensions toPlaceDimensions = DimensionsScanner.Instance.PerformScanOn(toPlace.gameObject);

        Dimensions validDimensions = new Dimensions
        {
            Axes = locationDimensions.Axes,
            Center = locationDimensions.Center,

            RawWidth = locationDimensions.RawWidth - toPlaceDimensions.RawWidth,
            RawHeight = locationDimensions.RawHeight - toPlaceDimensions.RawHeight,
            RawDepth = locationDimensions.RawDepth,
        };

        Vector3 randomWidth = Utility.RandomVector3(-validDimensions.Width, validDimensions.Width);
        Vector3 randomHeight = Utility.RandomVector3(-validDimensions.Height, validDimensions.Height);
        Vector3 randomDepth = validDimensions.Depth;

        Vector3 randomPosition = randomWidth + randomHeight + randomDepth + validDimensions.Center;

        return randomPosition;
    }
}