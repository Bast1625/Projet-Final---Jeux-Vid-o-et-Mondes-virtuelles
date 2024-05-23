using System;
using System.Collections.Generic;
using UnityEngine;

namespace DimensionUtility
{
    public class DimensionLibrary
    {
        private Dictionary<GameObject, DimensionInfo> entries = new Dictionary<GameObject, DimensionInfo>();

        public bool IsRegistered(GameObject gameObject) => entries.ContainsKey(gameObject);
        public bool IsUpToDate(GameObject gameObject)
        {
            if(!IsRegistered(gameObject))
                return false;
    
            DimensionInfo toScanInfo = entries[gameObject];

            bool arePositionsTheSame = Utility.AreEquals(gameObject.transform.position, toScanInfo.PositionAtTimeOfScan);
            bool areRotationsTheSame = gameObject.transform.rotation == toScanInfo.RotationAtTimeOfScan;
            bool areScalesTheSame = gameObject.transform.localScale == toScanInfo.ScaleAtTimeOfScan;

            return arePositionsTheSame && areRotationsTheSame && areScalesTheSame;
        }

        public Dimensions Get(GameObject gameObject) => entries[gameObject].Dimensions;
        public void Add(GameObject gameObject, DimensionInfo dimensionsInfo, bool doOverwrite = false) 
        {
            if(doOverwrite)
                Remove(gameObject);
    
            entries.Add(gameObject, dimensionsInfo);
        } 

        public void Remove(GameObject gameObject) => entries.Remove(gameObject);
    }
}