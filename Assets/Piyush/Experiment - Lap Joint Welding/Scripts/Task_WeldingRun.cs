using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CornerWelding;
using UnityEngine.Events;
using WeldingPoint = CornerWelding.WeldingPoint;
using WeldingMachine = CornerWelding.WeldingMachine;

namespace LapWelding
{
    public class Task_WeldingRun : Task
    {
        [SerializeField] GameObject gunHighlight;
        [SerializeField] GameObject weldingArea;
        [SerializeField] int parentCounter = 0, pointCounter = 0, slagRemaining = 0; //Exterior points
        [SerializeField] List<Transform> pointsParents;
        [SerializeField] List<WeldingPoint> points;
        [SerializeField] FlatWelding.CurrentKnob knob;
        [SerializeField] float targetCurrentValue = 116f;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] bool currentSet = false, electrodePlaced = false;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            parentCounter = 0;
            pointCounter = 0;
            machine = WeldingMachine.Instance;
            machine.RequiredElectrodeType = requiredElectrodeType;
            electrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            if (!electrodePlaced) machine.OnElectrodePlacedEvent += OnElectrodePlacedEvent;
            knob.TargetValue = targetCurrentValue;
            if (knob.CurrentValue != targetCurrentValue)
                FlatWelding.CurrentKnob.OnTargetValueSet += OnCurrentTargetValueSet;
            else
                currentSet = true;
            CheckIfMiniTasksCompleted();
        }

        void OnElectrodePlacedEvent()
        {
            electrodePlaced = true;
            CheckIfMiniTasksCompleted();
        }

        void OnCurrentTargetValueSet()
        {
            currentSet = true;
            CheckIfMiniTasksCompleted();
        }

        void CheckIfMiniTasksCompleted()
        {
            if (currentSet && electrodePlaced)
            {
                machine.OnElectrodePlacedEvent -= OnElectrodePlacedEvent;
                FlatWelding.CurrentKnob.OnTargetValueSet -= OnCurrentTargetValueSet;
                InitializeEverything();
            }
        }

        private void InitializeEverything()
        {
            pointsParents[parentCounter].gameObject.SetActive(true);
            points = new List<WeldingPoint>(pointsParents[parentCounter].
            GetComponentsInChildren<WeldingPoint>());
            if (weldingArea == null || weldingArea.gameObject == null)
            {
                weldingArea = FindObjectOfType<WeldingArea>(true).gameObject;
            }
            weldingArea.SetActive(true);
            if (parentCounter == 0)
                gunHighlight.SetActive(true);
            pointCounter = 0;
            points.ForEach(point =>
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
                    pointCounter++;
                    gunHighlight.SetActive(false);
                    CheckIfTaskCompleted();
                };
            });
            slagRemaining = points.Count;
        }

        public void CheckIfTaskCompleted()
        {
            if (pointCounter == points.Count && slagRemaining <= 0)
            {
                StartCoroutine(SlagChecker());
            }
        }

        IEnumerator SlagChecker()
        {
            while (GameObject.FindGameObjectsWithTag(_Constants.SLAG_TAG).Length > 0)
                yield return new WaitForEndOfFrame();
            StopAllCoroutines();
            parentCounter++;
            if (parentCounter == 1)
            {
                InitializeEverything();
            }
            else
            {
                OnTaskCompleted();
            }
        }
    }
}