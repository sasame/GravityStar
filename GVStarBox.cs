using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GVStarBox : GVStar
{
    public Vector3 Size = Vector3.one;

    public override Vector3 GetNearPositionOnSurface(Vector3 pos)
    {
        Vector3 localPos = Quaternion.Inverse(transform.rotation) * (pos - transform.position);

        for (int idDir = 0; idDir < 3; ++idDir)
        {
            int otherAxis1 = (idDir + 1) % 3;
            int otherAxis2 = (idDir + 2) % 3;
            if (Mathf.Abs(localPos[otherAxis1]) < Size[otherAxis1] * 0.5f)
            {
                if (Mathf.Abs(localPos[otherAxis2]) < Size[otherAxis2] * 0.5f)
                {
                    // 領域A
                    localPos[idDir] = ((localPos[idDir] > 0f) ? Size.x : -Size.x) * 0.5f;// Size[idDir] * 0.5f;
                    Vector3 posWorld = (transform.rotation * localPos) + transform.position;
                    Debug.DrawLine(pos, posWorld);
                    return posWorld;
                }
            }
        }
        // 領域B
        Vector3 nearEdge = Vector3.zero;
        nearEdge.x = (Mathf.Abs(localPos.x) < Size.x * 0.5f) ? localPos.x : ((localPos.x > 0f) ? Size.x : -Size.x) * 0.5f;
        nearEdge.y = (Mathf.Abs(localPos.y) < Size.y * 0.5f) ? localPos.y : ((localPos.y > 0f) ? Size.y : -Size.y) * 0.5f;
        nearEdge.z = (Mathf.Abs(localPos.z) < Size.z * 0.5f) ? localPos.z : ((localPos.z > 0f) ? Size.z : -Size.z) * 0.5f;
        nearEdge = (transform.rotation * nearEdge) + transform.position;
        return nearEdge;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Size);
    }
}
