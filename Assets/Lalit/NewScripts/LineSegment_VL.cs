using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment_VL : MonoBehaviour
{
    public List<Vector3> lineSegments;
    public Vector3 steelRulerPositon;
    public Vector3 GetPositionAtIndex(int index)
    {
        Vector3 pos = lineSegments[index];
        if (pos != null)
        {
            return pos;
        }
        else
        {
            Debug.LogError("index not found");
            return Vector3.zero;
        }
        
    }

}
