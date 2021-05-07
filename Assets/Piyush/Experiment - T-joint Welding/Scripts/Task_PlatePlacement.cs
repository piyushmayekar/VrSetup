using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The highlights of both the job positions")]
        List<XRSocketInteractor> jobSockets;

        [SerializeField, Tooltip("The final job plates which will be shown after the plates are placed in the sockets")]
        List<GameObject> finalJobs;

        [SerializeField] GameObject doneButton;

        [Header("Scriber Marking")]
        [SerializeField] List<GameObject> markingPoints;
        [SerializeField] LineRenderer markingLine;
        [SerializeField] int currentMarking = 0;
        [SerializeField] List<GameObject> scriberHighlights;
        [SerializeField] List<MarkingLinePoint> markingLinePoints;
        int markingLinePointIndex = 0;
        public int LineMarkingPoint { get => markingLinePointIndex; set => markingLinePointIndex = value; }

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
                else if (index == JobPlate.jobPlates.Count - 1)
                    doneButton.SetActive(true);
            }
        }

        public void StartScriberMarking()
        {
            TurnOnMarkingPoint();
        }

        void TurnOnMarkingPoint()
        {
            markingPoints[currentMarking].SetActive(true);
            if (markingPoints[currentMarking].GetComponentInChildren<MarkingPoint>())
                markingPoints[currentMarking].GetComponentInChildren<MarkingPoint>().OnMarkingDone += OnMarkingDone;
        }

        internal void OnMarkingDone()
        {
            currentMarking++;
            if (currentMarking < markingPoints.Count)
            {
                TurnOnMarkingPoint();
                if (markingPoints[currentMarking].GetComponentInChildren<MarkingPoint>())
                    markingPoints[currentMarking].GetComponentInChildren<MarkingPoint>().OnMarkingDone += OnMarkingDone;
            }
            if (currentMarking == markingPoints.Count - 1)
            {
                markingLinePoints = new List<MarkingLinePoint>(markingLine.transform.GetComponentsInChildren<MarkingLinePoint>());
                markingLinePoints.ForEach(point => point.OnScriberTipEnter += OnMarkingPointScriberEnter);
            }
        }

        internal void OnMarkingPointScriberEnter(int index, Vector3 position)
        {
            if (index == LineMarkingPoint)
            {
                markingLinePoints[index].gameObject.SetActive(false);
                OnMarkingDone(position);
            }
        }

        internal void OnMarkingDone(Vector3 position)
        {
            markingLinePointIndex++;
            markingLine.positionCount++;
            markingLine.SetPosition(markingLine.positionCount - 1, position);
            if (markingLinePointIndex >= markingLinePoints.Count)
            {
                jobSockets[1].socketActive = true;
                jobSockets[1].GetComponent<MeshRenderer>().enabled = true;
            }
            scriberHighlights.ForEach(highlight => highlight.SetActive(false));
        }

        public override void OnTaskCompleted()
        {
            doneButton.SetActive(false);
            base.OnTaskCompleted();
        }
    }
}