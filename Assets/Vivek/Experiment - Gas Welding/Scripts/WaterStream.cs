using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaterStream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private ParticleSystem splashParticle = null;
    private Coroutine pourcorutine;
    public string hitcollidername="";
    private Vector3 targetpos = Vector3.zero;
    public UnityEvent NextStep;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren < ParticleSystem>();

    }
    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);

    }
    public void Begin()
    {
        StartCoroutine(UpdateParticle());
        pourcorutine = StartCoroutine(BeginPour());

    }
    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetpos = FindEndPoint();
            MoveToPosition(0, transform.position);
            AnimatePos(1, targetpos);
        yield return null;

        }
            }
    public RaycastHit hit;
    private Vector3 FindEndPoint()
    {
        
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit,10f);
       if(hitcollidername!=null)
        {
            if (hit.collider != null)
            {
                if (hit.collider.name == hitcollidername)
                {
                    NextStep.Invoke();
                }
            }
        }
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);

        return endPoint;

    }
    public void End()
    {
        StopCoroutine(pourcorutine);
        pourcorutine = StartCoroutine(EndPour());
    }
    private IEnumerator EndPour()
    {
        while(!HasReachedPosition(0,targetpos))
        {
            AnimatePos(0, targetpos);
            AnimatePos(1, targetpos);
            yield return null;
        }
        Destroy(gameObject);

    }

    private void MoveToPosition(int index,Vector3 targetposition)
    {
        lineRenderer.SetPosition(index, targetposition);
    }
    private void AnimatePos(int index,Vector3 targetPos)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPos, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }
    private bool HasReachedPosition(int index,Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);

        return currentPosition==targetPosition;
    }
    private IEnumerator UpdateParticle()
    {
        while (gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetpos;
            bool isHitting = HasReachedPosition(1, targetpos);
            splashParticle.gameObject.SetActive(isHitting);
            yield return null;
        }
    }
}
