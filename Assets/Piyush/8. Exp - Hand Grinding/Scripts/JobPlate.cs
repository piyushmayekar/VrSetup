using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VWelding;

namespace Grinding
{
    public class JobPlate : MonoBehaviour
    {
        public event UnityAction OnScriberMarkingDone, OnCenterPunchMarkingDone, OnHacksawCuttingDone, OnFilingDone;
        public UnityEvent OnGrindingComplete;

        [Header("Scriber Marking")]
        [SerializeField] List<ScriberMarking> scriberMarkings;
        [SerializeField] int scriberMarkingIndex = 0;

        [Header("Center Punch Marking")]
        [SerializeField] List<CenterPunchMarking> centerPunchMarkings;
        [SerializeField] int cpMarkingIndex = 0;
        [Header("Grinding")]
        [SerializeField] Collider mainCollider;
        [SerializeField] Collider grindingCollider;
        [SerializeField] GameObject elongatedPlate, ogPlate, extraPlate;
        [SerializeField] bool isBeingGrinded = false;
        [SerializeField] float grindSpeed = 1f;
        [SerializeField] Vector3 finalScale;
        [SerializeField, Tooltip("1: rough grinding done, 2: surface grinding done")]
        int grindingStage = 0;
        [SerializeField] int maxGrindingStage = 2;
        [SerializeField] Vector3 _localScale;


        public static List<JobPlate> jobPlates = new List<JobPlate>();
        void Awake()
        {
            if (!jobPlates.Contains(this))
                jobPlates.Add(this);
        }

        //SCRIBER MARKING
        public void StartScriberMarking()
        {
            scriberMarkings[scriberMarkingIndex].StartMarkingProcess();
            scriberMarkings[scriberMarkingIndex].OnMarkingDone += ScriberMarkingStep;
        }

        void ScriberMarkingStep()
        {
            scriberMarkings[scriberMarkingIndex].OnMarkingDone -= ScriberMarkingStep;
            scriberMarkingIndex++;
            if (scriberMarkingIndex < scriberMarkings.Count)
            {
                scriberMarkings[scriberMarkingIndex].StartMarkingProcess();
                scriberMarkings[scriberMarkingIndex].OnMarkingDone += ScriberMarkingStep;
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

        //Grinding
        public void StartGrinding()
        {
            if (grindingStage >= maxGrindingStage) return;
            Invoke(nameof(TurnOffMainCollider), 1f);
            elongatedPlate.SetActive(false);
            ogPlate.SetActive(true);
            extraPlate.SetActive(true);
            grindingCollider.enabled = true;
            isBeingGrinded = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!isBeingGrinded || grindingStage >= maxGrindingStage) return;
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel && wheel.IsWheelSpinning && grindingStage == (int)GrindingWheelType.Rough)
            {
                StartCoroutine(_Grinder());
            }
        }

        void OnTriggerExit(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel && wheel.IsWheelSpinning)
            {
                StopAllCoroutines();
            }
        }

        IEnumerator _Grinder()
        {
            _localScale = extraPlate.transform.localScale;
            while (_localScale != finalScale)
            {
                _localScale = extraPlate.transform.localScale;
                _localScale = Vector3.MoveTowards(_localScale, finalScale, grindSpeed * Time.deltaTime);
                extraPlate.transform.localScale = _localScale;
                yield return new WaitForEndOfFrame();
            }
            if (_localScale == finalScale) //Grinding Complete
            {
                Destroy(extraPlate);
                mainCollider.enabled = true;
                grindingCollider.enabled = false;
                grindingStage = 1;
                StopAllCoroutines();
                centerPunchMarkings.ForEach(centerPunchMarking => centerPunchMarking.gameObject.SetActive(false));
                scriberMarkings.ForEach(scriberMarking => scriberMarking.gameObject.SetActive(false));
                OnGrindingComplete?.Invoke();
            }
        }

        /// <summary>
        /// For not letting user grab the attached plate to mechanical vise before the grinding is done
        /// </summary>
        void TurnOffMainCollider()
        {
            mainCollider.enabled = false;
        }
    }
}

