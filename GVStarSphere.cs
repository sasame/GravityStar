using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVStarSphere : GVStar
{
    public float Radius = 1f;

    public override Vector3 GetNearPositionOnSurface(Vector3 pos)
    {
        Vector3 dif = pos - transform.position;
        return transform.position + (dif.normalized * Radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
