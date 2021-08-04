using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatWelding;
using CornerWelding;
using UnityEngine.Events;

namespace VWelding
{
    public class Task_WeavingRunWelding : Task
    {
        [SerializeField] UnityEvent TurnOnJobGrabAfterTaskEnd;
        [SerializeField] CornerWelding.WeldingArea weldingArea;
        [SerializeField] GameObject machineAnimation;
        [SerializeField] int weldingDoneOnPoints = 0, slagRemaining = 0; //Exterior points
        [SerializeField] Transform pointsParent, weldingTipT;
        List<CornerWelding.WeldingPoint> allPoints;
        [SerializeField] CurrentKnob knob;
        [SerializeField] float targetCurrentValue = 116f;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] bool currentSet = false, electrodePlaced = false;
        [SerializeField] bool turnOnJobGrabAfterJobComplete = false;
        [SerializeField] GameObject weavingImage;
        [SerializeField] Vector2 weldingAngleLimits = new Vector2(95f, 115f);
        CornerWelding.WeldingMachine machine;
        bool isWeldingTipInsideWeldingArea = false;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            allPoints = new List<CornerWelding.WeldingPoint>(pointsParent.
            GetComponentsInChildren<CornerWelding.WeldingPoint>());

            machine = CornerWelding.WeldingMachine.Instance;
            machine.RequiredElectrodeType = requiredElectrodeType;
            electrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            if (!electrodePlaced) machine.OnElectrodePlacedEvent += OnElectrodePlaced;
            knob.TargetValue = targetCurrentValue;
            if (knob.CurrentValue != targetCurrentValue)
                CurrentKnob.OnTargetValueSet += OnKnobTargetValueSet;
            else
                currentSet = true;
            CheckIfMiniTasksCompleted();
        }

        void OnElectrodePlaced()
        {
            electrodePlaced = true;
            CheckIfMiniTasksCompleted();
        }

        void OnKnobTargetValueSet()
        {
            currentSet = true; 
            CheckIfMiniTasksCompleted();
        }

        void CheckIfMiniTasksCompleted()
        {
            if (currentSet && electrodePlaced) InitializeEverything();
        }

        private void InitializeEverything()
        {
            machine.OnElectrodePlacedEvent -= OnElectrodePlaced;
            CurrentKnob.OnTargetValueSet -= OnKnobTargetValueSet;
            if (weldingArea == null || weldingArea.gameObject==null)
            {
                weldingArea = FindObjectOfType<CornerWelding.WeldingArea>(true);
            }
            weldingArea.gameObject.SetActive(true);
            weldingArea.OnWeldingMachineTipEnter.AddListener(new UnityAction(StartWeldingMachineAngleCheck));
            weldingArea.OnWeldingMachineTipExit.AddListener(new UnityAction(StopWeldingMachineAngleCheck));
            allPoints[0].transform.parent.gameObject.SetActive(true);
            machineAnimation.SetActive(true);
            weavingImage.SetActive(true);
            allPoints.ForEach(point =>
            {
                if (point.ShouldShowSlag)
                {
                    point.OnHitWithHammer += () =>
                    {
                        slagRemaining--;
                        CheckIfTaskCompleted();
                    };
                }
                point.OnWeldingDone +=
                (point) =>
                {
                    int sibIndex = point.transform.GetSiblingIndex();
                    weldingDoneOnPoints++;
                    // machine.ShowErrorIndicator(sibIndex != weldingDoneOnPoints, "Welding is not being done in correct manner");
                    CheckIfTaskCompleted();
                };
            });
            slagRemaining = allPoints.Count;
        }

        public void CheckIfTaskCompleted()
        {
            machineAnimation.SetActive(false);
            if (weldingDoneOnPoints == allPoints.Count && slagRemaining <= 0)
            {
                machine.ShowErrorIndicator(false);
                StartCoroutine(SlagChecker());
            }
        }

        IEnumerator SlagChecker()
        {
            while (GameObject.FindGameObjectsWithTag(_Constants.SLAG_TAG).Length > 0)
                yield return new WaitForEndOfFrame();
            StopAllCoroutines();
            if (turnOnJobGrabAfterJobComplete) TurnOnJobGrabAfterTaskEnd?.Invoke();
            OnTaskCompleted();
        }

        //Check Welding Machine Angle While Welding
        public void StartWeldingMachineAngleCheck()
        {
            isWeldingTipInsideWeldingArea = true;
            StartCoroutine(CheckWeldingAngle());
        }
        public void StopWeldingMachineAngleCheck()
        {
            isWeldingTipInsideWeldingArea = false;
        }

        IEnumerator CheckWeldingAngle()
        {
            while (isWeldingTipInsideWeldingArea)
            {
                float angle = Vector3.Angle(weldingTipT.up, pointsParent.forward);
                machine.ShowErrorIndicator(angle <= weldingAngleLimits.x || angle >= weldingAngleLimits.y,
                _Constants.ELECTRODE_NOT_AT_CORRECT_ANGLE);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}