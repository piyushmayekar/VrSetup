using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Experiment15FlowManager : MonoBehaviour
{
    public static Experiment15FlowManager instance;

    public A_ReadStepsAndVideoManager readSteps;
    public GameObject myCanvas, finishCanvas;

    [Header("PPE KIT Colliders")]
    public Collider[] ppeKitColliders;

    [Header("Tools for Step 2")]
    public GameObject finalSheet1;
    public GameObject finalSheet2;

    [Header("Tools for Step 3")]
    public GameObject brush;

    [Header("Tools for Step 4")]
    public GameObject finalSheet1Impression;
    public GameObject finalSheet2Impression;



    [Header("Tools for Step 11")]
    public GameObject weldPointObj;
    public Collider flameCube;
    public GameObject fillerRod, torch, fillerRodImpression, torchImpression;

    [Header("Tools for Step 12")]
    public GameObject weldLine;

    [Header("Tools for Step 13")]
    public GameObject chippingImpression;
    public GameObject chippingHammer;


    [Header("Comman")]
    public int cntSteps = 1;
    public int ppeCount = 0;
    public int innerStep = 1;


    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Awake()
    {
        for (int i = 0; i < ppeKitColliders.Length; i++)
        {
            ppeKitColliders[i].enabled = false;
            ppeKitColliders[i].GetComponent<Outline>().enabled = false;
        }
    }

    public void SetTextOnCanvas()
    {
        Debug.Log(cntSteps);
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
                Step3_EnableHighlighting(1);
                break;
            case 2:
                Step4_EnableHighlighting(1);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
            case 16:
            case 17:
            case 18:
            case 19:

            case 22:
            case 23:
                break;

            case 20:
                Step13_EnableHighlighting(1);
                break;
            case 21:
                Step14_EnableHighlighting(1);
                break;

            case 24:
                Step16_EnableHighlighting(1);
                break;
            case 25:
                Step17_EnableHighlighting(1);
                break;
            case 26:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 27:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            default:
                break;
        }
    }

    #region Step 1: Wear PPE Kit
    private void EnablePpeKit()
    {
        for (int i = 0; i < ppeKitColliders.Length; i++)
        {
            ppeKitColliders[i].enabled = true;
            ppeKitColliders[i].GetComponent<Outline>().enabled = true;
        }
    }

    public void OnPpeClick(GameObject selectedObj)
    {
        ppeCount++;
        selectedObj.SetActive(false);
        if (ppeCount == ppeKitColliders.Length)
        {
            SetTextOnCanvas();
        }
    }
    #endregion

    #region Step 2: Clean both jobs by Wire brush.

    public void Step3_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                brush.GetComponent<Outline>().enabled = true;
                finalSheet1.GetComponent<BrushDetectionExp14>().enabled = true;
                finalSheet2.GetComponent<BrushDetectionExp14>().enabled = true;
                finalSheet1.GetComponent<BrushDetectionExp14>().SetText(1);
                finalSheet2.GetComponent<BrushDetectionExp14>().SetText(2);
                break;
            case 2:
                finalSheet1.GetComponent<BrushDetectionExp14>().ClearText();
                finalSheet2.GetComponent<BrushDetectionExp14>().ClearText();
                Destroy(finalSheet1.GetComponent<BrushDetectionExp14>());
                Destroy(finalSheet2.GetComponent<BrushDetectionExp14>());
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 3: Keep both jobs on Welding table for further steps.

    public void Step4_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 1;
                finalSheet1.GetComponent<Outline>().enabled = true;
                finalSheet2.GetComponent<Outline>().enabled = true;
                finalSheet1.GetComponent<XRGrabInteractable>().enabled = true;
                finalSheet2.GetComponent<XRGrabInteractable>().enabled = true;
                finalSheet1Impression.SetActive(true);
                finalSheet2Impression.SetActive(true);
                break;
            case 2:
                break;
            case 3:
                readSteps.onClickConfirmbtn();
                readSteps.AddClickConfirmbtnEvent(A_GasWelding.instance.Onclickbtn_s_2_confirm);
                break;
            default:
                break;
        }
    }
    #endregion


    #region Step 20: Do tack welding on both ends and centre of the job.

    public void Step13_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 1;
                torchImpression.SetActive(true);
                fillerRodImpression.SetActive(true);
                break;
            case 2:
                break;
            case 3:
                weldPointObj.SetActive(true);
                flameCube.enabled = true;
                break;
            case 4:
                //flameCube.enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 21: Start Welding by Leftward Technique.

    public void Step14_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                flameCube.GetComponent<WeldingJointExp14>().isWelding = true;
                weldLine.SetActive(true);
                flameCube.enabled = true;
                break;
            case 2:
                weldPointObj.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(3).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(4).GetComponent<MeshRenderer>().enabled = false;

                //readSteps.onClickConfirmbtn();
                readSteps.onClickConfirmbtn();
                readSteps.AddClickConfirmbtnEvent(A_GasWelding.instance.Onclickbtn_s_10_confirm);
                //SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion



    #region Step 24: Check the defects on Joints with the help of Chipping Hammer.

    public void Step16_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                chippingImpression.SetActive(true);
                chippingHammer.GetComponent<Outline>().enabled = true;
                break;
            case 2:
                chippingHammer.GetComponent<Outline>().enabled = false;
                weldLine.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                weldLine.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                chippingHammer.GetComponent<WeldingJointExp14>().enabled = true;
                break;
            case 3:
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 25: Clean the job surface with wire brush and remove distortion.

    public void Step17_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                brush.transform.GetChild(0).GetComponent<CleanSurface>().SetText();
                brush.transform.GetChild(0).GetComponent<CleanSurface>().enabled = true;
                weldPointObj.transform.GetChild(1).GetComponent<SphereCollider>().enabled = true;
                weldPointObj.transform.GetChild(2).GetComponent<SphereCollider>().enabled = true;
                weldPointObj.transform.GetChild(3).GetComponent<SphereCollider>().enabled = true;
                weldPointObj.transform.GetChild(4).GetComponent<SphereCollider>().enabled = true;
                break;
            case 2:
                readSteps.onClickConfirmbtn();
                myCanvas.SetActive(false);
                finishCanvas.SetActive(true);
                break;
            default:
                break;
        }
    }

    #endregion
}