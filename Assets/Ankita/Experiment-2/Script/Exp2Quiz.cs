using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using PiyushUtils;

public class Exp2Quiz : MonoBehaviour
{
    public static Exp2Quiz instance;
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
    public int quizCount = 0;
    public int wrongAns = 0;

    public GameObject wrongPanel, rightPanel;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Awake()
    {
        quizCount = 0;
        wrongAns = 0;
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
        if (index == quizCount)
        {
            Debug.Log("correct");
            audioWithStep.RightWrongAudio(0);
            rightPanel.SetActive(true);
            wrongPanel.SetActive(false);
        }
        else
        {
            Debug.Log("Not Correct");
            audioWithStep.RightWrongAudio(1);
            rightPanel.SetActive(false);
            wrongPanel.SetActive(true);
        }
        if(toolsIndex.Contains(index))
            toolsIndex.Remove(index);
    }

    public void OnPPESelectObj(GameObject obj)
    {
        //obj.GetComponent<Outline>().enabled = false;
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

        toolsColliders[index].GetComponent<Outline>().enabled = false;

        if (toolsIndex.Count <= 0)
        {
            DisablePpeKit();
            SetTextOnCanvas();
            myCanvas.SetActive(false);
            finishCanvas.SetActive(true);

            rightPanel.SetActive(false);
            wrongPanel.SetActive(false);
        }
        else
        {
            if (index == quizCount)
            {
                wrongAns = 0;
                Debug.Log("Exit correct");
                rightPanel.SetActive(false);
                wrongPanel.SetActive(false);
                quizCount++;
                EnableQuiz();
            }
            else
            {
                Debug.Log("Exit Not Correct");
                wrongAns++;
                if (wrongAns >= 2)
                {
                    toolsColliders[quizCount].GetComponent<Outline>().enabled = true;
                }
            }
        }
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
        }
        EnableQuiz();
    }

    private void EnableQuiz()
    {
        audioWithStep.OtherAudio(quizCount);
        readSteps.PPEKit(quizCount);
    }

    private void DisablePpeKit()
    {
        for (int i = 0; i < toolsColliders.Length; i++)
        {
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
