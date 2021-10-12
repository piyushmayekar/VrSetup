using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The highlights of both the job positions")]
        List<XRSocketInteractor> jobSockets;

        [SerializeField, Tooltip("The final job plates which will be shown after the plates are placed in the sockets")]
        List<GameObject> finalJobs;

        [Header("Scriber Marking")]
        [SerializeField] List<PiyushUtils.ScriberMarking> scriberMarkings;
        [SerializeField] int scriberIndex= 0;

        //Step 1: Place one job plate at the flat position
        //Step 2: Do the scriber marking
        //Step 3: Place the other job plate at the vertical position

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            jobSockets[0].socketActive = true;
            jobSockets[0].GetComponent<MeshRenderer>().enabled = true;
        }

        public void OnPlatePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                int index = jobSockets.FindIndex(socket => args.interactor.gameObject == socket.gameObject);
                if (args.interactor == jobSockets[index])
                {
                    Destroy(args.interactable.gameObject);
                    jobSockets[index].socketActive = false;
                    jobSockets[index].GetComponent<MeshRenderer>().enabled = false;
                    finalJobs[index].SetActive(true);
                }
                if (index == 0)
                    StartScriberMarking();
                else if (index == 1)
                {
                    OnTaskCompleted();
                }
            }
        }
        public void StartScriberMarking()
        {
            scriberMarkings[scriberIndex].StartMarkingProcess();
            scriberMarkings[scriberIndex].OnMarkingDone += OnScriberMarkingDone;
        }
        
        void OnScriberMarkingDone()
        {
            scriberIndex++;
            if (scriberIndex >= scriberMarkings.Count)
            {
                StartCoroutine(DelayedCalls());
            }
            else
                StartScriberMarking();
        }
        IEnumerator DelayedCalls()
        {
            yield return new WaitForSeconds(.2f);
            GameObject.FindGameObjectWithTag(_Constants.STEEL_RULER_TAG).GetComponent<PositionResetter>().ResetPos();
            yield return new WaitForSeconds(1f);
            jobSockets[1].socketActive = true;
            jobSockets[1].GetComponent<MeshRenderer>().enabled = true;
        }

        [ContextMenu("Hack complete")]
        public void HackCompleteThisTask()
        {
            jobSockets[0].socketActive = false;
            jobSockets[0].GetComponent<MeshRenderer>().enabled = false;
            finalJobs.ForEach(job => job.SetActive(true));
            OnTaskCompleted();
        }
    }
}