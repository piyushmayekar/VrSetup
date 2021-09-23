using PiyushUtils;
using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.Events;
using VWelding;

namespace Semester2
{

    public class JobPlate : MonoBehaviour
    {

        public event UnityAction OnScriberMarkingDone, OnCenterPunchMarkingDone, OnGasCuttingDone, OnFilingDone;

        [SerializeField] PlateType plateType;

        [Header("Scriber Marking")]
        [SerializeField] List<PiyushUtils.ScriberMarking> scriberMarkings;
        [SerializeField] int scriberMarkingIndex = 0;

        [Header("Center Punch Marking")]
        [SerializeField] List<CenterPunchMarking> centerPunchMarkings;
        [SerializeField] int cpMarkingIndex = 0;

        [Header("Cutting")]
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

        public static List<JobPlate> jobPlates = new List<JobPlate>();
        void Awake()
        {
            if (jobPlates == null)
                jobPlates = new List<JobPlate>();
            if (gameObject.activeSelf)
                if (jobPlates.Count == 0 || !jobPlates.Contains(this))
                    jobPlates.Add(this);
            _rb = GetComponent<Rigidbody>();
        }

        //SCRIBER MARKING
        public void StartScriberMarking()
        {
            scriberMarkings[scriberMarkingIndex].StartMarkingProcess();
            scriberMarkings[scriberMarkingIndex].OnMarkingDone += (ScriberMarkingStep);
        }

        void ScriberMarkingStep()
        {
            scriberMarkings[scriberMarkingIndex].OnMarkingDone -= ScriberMarkingStep;
            scriberMarkingIndex++;
            if (scriberMarkingIndex == 1 && scriberMarkings.Count > 1)
                FindObjectOfType<PlateMarkingSocket>().ToggleAttentionGrab();
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
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
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
            if (cpMarkingIndex == 1 && centerPunchMarkings.Count > 1)
                FindObjectOfType<PlateMarkingSocket>().ToggleAttentionGrab();
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
            ogPlate.gameObject.SetActive(true);
            extraPlate.gameObject.SetActive(true);
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
    
    [System.Serializable]
    public enum PlateType
    {
        Plate_45, Plate_100
    }
}