using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldJob : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public Material finishMat;
    public bool measeured;
    public bool filled;
    public bool markingDone;
    public GameObject Cut;
    public bool UseBrush;

    public GameObject CutLine;
    public GameObject CutMarkLine;

    public float filingDuration = 10;
    float speed = 2;

    public float brushDuration = 5;
    float br_speed = 1.5f;
    public GameObject HackSawHL;
    public bool needCut;


    public GameObject MarkingSocket;
    public bool cutMarkingDone;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (needCut && cutMarkingDone)
        {
            measeured = true;
            needCut = false;
           
        }
    }

    public void PrepareForCut()
    {
        CutMarkLine.SetActive(false);
        CutLine.SetActive(true);
        HackSawHL.SetActive(true);
    }

    public void RemoveCut()
    {
        Cut.gameObject.transform.parent = null;
        Cut.gameObject.AddComponent<Rigidbody>();
        CutMarkLine.SetActive(false);
        CutLine.SetActive(false);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "SteelRuler")
        {
            if (needCut)
            {
               
                ShowMarkingSocket();
            }
            else
            {
                measeured = true;
            }
            
        }

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
            filingDuration -= Time.deltaTime * speed;

            if (filingDuration <= 0)
            {
                filled = true;
                
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
               // markingQuad.SetActive(true);


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

    public void ShowMarkingSocket()
    {
        MarkingSocket.SetActive(true);
    }
}
