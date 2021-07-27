using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CopperSulphate_VL : MonoBehaviour
{
    private Transform pourPoint;
    public float maxDistance;
    public float pourDistance;
    public GameObject markingPlate;
    public Material mat;
    public float speed;
    LineRenderer dropLine;
    public bool donePouring;

    private UnityEvent CallMethodOnPouringDone = new UnityEvent();
    private void Start()
    {
        pourPoint = transform.GetChild(0);
        mat = markingPlate.GetComponent<MeshRenderer>().material;
        dropLine = transform.GetChild(0).GetComponent<LineRenderer>();
    }

    public void AssignMethodOnPoringDone(UnityAction method)
    {
        if (CallMethodOnPouringDone != null)
        {
            CallMethodOnPouringDone.RemoveAllListeners();
        }

        CallMethodOnPouringDone.AddListener(method);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "CSBottle")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        

        if (donePouring)
        {
            dropLine.SetPosition(1, new Vector3(0, 0, 0));
            return;
        }
        else
        {
            RaycastHit hit;

            if (Physics.Raycast(pourPoint.position, pourPoint.up, out hit, maxDistance))
            {
                if (hit.collider.tag == "Job")
                {
                    float distance = Vector3.Distance(hit.point, pourPoint.position);
                    
                    if (distance <= pourDistance)
                    {
                        Debug.DrawRay(pourPoint.position, pourPoint.up, Color.blue);
                        float r = mat.color.r;
                        r -= Time.deltaTime * speed;

                        float g = mat.color.g;
                        g -= Time.deltaTime * speed;

                        mat.color = new Color(r, g, 1);
                       
                        dropLine.SetPosition(1, new Vector3(0, 0.3f, 0));
                        if (r < 0.5f && g < 0.5)
                        {
                            donePouring = true;
                            if (CallMethodOnPouringDone!= null)
                            {
                                CallMethodOnPouringDone.Invoke();
                                return;
                            }
                        }
                    }
                    else
                    {
                        Debug.DrawRay(pourPoint.position, pourPoint.up, Color.red);
                        dropLine.SetPosition(1, new Vector3(0, 0, 0));
                    }

                }
                else
                {
                    Debug.DrawRay(pourPoint.position, pourPoint.up, Color.yellow);
                    dropLine.SetPosition(1, new Vector3(0, 0, 0));

                }
            }
        }

    }
}
