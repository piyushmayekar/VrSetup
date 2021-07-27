using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.Events;
using VWelding;

namespace PiyushUtils
{
    public class SMAWJobPlate : MonoBehaviour
    {
        public event UnityAction OnScriberMarkingDone, OnCenterPunchMarkingDone, OnGasCuttingDone, OnFilingDone;

        [SerializeField] PlateType plateType;

        [Header("Scriber Marking")]
        [SerializeField] List<PiyushUtils.ScriberMarking> scriberMarkings;
        [SerializeField] int scriberMarkingIndex = -1;

        [Header("Center Punch Marking")]
        [SerializeField] List<CenterPunchMarking> centerPunchMarkings;
        [SerializeField] int cpMarkingIndex = 0;

        [Header("Hacksaw cutting")]
        [SerializeField] GameObject centerPunchTaskParent;
        [SerializeField] GameObject scriberTaskParent;
        [SerializeField] GameObject elongatedPlate, ogPlate, extraPlate;
        [SerializeField] CuttingArea cuttingArea;
        [SerializeField] bool isCuttingDone = false, isFilingDone = false;
        [SerializeField] GameObject roughEdge;
        [SerializeField] List<FilingPoint> filingPoints;
        Rigidbody _rb;

        public bool IsCuttingDone { get => isCuttingDone; set => isCuttingDone = value; }
        public bool IsFilingDone { get => isFilingDone; set => isFilingDone = value; }
        public PlateType PlateType { get => plateType; set => plateType = value; }
        public int ScriberMarkingIndex { get => scriberMarkingIndex; set => scriberMarkingIndex = value; }

        public static List<SMAWJobPlate> jobPlates = new List<SMAWJobPlate>();
        void Awake()
        {
            if (jobPlates == null)
                jobPlates = new List<SMAWJobPlate>();
            if (gameObject.activeSelf)
                if (jobPlates.Count == 0 || !jobPlates.Contains(this))
                    jobPlates.Add(this);
            _rb = GetComponent<Rigidbody>();
        }

        //SCRIBER MARKING
        public void StartScriberMarking()
        {
            if (scriberMarkings[scriberMarkingIndex] != null)
            {
                scriberMarkings[scriberMarkingIndex].StartMarkingProcess();
                scriberMarkings[scriberMarkingIndex].OnMarkingDone += (ScriberMarkingStep);
            }
            else
            {
                Debug.Log("scriberMarkings found null");
            }
        }

        void ScriberMarkingStep()
        {
            scriberMarkings[scriberMarkingIndex].OnMarkingDone -= ScriberMarkingStep;
            scriberMarkingIndex++;
            if (scriberMarkingIndex < scriberMarkings.Count)
            {
                StartScriberMarking();
            }
            else
            {
                OnScriberMarkingDone?.Invoke();
                ToggleFreezePlate(false);
            }
        }

        public void ToggleFreezePlate(bool freeze = true)
        {
            _rb.constraints = freeze ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
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
            elongatedPlate.gameObject.SetActive(false);
            ogPlate.GetComponent<MeshRenderer>().enabled = true;
            extraPlate.GetComponent<MeshRenderer>().enabled = true;
            cuttingArea = GetComponentInChildren<CuttingArea>(true);
            cuttingArea.gameObject.SetActive(true);
            cuttingArea.OnCuttingComplete.AddListener(CuttingDone);
        }


        public void SetupForFiling()
        {
            Invoke(nameof(DisableMainCollider), 1f);
            filingPoints.ForEach(point => {
                point.GetComponent<Collider>().enabled = true;
                point.OnFilingDone += OnFilingDoneAtPoint;
            });
        }

        public void DisableMainCollider() => GetComponent<Collider>().enabled = false;

        public void OnMechanicalViseSocketExit()
        {
            GetComponent<Collider>().enabled = true;
        }

        public void CuttingDone()
        {
            IsCuttingDone = true;
            cuttingArea.isCuttingDone = true;
            elongatedPlate.SetActive(false);
            ogPlate.GetComponent<Collider>().enabled = true;
            GetComponent<Collider>().enabled = true;
            extraPlate.transform.parent = null;
            extraPlate.GetComponent<Collider>().enabled = true;
            Rigidbody extraPlateRB = extraPlate.AddComponent<Rigidbody>();
            extraPlateRB.useGravity = true;
            extraPlateRB.isKinematic = false;
            scriberTaskParent?.SetActive(false);
            centerPunchTaskParent?.SetActive(false);
            OnGasCuttingDone?.Invoke();

            //Rough edge
            roughEdge.SetActive(true);
            filingPoints.ForEach(point => point.GetComponent<Collider>().enabled = false);
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
        private void OnDestroy()
        {
            jobPlates.Remove(this);
        }
    }
}