using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BenchWise : MonoBehaviour
{
    public ExperimentManager taskManager;

    #region Job Var

    public Job job;
    public bool hasJob;
    public bool jobFitted;
    public GameObject cleanJob;
    #endregion

    #region TaskVariable
    public bool SecondFilingDone;
    public bool filing1, filing2, HackSawcutting;
    public bool firstCutDone;
    public bool secondCutDone;

    public bool showToolHighLight = true;

    public GameObject highlightObj1, highlightObj2, highlightObj3;
    public GameObject fileHighlight1, fileHighlight2, hackSawHighlight;
    public GameObject SnapZone1, SnapZone2, SnapZone3;
    public GameObject Cut1, Cut2, line1, line2;


    #endregion

    

    #region MachineVariables
    public Transform Slider;
    public Transform Handle;

    public bool HasControl;
    public bool canMoveForward = true;

    public float movingSpeed;
    public float rotSpeed;
    public bool moveForward;
    public bool moveBackward;
    public float maxPos = -0.1829f;
    public float minPos = -0.0459f;
    #endregion

    void Update()
    {
        if (!hasJob)
        {
            if (!jobFitted)
            {
                //HintText.text = "Use Left Hand To Move Wise";
            }

        }

        if (jobFitted)
        {
            moveForward = false;
            // HintText.text = "Grab HackSaw and start cutting";
            ManageTaskCheck();

        }

        AnimateWise();
    }


    private void ManageTaskCheck()
    {
        if (filing2 && !SecondFilingDone)
        {
            //HintText.text = "Grab Flat File for Filing as shown";
            ShowFlatFileHighlight2(showToolHighLight);
            job = cleanJob.GetComponent<Job>();
            job.benchWise = this;
        }
        if (SecondFilingDone && taskManager.currentTaskIndex == 2)
        {
            // HintText.text = "Filing Done Remove the Job";

        }

        if (!firstCutDone && taskManager.currentTaskIndex == 4)
        {
            if (Job.firstCutDone)
            {
                // Debug.Log("FirstCutDone");
                Cut1.transform.parent = null;
                Cut1.AddComponent<Rigidbody>();
                Cut1.GetComponent<Rigidbody>().drag = 25;
                Cut1.AddComponent<XRGrabInteractable>();

                line2.SetActive(true);
                job.SetScaleForSecondCut(0.71f);
                Cut2.SetActive(true);
                firstCutDone = true;

                TaskBoard.ins.ToggleButtonColor(true);
                TaskBoard.ins.ProceedButton.enabled = true;

            }
        }

        if (firstCutDone && !secondCutDone && taskManager.currentTaskIndex == 4)
        {
            if (Job.SecondCutDone)
            {
                Cut2.transform.parent = null;
                Cut2.AddComponent<Rigidbody>();
                Cut2.GetComponent<Rigidbody>().drag = 25;
                Cut2.AddComponent<XRGrabInteractable>();
                TaskBoard.ins.ProceedButton.enabled = true;
                TaskBoard.ins.ToggleButtonColor(true);
                secondCutDone = true;
            }
        }
    }

    public void SetJob(Job _job)
    {
        job = _job;
    }
    #region MachineWorking Methods
    public void MoveWiseForward(float speed, float rotSpeed)
    {
        MoveWise(-speed, rotSpeed, true);
    }

    public void MoveWiseBackWard(float speed, float rotSpeed)
    {

        MoveWise(speed, rotSpeed, false);
    }

    public void MoveWise(float speed, float rotSpeed, bool forward)
    {
        Vector3 pos = Slider.localPosition;
        pos.z += Time.deltaTime * speed;
        pos.z = Mathf.Clamp(pos.z, maxPos, minPos);
        Slider.localPosition = pos;

        RotateHandle(rotSpeed, pos.z, forward);
    }

    public void RotateHandle(float rotSpeed, float pos, bool forward)
    {
        if (forward)
        {
            if (pos <= maxPos)
            {
                return;
            }
            else
            {
                Vector3 rot = Handle.localEulerAngles;
                rot.z += Time.deltaTime * rotSpeed;
                Handle.localEulerAngles = rot;

            }
        }
        else if (!forward)
        {
            if (pos >= minPos)
            {
                return;
            }
            else
            {
                Vector3 rot = Handle.localEulerAngles;
                rot.z -= Time.deltaTime * rotSpeed;
                Handle.localEulerAngles = rot;

            }
        }


    }

    private void AnimateWise()
    {
        if (HasControl)
        {
           // PlayAudio.ins.PlayBenchWiseSound();
            if (moveForward && !moveBackward)
            {
                MoveWiseForward(movingSpeed, rotSpeed);
            }

            if (moveBackward && !moveForward)
            {
                MoveWiseBackWard(movingSpeed, rotSpeed);
            }
        }
        else
        {
            moveForward = false;
            moveBackward = false;

        }


    }
    #endregion

    #region SnapPosMethods
    public void ShowJobSnapPos1()
    {
        SnapZone1.SetActive(true);
    }

    public void ShowJobSnapPos2()
    {
        SnapZone2.SetActive(true);
    }

    public void ShowJobSnapForHackSaw()
    {
        //check if repeat code is here  task 3 acive the zone 3
        SnapZone3.SetActive(true);
        SnapZone2.SetActive(false);
    }

    #endregion


    #region ShowHighlightMethods
    private void ShowFlatFileHighlight1(bool show)
    {
        fileHighlight1.SetActive(show);
    }

    private void ShowFlatFileHighlight2(bool show)
    {
        fileHighlight2.SetActive(show);
    }

    private void ShowHackSawHighlight(bool show)
    {
        //Show the hackSaw highligh for cut
        Debug.Log("HackSaw highligh on fit");
    }

    #endregion


    #region OnJobSnapMethods
    public void OnFirstJobSnap()
    {
        highlightObj1.SetActive(false);
        // HintText.text = "Grab File and do the filing";
    }

    public void OnSecondJobSnap()
    {
        highlightObj2.SetActive(false);
        filing2 = true;
        TaskBoard.ins.ProceedButton.enabled = true;
        TaskBoard.ins.ToggleButtonColor(true);

    }

    public void OnThirdJobSnap()
    {
        highlightObj3.SetActive(false);
        HackSawcutting = true;

        TaskBoard.ins.ToggleButtonColor(true);
        TaskBoard.ins.ProceedButton.enabled = true;

        if (job)
        {
            job.PrepareJobForFirstCut();
        }
        Cut1.SetActive(true);
        if (line1)
        {
            line1.SetActive(true);
        }
        if (line2)
        {
            line2.SetActive(true);
        }

    }

    #endregion


    #region EnablingJob
    public void EnableHasJob()
    {
        hasJob = true;
    }

    public void DisableHasJob()
    {
        hasJob = false;
    }

    #endregion

}
