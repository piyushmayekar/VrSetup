using PiyushUtils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ExperimentFlowManager : MonoBehaviour
{
    public static ExperimentFlowManager instance;

    public A_ReadStepsAndVideoManager readSteps;
    public GameObject myCanvas, finishCanvas;

    [Header("PPE KIT Colliders")]
    public Collider[] ppeKitColliders;

    [Header("Tools for Step 2")]
    public GameObject ruler;
    public GameObject scriber, rulerImpression1, scriberImpression1, lineRenderer1, rulerImpression2, scriberImpression2, lineRenderer2;

    [Header("Tools for Step 3")]
    public GameObject brush;
    public GameObject sheet1, sheet2;

    [Header("Tools for Step 4")]
    public GameObject[] punchPoints;
    public GameObject hammerImpression, punchImpression, hammer, centerPunch, markingPoints1, markingPoints2;

    [Header("Tools for Step 5")]
    public GameObject sheetBenchImpression;
    public GameObject sheetTableImpression;

    [Header("Tools for Step 6")]
    public GameObject finalSheet1;
    public GameObject finalSheet2, finalSheetImpression, hammerVerticleImpression, handle, slidePart;
    public Vector3 openPosition, closePosition;

    [Header("Tools for Step 7")]
    public GameObject finalSheet1Impression;
    public GameObject finalSheet2Impression;


    [Header("Tools for Step 11")]
    public GameObject weldPointObj;
    public Collider flameCube;
    public GameObject torchImpression;
    public GameObject torchCollider;

    [Header("Tools for Step 12")]
    public GameObject weldLine;


    [Header("Comman")]
    public GameObject[] allMarkingPoints;
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
        readSteps.onClickConfirmbtn();
        readSteps.AddClickConfirmbtnEvent(ExperimentFlowManager.instance.OnNextBtnClick);
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
                Step2_EnableHighlighting(1);
                break;
            case 2:
                Step3_EnableHighlighting(1);
                break;
            case 3:
                Step4_EnableHighlighting(1);
                break;
            case 4:
                Step5_EnableHighlighting(1);
                break;
            case 5:
                Step6_EnableHighlighting(1);
                break;
            case 6:
                Step7_EnableHighlighting(1);
                break;
            case 7:
                Step8_EnableHighlighting(1);
                break;
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
            case 20:
            case 21:
            case 22:
            case 23:
            case 24:

            case 27:
            case 28:
                break;

            case 25:
                Step17_EnableHighlighting(1);
                break;
            case 26:
                Step18_EnableHighlighting(1);
                break;

            case 29:
                Step21_EnableHighlighting(1);
                break;
            case 30:
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case 31:
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

    #region Step 2: Keep raw material ready as per the given drawing.

    public void Step2_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 1;
                ruler.GetComponent<Outline>().enabled = true;
                rulerImpression1.SetActive(true);
                break;
            case 2:
                scriber.GetComponent<Outline>().enabled = true;
                scriberImpression1.SetActive(true);
                break;
            case 3:
                lineRenderer1.SetActive(true);
                break;
            case 4:
                innerStep = 4;
                for (int i = 0; i < lineRenderer1.transform.childCount; i++)
                {
                    Destroy(lineRenderer1.transform.GetChild(i).GetComponent<LineMarking>());
                    //lineRenderer1.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
                    lineRenderer1.transform.GetChild(i).gameObject.SetActive(false);
                }
                ruler.GetComponent<Outline>().enabled = true;
                ruler.GetComponent<CustomXRGrabInteractable>().enabled = true;
                rulerImpression2.SetActive(true);
                break;
            case 5:
                scriber.GetComponent<Outline>().enabled = true;
                //scriber.GetComponent<BoxCollider>().isTrigger = true;
                scriberImpression2.SetActive(true);
                break;
            case 6:
                lineRenderer2.SetActive(true);
                break;
            case 7:
                innerStep = 7;
                ruler.GetComponent<CustomXRGrabInteractable>().enabled = true;
                for (int i = 0; i < lineRenderer2.transform.childCount; i++)
                {
                    Destroy(lineRenderer2.transform.GetChild(i).GetComponent<LineMarking>());
                    //lineRenderer2.transform.GetChild(i).GetComponent<Collider>().isTrigger = false;
                    lineRenderer2.transform.GetChild(i).gameObject.SetActive(false);
                }
                for (int i = 0; i < allMarkingPoints.Length; i++)
                {
                    Destroy(allMarkingPoints[0]);
                }
                //ruler.SetActive(false);
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 3: Clean both the job surface with wire brush and remove burrs by filing.

    public void Step3_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                brush.GetComponent<Outline>().enabled = true;
                sheet1.GetComponent<BrushDetection>().enabled = true;
                sheet2.GetComponent<BrushDetection>().enabled = true;
                sheet1.GetComponent<BrushDetection>().SetText(1);
                sheet2.GetComponent<BrushDetection>().SetText(2);
                break;
            case 2:
                sheet1.GetComponent<BrushDetection>().ClearText();
                sheet2.GetComponent<BrushDetection>().ClearText();
                brush.GetComponent<Outline>().enabled = false;
                sheet1.GetComponent<BrushDetection>().enabled = false;
                sheet2.GetComponent<BrushDetection>().enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 4: As per the drawing, the surfaces of both the job should be marked and punched.

    public void Step4_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 0;
                hammerImpression.SetActive(true);
                punchImpression.SetActive(true);
                hammer.GetComponent<Outline>().enabled = true;
                centerPunch.GetComponent<Outline>().enabled = true;
                break;
            case 2:
                markingPoints1.SetActive(true);
                markingPoints2.SetActive(true);

                centerPunch.transform.GetComponent<CenterPunchMarking_A>().enabled = true;
                innerStep = -1; 
                PunchPoints();
                break;
            default:
                break;
        }
    }

    public void PunchPoints()
    {
        innerStep++;
        Debug.Log(innerStep + " --> " + punchPoints.Length);
        if (innerStep < punchPoints.Length)
        {
            punchPoints[innerStep].SetActive(true);
        }
        else
        {
            lineRenderer1.transform.parent = sheet1.transform;
            lineRenderer2.transform.parent = sheet2.transform;
            markingPoints1.transform.parent = sheet1.transform;
            markingPoints2.transform.parent = sheet2.transform;

            SetTextOnCanvas();
        }
    }

    #endregion

    #region Step 5: Take both job sheets to Bench wise table as per the drawing.    

    public void Step5_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 0;
                sheet1.GetComponent<Outline>().enabled = true;
                sheet2.GetComponent<Outline>().enabled = true;
                sheetBenchImpression.SetActive(true);
                sheetTableImpression.SetActive(true);
                sheet1.GetComponent<XRGrabInteractable>().enabled = true;
                sheet2.GetComponent<XRGrabInteractable>().enabled = true;
                break;
            case 2:
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 6: Now bend both job sheets one by one by using hammer as per the drawing.

    public void Step6_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                innerStep = 1;
                handle.GetComponent<XRGrabInteractable>().enabled = true;
                handle.GetComponent<Collider>().enabled = true;
                handle.transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = true;
                handle.AddComponent<BenchMachine>();
                break;
            case 2:
                hammerVerticleImpression.SetActive(true);
                hammer.GetComponent<Outline>().enabled = true;
                break;
            case 3:
                finalSheet1.SetActive(true);
                handle.GetComponent<XRGrabInteractable>().enabled = true;
                handle.GetComponent<Collider>().enabled = true;
                handle.transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = true;
                handle.AddComponent<BenchMachine>();
                break;
            case 4:
                finalSheet1.GetComponent<Outline>().enabled = true;
                finalSheet1.GetComponent<XRGrabInteractable>().enabled = true;
                finalSheetImpression.SetActive(true);
                break;
            case 5:
                StopCoroutine("myRotate");
                StopCoroutine("myReposition");
                if (sheet2.activeSelf)
                {
                    sheet2.GetComponent<Outline>().enabled = true;
                    sheet2.GetComponent<XRGrabInteractable>().enabled = true;
                    sheet2.GetComponent<Collider>().enabled = true;
                }
                else
                {
                    sheet1.GetComponent<Outline>().enabled = true;
                    sheet1.GetComponent<XRGrabInteractable>().enabled = true;
                    sheet1.GetComponent<Collider>().enabled = true;
                }
                finalSheet1.GetComponent<XRGrabInteractable>().enabled = false;
                sheetBenchImpression.SetActive(true);
                break;
            case 6:
                handle.GetComponent<XRGrabInteractable>().enabled = true;
                handle.GetComponent<Collider>().enabled = true;
                handle.transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = true;
                handle.AddComponent<BenchMachine>();
                break;
            case 7:
                hammerVerticleImpression.SetActive(true);
                hammer.GetComponent<Outline>().enabled = true;
                break;
            case 8:
                finalSheet2.SetActive(true);
                handle.GetComponent<XRGrabInteractable>().enabled = true;
                handle.GetComponent<Collider>().enabled = true;
                handle.transform.Find("Handle Highlight").GetComponent<MeshRenderer>().enabled = true;
                handle.AddComponent<BenchMachine>();
                break;
            case 9:
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    public void CallRotateReposition()
    {
        innerStep++;
        if (innerStep == 2)
        {
            StartCoroutine(myRotate(1, false));
            StartCoroutine(myReposition(1));
        }
        else if (innerStep == 7)
        {
            StartCoroutine(myRotate(1, true));
            StartCoroutine(myReposition(1));
        }
        else if (innerStep == 4)
        {
            StartCoroutine(myRotate(-1, false));
            StartCoroutine(myReposition(-1));
        }
        else
        {
            StartCoroutine(myRotate(-1, true));
            StartCoroutine(myReposition(-1));
        }
    }
    public float myRotateSpeed = 1.6f;
    IEnumerator myRotate(float speed, bool isSecond)
    {
        float angle;
        if (speed > 0)
        {
            if (isSecond)
            {
                angle = 85;
            }
            else
            {
                angle = 80;
            }
        }
        else
        {
            if (isSecond)
            {
                angle = 2;
            }
            else
            {
                angle = 5;
            }
        }        

        if (speed > 0)
        {
            while (handle.transform.localEulerAngles.z < angle)
            {
                handle.transform.Rotate(Vector3.forward * myRotateSpeed * speed);
                yield return new WaitForSeconds(0.005f);
                Debug.Log(handle.transform.localEulerAngles.z);
                if (handle.transform.localEulerAngles.z > angle)
                {
                    handle.transform.localEulerAngles = new Vector3(0, 0, angle);
                    break;
                }
            }
        }
        else
        {
            while (handle.transform.localEulerAngles.z > angle)
            {
                handle.transform.Rotate(Vector3.forward * myRotateSpeed * speed);
                yield return new WaitForSeconds(0.005f);
                Debug.Log(handle.transform.localEulerAngles.z);
                if (handle.transform.localEulerAngles.z < angle)
                {
                    handle.transform.localEulerAngles = new Vector3(0, 0, angle);
                    break;
                }
            }
        }
        Debug.Log("Final : " + handle.transform.localEulerAngles.z);

    }

    IEnumerator myReposition(float speed)
    {
        var i = 0.0f;
        var rate = 1.0f / 1.3f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            if(speed > 0)
                slidePart.transform.localPosition = Vector3.Lerp(openPosition, closePosition, i);
            else
                slidePart.transform.localPosition = Vector3.Lerp(closePosition, openPosition, i);

            yield return null;
        }
        Step6_EnableHighlighting(innerStep);
    }
    #endregion

    #region Step 7: Keep both job sheets on welding table in flat position.

    public void Step7_EnableHighlighting(int num)
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
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 8: Clean both the job surface with wire brush and remove burrs by filing.

    public void Step8_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                brush.GetComponent<Outline>().enabled = true;
                finalSheet1.GetComponent<BrushDetection>().SetText(1);
                finalSheet2.GetComponent<BrushDetection>().SetText(2);
                finalSheet1.GetComponent<BrushDetection>().enabled = true;
                finalSheet2.GetComponent<BrushDetection>().enabled = true;
                finalSheet1.GetComponent<Collider>().enabled = true;
                finalSheet2.GetComponent<Collider>().enabled = true;
                break;
            case 2:
                finalSheet1.GetComponent<BrushDetection>().ClearText();
                finalSheet2.GetComponent<BrushDetection>().ClearText();
                Destroy(finalSheet1.GetComponent<BrushDetection>());
                Destroy(finalSheet2.GetComponent<BrushDetection>());
                finalSheet1.GetComponent<Collider>().enabled = false;
                finalSheet2.GetComponent<Collider>().enabled = false;

                readSteps.onClickConfirmbtn();
                readSteps.AddClickConfirmbtnEvent(A_GasWelding.instance.Onclickbtn_s_2_confirm);
                break;
            default:
                break;
        }
    }

    #endregion




    #region Step 17: Do tack welding on both ends and centre of the job.

    public void Step17_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                torchCollider.SetActive(true);
                torchImpression.SetActive(true);
                break;
            case 2:
                //Destroy(torchCollider);
                weldPointObj.SetActive(true);
                flameCube.enabled = true;
                break;
            case 3:
                flameCube.enabled = false;
                SetTextOnCanvas();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Step 18: Start Welding by Leftward Technique.

    public void Step18_EnableHighlighting(int num)
    {
        switch (num)
        {
            case 1:
                flameCube.GetComponent<WeldingJoint>().isWelding = true;
                weldLine.SetActive(true);
                flameCube.enabled = true;
                break;
            case 2:
                weldPointObj.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(2).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(3).GetComponent<MeshRenderer>().enabled = false;
                weldPointObj.transform.GetChild(4).GetComponent<MeshRenderer>().enabled = false;

                readSteps.onClickConfirmbtn();
                readSteps.AddClickConfirmbtnEvent(A_GasWelding.instance.Onclickbtn_s_10_confirm);
                break;
            default:
                break;
        }
    }

    #endregion




    #region Step 20: Check the defects on Joints with the help of Chipping Hammer.

    public void Step20_EnableHighlighting(int num)
    {
        //switch (num)
        //{
        //    case 1:
        //        chippingImpression.SetActive(true);
        //        chippingHammer.GetComponent<Outline>().enabled = true;
        //        break;
        //    case 2:
        //        chippingHammer.GetComponent<Outline>().enabled = false;
        //        weldLine.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        //        weldLine.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        //        chippingHammer.GetComponent<WeldingJoint>().enabled = true;
        //        break;
        //    case 3:
        //        SetTextOnCanvas();
        //        break;
        //    default:
        //        break;
        //}
    }

    #endregion

    #region Step 21: Clean the job surface with wire brush and remove distortion.

    public void Step21_EnableHighlighting(int num)
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