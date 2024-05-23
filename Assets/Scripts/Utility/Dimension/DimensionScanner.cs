using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DimensionUtility
{
    public class DimensionsScanner : MonoBehaviour
    {
        public static DimensionsScanner Instance { get; private set; }

        public float step;
        
        private DimensionLibrary dimensionLibrary = new DimensionLibrary();

        public void Initialize()
        {
            if(Instance == null)
                Instance = this;
            else
                throw new NotSupportedException($"{name} must only be instantiated once.");
        }

        public Dimensions PerformScanOn(GameObject toScan, bool doOverride = false)
        {
            if(dimensionLibrary.IsUpToDate(toScan) && !doOverride)
                return dimensionLibrary.Get(toScan);

            Axes axesToUse = Axes.GetAxes(toScan.transform);

            Collider toScanCollider = toScan.GetComponent<Collider>();
            Vector3 toScanPosition = toScanCollider.bounds.center;

            float width = Scan(toScanCollider, toScanPosition, axesToUse.Side);
            float height = Scan(toScanCollider, toScanPosition, axesToUse.Top);
            float depth = Scan(toScanCollider, toScanPosition, axesToUse.Front);
        
            Dimensions dimensions = new Dimensions {
                Axes = axesToUse,

                Center = toScanPosition,
                
                RawWidth = width,
                RawHeight = height,
                RawDepth = depth
            };

            DimensionInfo newDimensionInfo = new DimensionInfo
            {
                Dimensions = dimensions,

                PositionAtTimeOfScan = toScanPosition,
                RotationAtTimeOfScan = toScan.transform.rotation,
                ScaleAtTimeOfScan = toScan.transform.localScale
            };

            dimensionLibrary.Add(toScan, newDimensionInfo, true);

            return dimensions;
        }

        private float Scan(Collider toScan, Vector3 initialPosition, Vector3 direction)
        {
            transform.position = initialPosition;

            Collider[] collidersOverlappingWithScanner;

            do
            {
                transform.Translate(direction * step);

                collidersOverlappingWithScanner = Physics.OverlapBox(
                    transform.position, 
                    transform.localScale / 2f, 
                    toScan.transform.rotation
                );
            }
            while(collidersOverlappingWithScanner.Contains(toScan));

            Vector3 scannerSize = direction.normalized * transform.localScale.x / 2f;

            return (transform.position - initialPosition - scannerSize).magnitude;
        }
    }
}