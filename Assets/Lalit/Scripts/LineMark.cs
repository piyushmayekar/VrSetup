using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMark : MonoBehaviour
{
    public GameObject Next;
    public bool hasNext;
    public LineRenderer line;
    public Vector3 pos;
    public Vector3 cPos;
    public bool isLast;
    public bool secondCut;
    public bool firstCut;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MarkPoint")
        {
            // Debug.Log("touched");
            if (hasNext)
            {
                Next.SetActive(true);
               // PlayAudio.ins.PlayScriberSound();

            }
            line.SetPosition(1, pos);
            if (isLast)
            {
                Job.UseScriber = true;
                TaskBoard.ins.ProceedButton.enabled = true;
                TaskBoard.ins.ToggleButtonColor(true);
            }
            this.enabled = false;
        }

        if (other.gameObject.tag == "HackSawBlade")
        {
            if (hasNext)
            {
                Next.SetActive(true);
            }

            if (isLast)
            {
                line.SetPosition(0, cPos);
                line.SetPosition(1, cPos);
                if (secondCut)
                {
                    Job.SecondCutDone = true;
                }
                else if (firstCut)
                {
                    Job.firstCutDone = true;

                }

                Destroy(gameObject.transform.parent.gameObject);

            }
            else
            {
                line.SetPosition(0, cPos);
            }




        }
    }
}
