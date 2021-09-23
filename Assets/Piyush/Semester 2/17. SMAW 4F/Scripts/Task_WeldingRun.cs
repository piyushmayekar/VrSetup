using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CornerWelding;

namespace Semester2
{
    public class Task_WeldingRun : Task
    {

        [SerializeField] GameObject weldingArea, machineAnimation;
        [SerializeField] int weldingDoneOnPoints = 0, slagRemaining = 0; //Exterior points
        [SerializeField] Transform pointsParent;
        List<WeldingPoint> allPoints;
        [SerializeField] FlatWelding.CurrentKnob knob;
        [SerializeField] float targetCurrentValue = 116f;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] bool currentSet = false, electrodePlaced = false;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            
            //Getting all points
            allPoints = new List<WeldingPoint>(pointsParent.
            GetComponentsInChildren<WeldingPoint>());
            
            //If welding machine does not have the required electrode attached, wait for it to be attached
            machine = WeldingMachine.Instance;
            machine.RequiredElectrodeType = requiredElectrodeType;
            electrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            if (!electrodePlaced) machine.OnElectrodePlacedEvent += OnElectrodePlaced;
            
            //If current as per required, wait for it to be set
            knob.TargetValue = targetCurrentValue;
            if (knob.CurrentValue != targetCurrentValue)
                FlatWelding.CurrentKnob.OnTargetValueSet += OnKnobTargetValueSet;
            else
                currentSet = true;

            //If all conditions are met OnTaskBegin, start the welding
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

        /// <summary>
        /// Check if the current & electrode is appropriate
        /// </summary>
        void CheckIfMiniTasksCompleted()
        {
            if (currentSet && electrodePlaced) InitializeEverything();
        }

        private void InitializeEverything()
        {
            machine.OnElectrodePlacedEvent -= OnElectrodePlaced;
            FlatWelding.CurrentKnob.OnTargetValueSet -= OnKnobTargetValueSet;
            if (weldingArea == null || weldingArea.gameObject == null)
            {
                weldingArea = FindObjectOfType<CornerWelding.WeldingArea>(true).gameObject;
            }
            weldingArea.SetActive(true);
            allPoints[0].transform.parent.gameObject.SetActive(true);
            if (machineAnimation != null)
                machineAnimation.SetActive(true);
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
                point.OnWeldingDone += (point) =>
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
            if (machineAnimation != null)
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
            weldingArea.SetActive(false);
            OnTaskCompleted();
        }

        [ContextMenu("Reduce Welding Points Timer")]
        public void ReduceWeldingTimer()
        {
            List<WeldingPoint> weldingPoints = new List<WeldingPoint>(FindObjectsOfType<WeldingPoint>(true));
            weldingPoints.ForEach(point => point.WeldingTimer = 0.1f);
        }
    }
}