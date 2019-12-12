using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GVShape
{
    Sphere,
    Box,
    Cylinder,
}


public abstract class GVStar : MonoBehaviour
{
    public abstract Vector3 GetNearPositionOnSurface(Vector3 pos);
    public float GetLength(Vector3 pos)
    {
        var near = GetNearPositionOnSurface(pos);
        return (pos - near).magnitude;
    }
    public Vector3 GetGravityDirection(Vector3 pos)
    {
        var near = GetNearPositionOnSurface(pos);
        return -(pos - near).normalized;
    }
}

