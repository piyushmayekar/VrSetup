using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkingPointGet : MonoBehaviour
{
    public Action OnMarkingDone;

    [SerializeField] Renderer _renderer;
    [SerializeField] List<GameObject> highlights;
    public GameObject enableobject;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MarkPoint")&& other.gameObject.name == "hitpoint" && this.gameObject.name == "MarkingPoint2")
        {

            GetComponent<Collider>().enabled = false;
            _renderer.enabled = true;
            highlights.ForEach(x => x.SetActive(false));
            OnMarkingDone?.Invoke();
        }
        else if (other.CompareTag("SteelRuler")&& other.gameObject.name =="SteelRule" && this.gameObject.tag == "SteelRuleHighlight")
        {
            this.gameObject.GetComponent<Collider>().enabled = false;
            _renderer.enabled = true;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            // enableobject.SetActive(true);
            highlights.ForEach(x => x.SetActive(false));
            OnMarkingDone?.Invoke();
        }
    }
}