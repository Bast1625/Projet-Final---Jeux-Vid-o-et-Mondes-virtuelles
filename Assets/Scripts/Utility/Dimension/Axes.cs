using UnityEngine;

namespace DimensionUtility
{
    public class Axes
    {
        public Vector3 Top;
        public Vector3 Side;
        public Vector3 Front;

        public static Vector3 FromAxisToVector3(Transform transform, Axis axis)
        {
            if(axis == Axis.Red)
                return transform.right;
            if(axis == Axis.Green)
                return transform.up;
            if(axis == Axis.Blue)
                return transform.forward;

            if (axis == Axis.MinusRed)
                return -transform.right;
            if (axis == Axis.MinusGreen)
                return -transform.up;
            if (axis == Axis.MinusBlue)
                return -transform.forward;

            return Vector3.zero;
        }

        public static Axes GetAxes(Transform transform)
        {
            transform.TryGetComponent(out AxesConfiguration newAxes);

            if (newAxes == null)
                return new Axes
                {
                    Top = transform.up,
                    Side = transform.right,
                    Front = transform.forward
                };
            else
                return new Axes
                {
                    Top = Axes.FromAxisToVector3(transform, newAxes.TopAxis),
                    Side = Axes.FromAxisToVector3(transform, newAxes.SideAxis),
                    Front = Axes.FromAxisToVector3(transform, newAxes.FrontAxis)
                };
        }
    }
}