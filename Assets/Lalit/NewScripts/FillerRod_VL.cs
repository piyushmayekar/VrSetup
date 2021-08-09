using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class FillerRod_VL : MonoBehaviour
{
    public bool OnWeldingPoint;

    private Material defaultMat;
    public Material redMat;
    public MeshRenderer MeltPart;
    private float reducetolength = 0.002f;
    Rigidbody rb;

    bool heated = false;
    public float timeToCoolDown = 10;
    private float _time = 0;
    public float speed = 2;

    public bool freezXPos;
    public bool freezZPos;


    public GameObject light;
    public ParticleSystem effect;
    public AudioSource weldingSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultMat = MeltPart.sharedMaterial;
    }


    private void Update()
    {
        if (heated)
        {
            _time -= Time.deltaTime * 2;

            if (_time<= 0)
            {
                MeltPart.sharedMaterial = defaultMat;
                heated = false;
            }
        }
    }
    public void LockRotation(Transform HL)
    {
        Vector3 pos = transform.localPosition;
        pos = transform.position;
        HL.transform.localPosition = pos;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        if (freezXPos)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
        }
        else if (freezZPos)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        gameObject.GetComponent<XRGrabInteractable>().trackRotation = false;
    }

    public void UnlockRotation()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;
        gameObject.GetComponent<XRGrabInteractable>().trackRotation = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GasWeldingPoint")
        {
            OnWeldingPoint = true;
            //transform.GetComponentInChildren<MeshRenderer>().sharedMaterial = redMat;
            MeltPart.sharedMaterial = redMat;
           
        }

        if (other.tag == "FillerRodHL")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
            LockRotation(other.transform);
        }

        if (other.tag == "Flame")
        {
            //Debug.Log("InteractingWithflame");
        }

       
    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.tag == "GasWeldingPoint")
        {
            OnWeldingPoint = false;
            // transform.GetComponentInChildren<MeshRenderer>().sharedMaterial = defaultMat;
            MeltPart.sharedMaterial = defaultMat;
        }

        if (other.tag == "Flame")
        {
           // Debug.Log("Not_InteractingWithflame");
        }

    }

    public void ReduceScale()
    {
        Vector3 scale = transform.localScale;
        scale.y -= 0.002f;
        transform.localScale = scale;
        MeltPart.sharedMaterial = redMat;
        _time = timeToCoolDown;

        //StartCoroutine(PlayEffect());
    }

    public void  PlayEffect(bool on)
    {
        if (light)
        {
            light.SetActive(on);
        }

        if (effect)
        {
            if (on)
            {
                effect.Play();
            }
            else
            {
                effect.Stop();
            }
            
        }

        if (weldingSound)
        {
            if (on)
            {
                weldingSound.Play();
            }
            else
            {
                weldingSound.Stop();
            }

        }

       
    }



    
}
