using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatWelding;
using UnityEngine.Events;

namespace CornerWelding
{
    public class Task_WeavingRunWelding : Task
    {

        [SerializeField] GameObject weldingArea, machineAnimation;
        [SerializeField] int weldingDoneOnPoints = 0, slagRemaining = 0; //Exterior points
        [SerializeField] Transform pointsParent;
        List<WeldingPoint> allPoints;
        [SerializeField] CurrentKnob knob;
        [SerializeField] float targetCurrentValue = 116f;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] bool currentSet = false, electrodePlaced = false;
        [SerializeField] GameObject weavingImage;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            allPoints = new List<WeldingPoint>(pointsParent.
            GetComponentsInChildren<WeldingPoint>());

            machine = WeldingMachine.Instance;
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
            if (weldingArea == null || weldingArea.gameObject == null)
            {
                weldingArea = FindObjectOfType<CornerWelding.WeldingArea>(true).gameObject;
            }
            weldingArea.SetActive(true);
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
            OnTaskCompleted();
        }
    }
}