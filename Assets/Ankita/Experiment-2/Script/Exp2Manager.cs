using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;

public class Exp2Manager : MonoBehaviour
{
    public static Exp2Manager instance;
    public A_ReadStepsAndVideoManager readSteps;
    public GameObject myCanvas, finishCanvas;
    public int cntSteps = 1;
    public A_AudioManagerWithLanguage audioWithStep;

    [Header("Tools Colliders")]
    public GameObject[] toolsColliders;
    public List<int> toolsIndex;
    public List<Vector3> toolsPos;
    public List<Quaternion> toolsRot;

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
        for (int i = 0; i < toolsColliders.Length; i++)
        {
            if (toolsColliders[i].GetComponent<XRGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<XRGrabInteractable>().enabled = false;
            }
            if (toolsColliders[i].GetComponent<CustomXRGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<CustomXRGrabInteractable>().enabled = false;
            }
            if (toolsColliders[i].GetComponent<TwoHandGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<TwoHandGrabInteractable>().enabled = false;
            }
            toolsColliders[i].GetComponent<Outline>().enabled = false;
            toolsIndex.Add(i);
            toolsPos.Add(toolsColliders[i].transform.localPosition);
            toolsRot.Add(toolsColliders[i].transform.localRotation);
        }
        readSteps.AddClickConfirmbtnEvent(OnNextBtnClick);
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
                EnablePpeKit();
                break;
            case 1:
                EnablePpeKit();
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    public void OnPPESelect(int index)
    {
        audioWithStep.OtherAudio(index);
        if(toolsIndex.Contains(index))
            toolsIndex.Remove(index);
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

        toolsColliders[index].transform.localPosition = toolsPos[index];
        toolsColliders[index].transform.localRotation = toolsRot[index];

        if (toolsIndex.Count <= 0)
        {
            DisablePpeKit();
            SetTextOnCanvas();
            myCanvas.SetActive(false);
            finishCanvas.SetActive(true);
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
        for (int i = 0; i < toolsColliders.Length; i++)
        {
            if (toolsColliders[i].GetComponent<XRGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<XRGrabInteractable>().enabled = true;
            }
            if (toolsColliders[i].GetComponent<CustomXRGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<CustomXRGrabInteractable>().enabled = true;
            }
            if (toolsColliders[i].GetComponent<TwoHandGrabInteractable>() != null)
            {
                toolsColliders[i].GetComponent<TwoHandGrabInteractable>().enabled = true;
            }
            toolsColliders[i].GetComponent<Outline>().enabled = true;
        }
    }

    private void DisablePpeKit()
    {
        for (int i = 0; i < toolsColliders.Length; i++)
        {
            //toolsColliders[i].enabled = false;
            if (toolsColliders[i].GetComponent<XRGrabInteractable>() != null)
            {
                Destroy(toolsColliders[i].GetComponent<XRGrabInteractable>());
            }
            if (toolsColliders[i].GetComponent<CustomXRGrabInteractable>() != null)
            {
                Destroy(toolsColliders[i].GetComponent<CustomXRGrabInteractable>());
            }
            if (toolsColliders[i].GetComponent<TwoHandGrabInteractable>() != null)
            {
                Destroy(toolsColliders[i].GetComponent<TwoHandGrabInteractable>());
            }
            toolsColliders[i].GetComponent<Outline>().enabled = false;
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
