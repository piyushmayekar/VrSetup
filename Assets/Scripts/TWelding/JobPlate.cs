using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class JobPlate : MonoBehaviour
    {
        public event Action OnScriberMarkingDone, OnCenterPunchMarkingDone, OnHacksawCuttingDone;
        public static List<JobPlate> jobPlates = new List<JobPlate>();

        [Header("Scriber Marking")]
        [SerializeField] List<GameObject> markingPoints;
        [SerializeField] LineRenderer markingLine;
        [SerializeField] int currentMarking = 0;
        [SerializeField] List<GameObject> scriberHighlights;
        int totalLineMarkingPoints = 0;
        int lineMarkingPoint = 0;
        public int LineMarkingPoint { get => lineMarkingPoint; set => lineMarkingPoint = value; }

        [Header("Center Punch Marking")]
        [SerializeField] List<PunchMarkingPoint> centerPunchMarkingPoints;
        [SerializeField] int currentCPMarkingPointIndex = 0;
        PunchMarkingPoint CurrentMarkingPoint => centerPunchMarkingPoints[currentCPMarkingPointIndex];

        [Header("Hacksaw cutting")]
        [SerializeField] GameObject centerPunchTaskParent;
        [SerializeField] GameObject scriberTaskParent;
        [SerializeField] GameObject elongatedPlate, ogPlate, extraPlate;
        [SerializeField] Rigidbody extraPlateRB;
        [SerializeField] List<GameObject> cuttingPoints;
        [SerializeField] int currentCuttingPoints = 0;
        [SerializeField] bool isCuttingDone = false;
        public bool IsCuttingDone { get => isCuttingDone; set => isCuttingDone = value; }

        void Awake()
        {
            if (!jobPlates.Contains(this))
                jobPlates.Add(this);
        }

        //SCRIBER MARKING
        public void StartScriberMarking()
        {
            TurnOnMarkingPoint();
            totalLineMarkingPoints = markingLine.transform.childCount;
        }

        void TurnOnMarkingPoint()
        {
            markingPoints[currentMarking].SetActive(true);
        }

        internal void OnMarkingDone()
        {
            if (currentMarking < markingPoints.Count)
            {
                currentMarking++;
                TurnOnMarkingPoint();
            }
        }

        internal void OnMarkingDone(Vector3 position)
        {
            lineMarkingPoint++;
            markingLine.positionCount++;
            markingLine.SetPosition(markingLine.positionCount - 1, position);
            if (lineMarkingPoint >= totalLineMarkingPoints)
                OnScriberMarkingDone?.Invoke();
            scriberHighlights[1].SetActive(false);
        }

        //CENTER PUNCH
        public void StartCenterPunchMarking()
        {
            CurrentMarkingPoint.gameObject.SetActive(true);
            CenterPunch.OnHammerHit += OnHammerHit;
        }

        private void OnHammerHit()
        {
            if (CurrentMarkingPoint.IsCenterPunchInside)
            {
                CurrentMarkingPoint.MarkingDone();
                currentCPMarkingPointIndex++;
                if (currentCPMarkingPointIndex < centerPunchMarkingPoints.Count)
                    CurrentMarkingPoint.gameObject.SetActive(true);
                else
                {
                    CenterPunch.OnHammerHit -= OnHammerHit;
                    OnCenterPunchMarkingDone?.Invoke();
                }
            }
        }

        //HACKSAW CUTTING
        public void SetupForCutting()
        {
            elongatedPlate.GetComponent<Collider>().enabled = false;
            cuttingPoints[0].transform.parent.gameObject.SetActive(true);
        }

        internal void CuttingDone()
        {
            currentCuttingPoints++;
            if (currentCuttingPoints >= cuttingPoints.Count) //Cutting Complete
            {
                IsCuttingDone = true;
                elongatedPlate.SetActive(false);
                ogPlate.GetComponent<MeshRenderer>().enabled = true;
                extraPlate.GetComponent<MeshRenderer>().enabled = true;
                ogPlate.GetComponent<Collider>().enabled = true;
                extraPlate.transform.parent = null;
                extraPlate.GetComponent<Collider>().enabled = true;
                extraPlateRB.useGravity = true;
                extraPlateRB.isKinematic = false;
                scriberTaskParent.SetActive(false);
                centerPunchTaskParent.SetActive(false);
                OnHacksawCuttingDone?.Invoke();
            }
        }
    }
}