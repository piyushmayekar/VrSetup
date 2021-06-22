using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// v1 of wire made by Piyush M.
/// </summary>
public class WireV1 : MonoBehaviour
{
    [SerializeField] GameObject segmentPrefab;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform start, end;
    [SerializeField] int segmentCount = 10;
    [SerializeField] List<GameObject> points;
    IEnumerator Start()
    {
        int count = 0;
        float lerpFac = 1f / segmentCount;
        float t = 0f;
        _lineRenderer.positionCount = segmentCount + 2;
        _lineRenderer.SetPosition(0, start.position);
        while (count < segmentCount)
        {
            Vector3 pos = Vector3.Lerp(start.position, end.position, t);
            GameObject point = Instantiate(segmentPrefab, pos, Quaternion.identity, transform);
            if (points.Count > 0)
                point.GetComponent<SpringJoint>().connectedBody = points[points.Count - 1].GetComponent<Rigidbody>();
            else
                point.GetComponent<SpringJoint>().connectedBody = start.GetComponent<Rigidbody>();
            points.Add(point);
            count++;
            t += lerpFac;
            _lineRenderer.SetPosition(count + 1, pos);
            yield return new WaitForEndOfFrame();
        }
        // start.GetComponent<SpringJoint>().connectedBody = points[0].GetComponent<Rigidbody>();
        end.GetComponent<FixedJoint>().connectedBody = points[points.Count - 1].GetComponent<Rigidbody>();
        StartCoroutine(PlaceLineRendererPoints());
    }

    IEnumerator PlaceLineRendererPoints()
    {
        while (gameObject.activeSelf)
        {
            for (int i = 0; i < segmentCount; i++)
                _lineRenderer.SetPosition(i + 1, points[i].transform.position);
            _lineRenderer.SetPosition(0, start.position);
            _lineRenderer.SetPosition(segmentCount + 1, end.position);
            yield return new WaitForEndOfFrame();
        }
    }


}
