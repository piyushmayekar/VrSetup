using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;


namespace TWelding
{
    public class Task_CoverRunWelding : Task
    {
        [SerializeField] GameObject weldingArea;
        [SerializeField] GameObject pointsParent;
        [SerializeField] Transform leftPointingT, rightPointingT, electrodeT;
        [SerializeField, Tooltip("The max angle between the job plate angle vector & the electrode beyond which we will display an error")]
        float maxAngleThreshold = 10f;
        [SerializeField] int weldingRemainingCount = 0, slagRemaining = 0; //Exterior points
        [SerializeField] List<WeldingPoint> weldingPoints;
        [SerializeField] CurrentKnob currentKnob;
        [SerializeField] ElectrodeType requiredElectrodeType = ElectrodeType._4mm;
        [SerializeField] bool electrodePlaced = false, isCurrentSet = false;
        [SerializeField] GameObject weldingGunHighlights;

        WeldingMachine weldingMachine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingMachine = WeldingMachine.Instance;
            weldingMachine.ToggleMachine(false);
            weldingArea.SetActive(false); //To not let the user weld without setting the correct current
            currentKnob.TargetValue = 170f;
            CurrentKnob.OnTargetValueSet += OnKnobTargetValueSet;
            weldingMachine.RequiredElectrodeType = requiredElectrodeType;
            electrodePlaced = weldingMachine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            weldingMachine.OnElectrodePlacedEvent += OnElectrodePlaced;
        }

        void OnElectrodePlaced()
        {
            electrodePlaced = true;
            CheckIfMiniTasksCompleted();
        }

        void OnKnobTargetValueSet()
        {
            isCurrentSet = true;
            CheckIfMiniTasksCompleted();
        }
        void CheckIfMiniTasksCompleted()
        {
            if (isCurrentSet && electrodePlaced) InitializeEverything();
        }
        private void InitializeEverything()
        {
            weldingMachine.OnElectrodePlacedEvent -= OnElectrodePlaced;
            CurrentKnob.OnTargetValueSet -= OnKnobTargetValueSet;
            weldingPoints = new List<WeldingPoint>(
                pointsParent.GetComponentsInChildren<WeldingPoint>());
            if (weldingArea == null || weldingArea.gameObject == null)
            {
                weldingArea = FindObjectOfType<CornerWelding.WeldingArea>(true).gameObject;
            }
            weldingArea.SetActive(true);
            pointsParent.SetActive(true);
            weldingGunHighlights.SetActive(true);
            weldingPoints.ForEach(point =>
            {
                if (point.ShouldShowSlag)
                {
                    point.OnHitWithHammer += () =>
                    {
                        slagRemaining--;
                        CheckIfTaskCompleted();
                    };
                }
                point.OnWeldingDone += () =>
                    {
                        weldingRemainingCount--;
                        weldingGunHighlights.SetActive(false);
                        CheckIfTaskCompleted();
                    };
            });
            weldingRemainingCount = weldingPoints.Count;
            slagRemaining = weldingPoints.Count;

            WeldingPoint.CheckWeldingElectrodeAngle += (isLeft) =>
            {
                float angle = 0f;
                if (isLeft) angle = Vector3.Angle(-electrodeT.forward, leftPointingT.forward);
                else angle = Vector3.Angle(-electrodeT.forward, rightPointingT.forward);
                weldingMachine.ShowErrorIndicator(angle >= maxAngleThreshold, _Constants.ELECTRODE_NOT_AT_CORRECT_ANGLE);
            };
        }

        public void CheckIfTaskCompleted()
        {
            if (weldingRemainingCount <= 0 && slagRemaining <= 0)
            {
                CurrentKnob.OnTargetValueSet -= InitializeEverything;
                weldingMachine.ShowErrorIndicator(false);
                StartCoroutine(SlagChecker());
            }
        }

        IEnumerator SlagChecker()
        {
            while (GameObject.FindGameObjectsWithTag(_Constants.SLAG_TAG).Length > 0)
                yield return new WaitForEndOfFrame();
            OnTaskCompleted();
        }
    }
}