using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SnapGrabbleObject : MonoBehaviour
{
    [SerializeField]
    public GameObject SnapObject;
  //  OVRGrabbable EndGrabbable;
    public bool connected;
    public GameObject[] SnapObject_new;
    public GameObject[] HideObject;
    public float distance;
    public UnityEvent callAnyOtherMethod;
    public bool isHideThis, isLighter;
    public string pipeTag;

    // Start is called before the first frame update
    void Start()
    {
        //    connected = false;
        //transform.eulerAngles = new Vector3(360, 180, 270);
       // EndGrabbable = GetComponent<OVRGrabbable>();
    }
    void Update()
    {
        if (!connected)
        {
            if (Vector3.Distance(transform.position, SnapObject.transform.position) < distance)
            {

                for (int i = 0; i < SnapObject_new.Length; i++)
                {
                    SnapObject_new[i].SetActive(true);
                }
                for (int i = 0; i < HideObject.Length; i++)
                {
                    HideObject[i].SetActive(false);
                }

                if (callAnyOtherMethod != null)
                {
                    //    Debug.Log("calllllllllllllllllllllll");
                    callAnyOtherMethod.Invoke();
                }
                if (!isHideThis)
                {
                    this.gameObject.SetActive(false);
                }
                else
                {
                 //   EndGrabbable.enabled = false;

                }
               // EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
               // EndGrabbable.enabled = false;
                connected = true;
                //    OnSnap();

            }
        }

    }

    public void OnSnap()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == pipeTag)
        {

            for (int i = 0; i < SnapObject_new.Length; i++)
            {
                SnapObject_new[i].SetActive(true);
            }
            for (int i = 0; i < HideObject.Length; i++)
            {
                HideObject[i].SetActive(false);
            }

            if (callAnyOtherMethod != null)
            {
                //    Debug.Log("calllllllllllllllllllllll");
                callAnyOtherMethod.Invoke();
            }
            if (!isHideThis)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
         //       EndGrabbable.enabled = false;

            }
           // EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
           // EndGrabbable.enabled = false;
            connected = true;
        }
        else if (isLighter && (other.gameObject.tag == "RightHand" || other.gameObject.tag == "LeftHand"))
        {
            StartCoroutine(Lighter());
          
        }

    }
    IEnumerator Lighter()
    {
        isLighter = false;
        yield return new WaitForSeconds(1.5f);
        GasWeldingStep.instance.lighter_Flame.SetActive(true);
    }
}

/*[SerializeField]
Transform SnapPoint;
OVRGrabbable EndGrabbable;
bool connected;

public Transform newPos;
// Start is called before the first frame update
void Start()
{
    connected = false;
    //transform.eulerAngles = new Vector3(360, 180, 270);
    EndGrabbable = GetComponent<OVRGrabbable>();
}
void Update()
{
    if (!connected)
    {
        if (Vector3.Distance(transform.position, newPos.position) < 0.2f)
        {

            transform.position = newPos.position;

            transform.rotation = SnapPoint.rotation;

            EndGrabbable.UnGrabMe(Vector3.zero, Vector3.zero);
            EndGrabbable.enabled = false;
            connected = true;
            OnSnap();

        }
    }

}
public void OnSnap()
{

    this.GetComponent<MeshRenderer>().material.color = Color.red;
}*/
