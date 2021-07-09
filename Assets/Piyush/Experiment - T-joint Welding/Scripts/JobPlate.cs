using System;
using System.Collections;
using System.Collections.Generic;
using VWelding;
using UnityEngine;
using UnityEngine.Events;

namespace TWelding
{
    public class JobPlate : MonoBehaviour
    {
        public event UnityAction OnScriberMarkingDone, OnCenterPunchMarkingDone, OnHacksawCuttingDone, OnFilingDone;

        [SerializeField] PlateType plateType;

        [Header("Scriber Marking")]
        [SerializeField] List<ScriberMarking> scriberMarkings;
        [SerializeField] int scriberMarkingIndex = -1;

        [Header("Center Punch Marking")]
        [SerializeField] List<CenterPunchMarking> centerPunchMarkings;
        [SerializeField] int cpMarkingIndex = 0;

        [Header("Hacksaw cutting")]
        [SerializeField] GameObject centerPunchTaskParent;
        [SerializeField] GameObject scriberTaskParent;
        [SerializeField] GameObject elongatedPlate, ogPlate, extraPlate;
        [SerializeField] List<CuttingPoint> cuttingPoints;
        [SerializeField] int currentCuttingPoints = 0;
        [SerializeField] bool isCuttingDone = false, isFilingDone = false;
        [SerializeField] GameObject roughEdge;
        [SerializeField] List<FilingPoint> filingPoints;

        public bool IsCuttingDone { get => isCuttingDone; set => isCuttingDone = value; }
        public bool IsFilingDone { get => isFilingDone; set => isFilingDone = value; }
        public PlateType PlateType { get => plateType; set => plateType = value; }
        public int ScriberMarkingIndex { get => scriberMarkingIndex; set => scriberMarkingIndex = value; }

        public static List<JobPlate> jobPlates = new List<JobPlate>();
        void Awake()
        {
            if (gameObject.activeSelf)
                if (!jobPlates.Contains(this))
                    jobPlates.Add(this);

        }

        internal void ScriberMarkingStep()
        {
            Debug.Log(nameof(ScriberMarkingStep) + ": " + ScriberMarkingIndex);
            if (ScriberMarkingIndex >= 0 && ScriberMarkingIndex < scriberMarkings.Count)
                scriberMarkings[ScriberMarkingIndex].OnMarkingDone -= ScriberMarkingStep;
            ScriberMarkingIndex++;
            if (ScriberMarkingIndex < scriberMarkings.Count)
            {
                scriberMarkings[ScriberMarkingIndex].StartMarkingProcess();
                scriberMarkings[ScriberMarkingIndex].OnMarkingDone += ScriberMarkingStep;
            }
            else
                OnScriberMarkingDone?.Invoke();
        }

        //CENTER PUNCH
        public void StartCenterPunchMarking()
        {
            centerPunchMarkings[cpMarkingIndex].StartMarkingProcess();
            centerPunchMarkings[cpMarkingIndex].OnMarkingDone += CenterPunchStep;
        }

        void CenterPunchStep()
        {
            centerPunchMarkings[cpMarkingIndex].OnMarkingDone -= CenterPunchStep;
            cpMarkingIndex++;
            if (cpMarkingIndex < centerPunchMarkings.Count)
            {
                centerPunchMarkings[cpMarkingIndex].StartMarkingProcess();
                centerPunchMarkings[cpMarkingIndex].OnMarkingDone += CenterPunchStep;
            }
            else
                OnCenterPunchMarkingDone?.Invoke();
        }


        //HACKSAW CUTTING
        public void SetupForCutting()
        {
            Invoke(nameof(DisableMainCollider), 1f);
            elongatedPlate.GetComponent<Collider>().enabled = false;
            cuttingPoints[0].transform.parent.gameObject.SetActive(true);
            cuttingPoints.ForEach(point => point.OnCuttingDone += CuttingDone);
        }

        public void DisableMainCollider() => GetComponent<Collider>().enabled = false;

        public void OnMechanicalViseSocketExit()
        {
            GetComponent<Collider>().enabled = true;
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
                Rigidbody extraPlateRB = extraPlate.AddComponent<Rigidbody>();
                extraPlateRB.useGravity = true;
                extraPlateRB.isKinematic = false;
                scriberTaskParent?.SetActive(false);
                centerPunchTaskParent?.SetActive(false);
                OnHacksawCuttingDone?.Invoke();

                //Rough edge
                roughEdge.SetActive(true);
                filingPoints.ForEach(point => point.OnFilingDone += OnFilingDoneAtPoint);
            }
        }

        internal void OnFilingDoneAtPoint(FilingPoint point)
        {
            if (filingPoints.Contains(point)) filingPoints.Remove(point);
            if (filingPoints.Count == 0)
            {
                IsFilingDone = true;
                roughEdge.SetActive(false);
                GetComponent<Collider>().enabled = true;
                OnFilingDone?.Invoke();
            }
        }
    }
}

