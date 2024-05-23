using UnityEngine;

namespace DimensionUtility
{
    public struct Dimensions
    {
        public Axes Axes;

        public Vector3 Center;
        public Vector3 Orientation;

        public float RawWidth;
        public float RawHeight;
        public float RawDepth;

        public Vector3 Width { get => RawWidth * Axes.Side; }
        public Vector3 Height { get => RawHeight * Axes.Top; }
        public Vector3 Depth { get => RawDepth * Axes.Front; }
        public Vector3 Scale { get => Width + Height + Depth; }
        public Vector3 ScaleAbs { get => Utility.Abs(Scale); }

        public Vector3 LeftFace { get => -Width + Center; }
        public Vector3 RightFace { get => Width + Center; }
        public Vector3 TopFace { get => Height + Center; }
        public Vector3 BottomFace { get => -Height + Center; }
        public Vector3 FrontFace { get => Depth + Center; }
        public Vector3 BackFace { get => -Depth + Center; }

        public Vector3 TopLeftFront { get => Center - Width + Height + Depth; }
        public Vector3 TopRightFront { get => Center + Width + Height + Depth; }
        public Vector3 BottomLeftFront { get => Center - Width - Height + Depth; }
        public Vector3 BottomRightFront { get => Center + Width - Height + Depth; }

        public Vector3 TopLeftBack { get => Center - Width + Height - Depth; }
        public Vector3 TopRightBack { get => Center + Width + Height - Depth; }
        public Vector3 BottomLeftBack { get => Center - Width - Height - Depth; }
        public Vector3 BottomRightBack { get => Center + Width - Height - Depth; }

        public Vector3[] All { get => new Vector3[] {
            LeftFace, RightFace, TopFace, BottomFace, FrontFace, BackFace,
            TopLeftFront, TopRightFront, BottomLeftFront, BottomRightFront,
            TopLeftBack, TopRightBack, BottomLeftBack, BottomRightBack
        }; }

        public override string ToString()
        {
            return $"W:{Width}, H:{Height}, D:{Depth}";
        }
    }
}