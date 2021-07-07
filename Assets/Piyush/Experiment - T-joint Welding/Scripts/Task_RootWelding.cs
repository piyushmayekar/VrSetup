using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatWelding;

namespace TWelding
{
    public class Task_RootWelding : Task
    {
        [SerializeField] GameObject weldingArea;
        [SerializeField] GameObject pointsParent, tackingPointsParent;
        [SerializeField, Tooltip("Transforms used for determining electrode angle")]
        Transform leftPointingT, rightPointingT, electrodeT;
        [SerializeField, Tooltip("The max angle between the job plate angle vector & the electrode beyond which we will display an error")]
        float maxAngleThreshold = 10f;
        [SerializeField] int weldingRemainingCount = 0, slagRemaining = 0; //Exterior points
        [SerializeField] List<WeldingPoint> weldingPoints;
        [SerializeField] CurrentKnob currentKnob;
        [SerializeField] float targetCurrent = 130f;
        [SerializeField] ElectrodeType requiredElectrodeType = ElectrodeType._315mm;
        [SerializeField] bool isElectrodePlaced = false, isCurrentSet = false;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            machine = WeldingMachine.Instance;
            machine.ToggleMachine(false);
            weldingArea.SetActive(false); //To not let the user weld without setting the correct current
            currentKnob.TargetValue = targetCurrent;
            if (currentKnob.CurrentValue == targetCurrent)
            {
                isCurrentSet = true;
                if (isCurrentSet && isElectrodePlaced)
                    InitializeEverything();
            }
            else
                CurrentKnob.OnTargetValueSet += () =>
                {
                    isCurrentSet = true;
                    if (isCurrentSet && isElectrodePlaced)
                        InitializeEverything();
                };
            machine.RequiredElectrodeType = requiredElectrodeType;
            isElectrodePlaced = machine.CheckIfRequiredElectrodePlaced(requiredElectrodeType);
            machine.OnElectrodePlacedEvent += () =>
            {
                isElectrodePlaced = machine.IsElectrodePlaced;
                if (isCurrentSet && isElectrodePlaced)
                    InitializeEverything();
            };
        }

        private void InitializeEverything()
        {
            weldingPoints = new List<WeldingPoint>(
                pointsParent.GetComponentsInChildren<WeldingPoint>());
            weldingArea.SetActive(true);
            pointsParent.SetActive(true);
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
                        CheckIfTaskCompleted();
                    };
            });
            weldingRemainingCount = weldingPoints.Count;
            slagRemaining = weldingPoints.Count;

            WeldingPoint.CheckWeldingElectrodeAngle += (isLeft) =>
            {
                float angle = 0f;
                if (isLeft) angle = Vector3.Angle(electrodeT.up, leftPointingT.forward);
                else angle = Vector3.Angle(electrodeT.up, rightPointingT.forward);
                machine.ShowErrorIndicator(angle >= maxAngleThreshold, _Constants.ELECTRODE_NOT_AT_CORRECT_ANGLE);
            };
        }

        public void CheckIfTaskCompleted()
        {
            if (weldingRemainingCount <= 0 && slagRemaining <= 0)
            {
                CurrentKnob.OnTargetValueSet -= InitializeEverything;
                tackingPointsParent.SetActive(false);
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