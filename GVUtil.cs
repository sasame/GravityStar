using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GVUtil
{
    public static GVStar GetNearStar(Vector3 pos)
    {
        GVStar[] stars = GameObject.FindObjectsOfType<GVStar>();
        float nearStarDistance = float.MaxValue;
        GVStar nearStar = null;
        foreach (var s in stars)
        {
            Vector3 dif = s.GetNearPositionOnSurface(pos) - pos;
            float len = dif.magnitude;
            if (len < nearStarDistance)
            {
                nearStarDistance = len;
                nearStar = s;
            }
        }
        return nearStar;
    }
    public static Vector3 GetGravityDirection(Vector3 pos)
    {
        GVStar nearStar = GetNearStar(pos);
        if (nearStar != null)
        {
            return nearStar.GetGravityDirection(pos);
        }
        return Vector3.down;
    }
}
