using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlassManage : MonoBehaviour
{
    public string glassTag;

    public UnityEvent NextStep;
    public GameObject Parentgo;
    public GameObject[] SnapObject_new;
    public GameObject[] HideObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GlassDetect()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;

        Parentgo.GetComponent<Outline>().enabled = false;
    
        StartCoroutine(waiting());
    }
   

    IEnumerator waiting()
    {
        yield return new WaitForSeconds(1f);
        HideObject[0].SetActive(false);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < SnapObject_new.Length; i++)
        {
            SnapObject_new[i].SetActive(true);
        }
        for (int i = 0; i < HideObject.Length; i++)
        {
            HideObject[i].SetActive(false);
        }
        NextStep.Invoke();
        this.gameObject.SetActive(false);

    }
}