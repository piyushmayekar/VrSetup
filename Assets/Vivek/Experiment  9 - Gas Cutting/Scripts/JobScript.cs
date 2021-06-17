using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobScript : MonoBehaviour
{
    public BenchWise benchWise;

    public float filingDuration = 10;
    float speed = 2;

    public float brushDuration = 5;
    float br_speed = 1.5f;

    public Mesh finishMesh;
    public MeshFilter meshfilter;
    public MeshRenderer meshRenderer;
    public Material finishMat;

    public bool UseCS;
    public bool UseBrush;
    public static bool UseScriber;
    public static bool UseDotPunch;
    public static bool firstCutDone;
    public static bool SecondCutDone;

    public static bool doneMarking = false;

    public GameObject markingQuad;
    public GameObject Line1;
    public GameObject Line2;

    public Transform Cut1;
    public Transform Cut2;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "FlatFile")
        {
            // PlayAudio.ins.PlayFileSound(true);
        }

        if (collision.collider.tag == "HackSawBlade")
        {
            //  PlayAudio.ins.PlayHackSawSound(true);
        }

        if (collision.collider.tag == "FlatBrush")
        {
            // PlayAudio.ins.PlayBruchSound();
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "FlatFile")
        {
            benchWise.showToolHighLight = false;
            filingDuration -= Time.deltaTime * speed;

            if (filingDuration <= 0)
            {
                benchWise.SecondFilingDone = true;
                meshfilter.mesh = finishMesh;
                meshRenderer.sharedMaterial = finishMat;

            }
        }

        if (collision.collider.tag == "FlatBrush")
        {
            brushDuration -= Time.deltaTime * br_speed;
            if (brushDuration <= 0 && !UseBrush)
            {
                Splash[] S = GameObject.FindObjectsOfType<Splash>();
                foreach (Splash item in S)
                {
                    Destroy(item.gameObject);
                }
                UseBrush = true;
                TaskBoard.ins.ProceedButton.enabled = true;
                TaskBoard.ins.ToggleButtonColor(true);
                markingQuad.SetActive(true);


            }
        }


    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "FlatFile")
        {
            // PlayAudio.ins.PlayFileSound(false);
        }

        if (collision.collider.tag == "HackSawBlade")
        {
            // PlayAudio.ins.PlayHackSawSound(false);
        }
    }

    public void PrepareJobForFirstCut()
    {
        Line1.SetActive(false);
        Line2.SetActive(false);
        Vector3 scale = transform.localScale;
        scale.x = 0.86f;
        transform.localScale = scale;
    }

    public void SetJobStateForTaskCutting()
    {
        markingQuad.SetActive(false);

        DrawLine(Line1, Line2);

    }

    public void SetScaleForSecondCut(float value)
    {
        Vector3 scale = transform.localScale;
        scale.x = value;
        transform.localScale = scale;
    }

    public void DrawLine(GameObject L1, GameObject L2)
    {
        L1.SetActive(true);
        LineRenderer line = L1.GetComponent<LineRenderer>();
        line.SetPosition(1, new Vector3(0.44f, 0.0106f, 0));

        L2.SetActive(true);
        LineRenderer line2 = L2.GetComponent<LineRenderer>();
        line2.SetPosition(1, new Vector3(0.44f, 0.0106f, 0));
    }

}
