using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVStarCylinder : GVStar
{
    public float Radius = 1f;
    public float Length = 1f;

    public override Vector3 GetNearPositionOnSurface(Vector3 pos)
    {
        Vector3 dif = pos - transform.position;
        float dot = Vector3.Dot(dif, transform.up);
        Vector3 posNearOnAxis = (transform.position + transform.up * dot); // シリンダ軸上の近い位置
        Vector3 dirToPos = pos - posNearOnAxis; // 軸上の近い点からターゲット位置へのベクトル
        if (Mathf.Abs(dot) < Length * 0.5f)
        {
            // 領域A
            return posNearOnAxis + dirToPos.normalized * Radius;
        }
        float len = (dirToPos).magnitude;
        if (len < Radius)
        {
            Vector3 near = transform.position + dirToPos.normalized * len;
            // 領域B
            if (dot > 0f)
            {
                // 上
                return near + transform.up * Length * 0.5f;
            }
            else
            {
                // 下
                return near - transform.up * Length * 0.5f;
            }
        }
        // 領域C
        Vector3 posNearCyl = (transform.position + transform.up * ((dot > 0f) ? Length * 0.5f : -Length * 0.5f)) + (dirToPos.normalized) * Radius;
        return posNearCyl;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position-transform.up * (Length*0.5f), transform.position + transform.up * (Length * 0.5f));
    }
}
