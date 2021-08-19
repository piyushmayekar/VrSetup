using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Exp1Manager : MonoBehaviour
{
    public static Exp1Manager instance;
    public A_ReadStepsAndVideoManager readSteps;
    public GameObject myCanvas, finishCanvas;
    public int cntSteps = 1;
    public A_AudioManagerWithLanguage audioWithStep;

    [Header("PPE KIT Colliders")]
    public Collider[] ppeKitColliders;
    public List<int> ppeIndex;
    public List<Vector3> ppePos;
    public List<Quaternion> ppeRot;

    public Collider firstAidKit;

    public GameObject[] otherObj;

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
        for (int i = 0; i < ppeKitColliders.Length; i++)
        {
            ppeKitColliders[i].enabled = false;
            ppeKitColliders[i].GetComponent<Outline>().enabled = false;
            ppeIndex.Add(i);
            ppePos.Add(ppeKitColliders[i].transform.localPosition);
            ppeRot.Add(ppeKitColliders[i].transform.localRotation);
        }
        firstAidKit.enabled = false;
        firstAidKit.GetComponent<Outline>().enabled = false; 
        ppePos.Add(firstAidKit.transform.localPosition);
        ppeRot.Add(firstAidKit.transform.localRotation);

        readSteps.AddClickConfirmbtnEvent(SetTextOnCanvas);
    }

    public void SetTextOnCanvas()
    {
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(OnNextBtnClick);

        if (cntSteps == 3)
        {
            myCanvas.SetActive(false);
            finishCanvas.SetActive(true);
        }
    }

    public void OnNextBtnClick()
    {
        Debug.Log(cntSteps);
        readSteps.HideConifmBnt();
        switch (cntSteps)
        {
            case 0:
                StartCoroutine(WaitNCall());
                break;
            case 1:
                EnablePpeKit();
                break;
            case 2:
                EnableFirstAid();
                break;
            case 3:
                StartCoroutine(WaitNCall());
                myCanvas.SetActive(false);
                finishCanvas.SetActive(true);
                break;
            default:
                myCanvas.SetActive(false);
                finishCanvas.SetActive(true);
                break;
        }
    }

    void EnableFirstAid()
    {
        firstAidKit.enabled = true;
        firstAidKit.GetComponent<Outline>().enabled = true;
    }

    public void FirstAidSelect()
    {
        audioWithStep.OtherAudio(9); 
        readSteps.PPEKit(9);
    }

    public void FirstAidExit()
    {
        firstAidKit.transform.localPosition = ppePos[9];
        firstAidKit.transform.localRotation = ppeRot[9];

        if (firstAidKit.GetComponent<XRGrabInteractable>() != null)
        {
            Destroy(firstAidKit.GetComponent<XRGrabInteractable>());
        }
        if (firstAidKit.GetComponent<Outline>() != null)
        {
            Destroy(firstAidKit.GetComponent<Outline>());
        }

        SetTextOnCanvas();
    }

    public void OnPPESelect(int index)
    {
        audioWithStep.OtherAudio(index);
        if(ppeIndex.Contains(index))
            ppeIndex.Remove(index);
        readSteps.PPEKit(index);
    }
    public void OnPPESelectObj(GameObject obj)
    {
        obj.GetComponent<Outline>().enabled = false;
    }

    public void ONPPEExit(int index)
    {
        StartCoroutine(SetDefault(index));
    }

    IEnumerator SetDefault(int index)
    {
        yield return new WaitForSeconds(0.1f);

        ppeKitColliders[index].transform.localPosition = ppePos[index];
        ppeKitColliders[index].transform.localRotation = ppeRot[index];

        if (ppeIndex.Count <= 0)
        {
            DisablePpeKit();
            SetTextOnCanvas();
        }
    }

    IEnumerator WaitNCall()
    {
        if(audioWithStep.GetComponent<AudioSource>().isPlaying)
            yield return new WaitForSeconds(1f);
        SetTextOnCanvas();
    }

    private void EnablePpeKit()
    {
        for (int i = 0; i < ppeKitColliders.Length; i++)
        {
            ppeKitColliders[i].enabled = true;
            ppeKitColliders[i].GetComponent<Outline>().enabled = true;
        }
    }

    private void DisablePpeKit()
    {
        for (int i = 0; i < ppeKitColliders.Length; i++)
        {
            if (ppeKitColliders[i].GetComponent<XRGrabInteractable>() != null)
            {
                Destroy(ppeKitColliders[i].GetComponent<XRGrabInteractable>());
            }
            if (ppeKitColliders[i].GetComponent<CustomXRGrabInteractable>() != null)
            {
                Destroy(ppeKitColliders[i].GetComponent<CustomXRGrabInteractable>());
            }
            if (ppeKitColliders[i].GetComponent<TwoHandGrabInteractable>() != null)
            {
                Destroy(ppeKitColliders[i].GetComponent<TwoHandGrabInteractable>());
            }
            if (ppeKitColliders[i].GetComponent<Outline>() != null)
            {
                Destroy(ppeKitColliders[i].GetComponent<Outline>());
            }
        }
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
