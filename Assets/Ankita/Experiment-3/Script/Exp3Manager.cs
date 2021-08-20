using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;
using TMPro;

public class Exp3Manager : MonoBehaviour
{
    public static Exp3Manager instance;
    public A_ReadStepsAndVideoManager readSteps;
    public GameObject myCanvas, finishCanvas;
    public int cntSteps = 1;
    public A_AudioManagerWithLanguage audioWithStep;

    public GameObject[] otherObj;

    [Header("Tools for Step 1")]
    public GameObject canvas;
    public int innerStep = 0;
    public GameObject benchSwitch;

    [Header("Tools for Step 2")]
    public GameObject handSwitch;
    public GameObject handGrinder;
    public Collider handGrindCollider;

    [Header("Tools for Step 3")]
    public GameObject benchHighlight1; 
    public GameObject benchHighlight2, benchWise, handle;

    [Header("Tools for Step 4")]

    public GameObject plug1Impression;
    public GameObject plug2Impression, plug1, plug2, knobHighlight, knob, powerSwitch, voltReading;
    public GameObject machineOnCanvas, machineOffCanvas;

    public Quaternion rot;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Awake()
    {
        DisableOther();
        rot = powerSwitch.transform.localRotation;

        benchSwitch.GetComponent<XRSimpleInteractable>().enabled = false;

        handSwitch.GetComponent<XRSimpleInteractable>().enabled = false;

        handGrinder.GetComponent<Rigidbody>().useGravity = false;
        handGrindCollider.enabled = false;

        handle.GetComponent<XRGrabInteractable>().enabled = false;
        powerSwitch.GetComponent<XRSimpleInteractable>().enabled = false;
        knob.GetComponent<XRGrabInteractable>().enabled = false;
        plug1.GetComponent<XRGrabInteractable>().enabled = false;
        plug2.GetComponent<XRGrabInteractable>().enabled = false;

        voltReading.SetActive(false);

        readSteps.AddClickConfirmbtnEvent(SetTextOnCanvas);
    }

    public void SetTextOnCanvas()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnNextBtnClick);
    }

    public void OnNextBtnClick()
    {
        Debug.Log(cntSteps);
        readSteps.HideConifmBnt();
        switch (cntSteps)
        {
            case 0:
                Step1Work(1);
                break;
            case 1:
                Step2Work(1);
                break;
            case 2:
                Step3Work(1);
                break;
            case 3:
                handGrinder.GetComponent<Rigidbody>().useGravity = false;
                handGrindCollider.enabled = false;
                Step4Work(1);
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void Step1Work(int cnt)
    {
        switch (cnt)
        {
            case 1:
                innerStep = 1;
                benchSwitch.GetComponent<XRSimpleInteractable>().enabled = true;
                benchSwitch.transform.parent.GetComponent<Outline>().enabled = true;
                break;
            case 2:
                canvas.transform.Find("Canvas/BG/Text").GetComponent<TMP_Text>().text = "Switch OFF\nfrom here";
                break;
            case 3:
                canvas.SetActive(false);
                benchSwitch.transform.parent.GetComponent<Outline>().enabled = false;
                benchSwitch.GetComponent<XRSimpleInteractable>().enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }
    public void BenchGrinderOnOff()
    {
        innerStep++;
        Step1Work(innerStep);
    }

    private void Step2Work(int cnt)
    {
        switch (cnt)
        {
            case 1:
                innerStep = 1;
                handSwitch.GetComponent<XRSimpleInteractable>().enabled = true;
                handGrindCollider.enabled = true;
                handGrinder.GetComponent<Rigidbody>().useGravity = true;
                handGrinder.GetComponent<Outline>().enabled = true;
                break;
            case 2:
                break;
            case 3:
                handGrinder.GetComponent<Outline>().enabled = false;
                handSwitch.GetComponent<XRSimpleInteractable>().enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    public void HandGrinderOnOff()
    {
        innerStep++;
        Step2Work(innerStep);
    }

    private void Step3Work(int cnt)
    {
        switch (cnt)
        {
            case 1:
                innerStep = 1;
                handle.GetComponent<XRGrabInteractable>().enabled = true;
                benchWise.GetComponent<Outline>().enabled = true;
                benchHighlight1.SetActive(true);
                break;
            case 2:
                benchHighlight1.SetActive(false);
                benchHighlight2.SetActive(true);
                break;
            case 3:
                handle.GetComponent<XRGrabInteractable>().enabled = false;
                benchHighlight2.SetActive(false);
                benchWise.GetComponent<Outline>().enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    public void BenchWiseRotate()
    {
        Debug.Log("Bench Wise");
        innerStep++;
        Step3Work(innerStep);
    }

    private void Step4Work(int cnt)
    {
        switch (cnt)
        {
            case 1:
                voltReading.transform.parent.GetComponent<Outline>().enabled = true;
                innerStep = 1;
                plug1Impression.SetActive(true);
                plug2Impression.SetActive(true);

                plug1.GetComponent<Outline>().enabled = true;
                plug2.GetComponent<Outline>().enabled = true;
                plug1.GetComponent<XRGrabInteractable>().enabled = true;
                plug2.GetComponent<XRGrabInteractable>().enabled = true;
                break;
            case 2:
                voltReading.transform.parent.GetComponent<Outline>().enabled = false;
                break;
            case 3:
                plug1.GetComponent<Outline>().enabled = false;
                plug2.GetComponent<Outline>().enabled = false;

                machineOnCanvas.SetActive(true);
                powerSwitch.GetComponent<Outline>().enabled = true;
                powerSwitch.GetComponent<XRSimpleInteractable>().enabled = true;
                break;
            case 4:
                voltReading.SetActive(true);
                knob.GetComponent<XRGrabInteractable>().enabled = true;
                knob.GetComponent<Outline>().enabled = true;
                knobHighlight.SetActive(true);
                break;
            case 5:
                machineOffCanvas.SetActive(true);
                powerSwitch.GetComponent<Outline>().enabled = true;
                powerSwitch.GetComponent<XRSimpleInteractable>().enabled = true;
                break;
            case 6:
                powerSwitch.GetComponent<Outline>().enabled = false;
                powerSwitch.GetComponent<XRSimpleInteractable>().enabled = false;

                SetTextOnCanvas();
                finishCanvas.SetActive(true);
                myCanvas.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void OnPowerSwitch()
    {
        if (innerStep == 3)
        {
            powerSwitch.transform.localEulerAngles = new Vector3(-170f, powerSwitch.transform.localEulerAngles.y, powerSwitch.transform.localEulerAngles.z);
            powerSwitch.GetComponent<AudioSource>().Play();

            powerSwitch.transform.Find("Handle").GetComponent<AudioSource>().Play();
            machineOnCanvas.SetActive(false);
        }
        else
        {
            powerSwitch.transform.localRotation = rot;
            powerSwitch.GetComponent<AudioSource>().Play();

            powerSwitch.transform.Find("Handle").GetComponent<AudioSource>().Stop();
            machineOffCanvas.SetActive(false);
        }

        powerSwitch.GetComponent<XRSimpleInteractable>().enabled = false;

        innerStep++;
        Step4Work(innerStep);
    }

    public void OnPlugRelease(GameObject plug)
    {
        innerStep++;
        plug.GetComponent<machin_reset>().enabled = true; 
        plug.transform.parent.Find("Plug Nut").GetComponent<machin_reset>().enabled = true;
        plug.transform.parent.Find("Plug Screw").GetComponent<machin_reset>().enabled = true;
        if (plug.transform.parent.name.Contains("1"))
        {
            plug1Impression.SetActive(false);
        }
        else
        {
            plug2Impression.SetActive(false);
        }
        Step4Work(innerStep);
    }

    public void AfterKnob()
    {
        knob.GetComponent<XRGrabInteractable>().enabled = false;
        knob.GetComponent<Outline>().enabled = false;
        knobHighlight.SetActive(false);
        innerStep++;
        Step4Work(innerStep);
    }

    private void DisableOther()
    {
        for (int i = 0; i < otherObj.Length; i++)
        {
            if (otherObj[i].GetComponent<XRGrabInteractable>() != null)
            {
                Destroy(otherObj[i].GetComponent<XRGrabInteractable>());
            }
            if (otherObj[i].GetComponent<CustomXRGrabInteractable>() != null)
            {
                Destroy(otherObj[i].GetComponent<CustomXRGrabInteractable>());
            }
            if (otherObj[i].GetComponent<TwoHandGrabInteractable>() != null)
            {
                Destroy(otherObj[i].GetComponent<TwoHandGrabInteractable>());
            }
            if (otherObj[i].GetComponent<Outline>() != null)
            {
                Destroy(otherObj[i].GetComponent<Outline>());
            }
        }
    }
}
