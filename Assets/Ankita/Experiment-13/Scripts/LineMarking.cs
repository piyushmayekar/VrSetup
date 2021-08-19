using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMarking : MonoBehaviour
{
    public int _index;
    public static int lastIndex = -1;
    public SoundPlayer soundPlayer;

    private void OnEnable()
    {
        lastIndex = -1;
        _index = transform.GetSiblingIndex();
    }

    private void Start()
    {
    }
    public void PlayHitSound()
    {
        soundPlayer.PlayClip(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Scriber"))
        {
            if (lastIndex == _index - 1)
            {
                PlayHitSound();
                lastIndex = _index;
                transform.parent.GetComponent<LineRenderer>().positionCount++;
                transform.parent.GetComponent<LineRenderer>().SetPosition(_index, transform.localPosition);
                //GetComponent<LineMarking>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
            }
        }
        if (transform.parent.GetComponent<LineRenderer>().positionCount >= transform.parent.childCount)
        {
            if (transform.parent.name.Contains("1"))
            {
                ExperimentFlowManager.instance.Step2_EnableHighlighting(4);
            }
            else
            {
                ExperimentFlowManager.instance.Step2_EnableHighlighting(7);
            }
        }
    }
}
