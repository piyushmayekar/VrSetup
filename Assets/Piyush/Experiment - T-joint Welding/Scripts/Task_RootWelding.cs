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
        [SerializeField] Transform leftPointingT, rightPointingT, electrodeT;
        [SerializeField, Tooltip("The max angle between the job plate angle vector & the electrode beyond which we will display an error")]
        float maxAngleThreshold = 10f;
        [SerializeField] int interiorWeldingPoints = 0, weldingPointsToHitWHammer = 0; //Exterior points
        [SerializeField] List<WeldingPoint> weldingPoints;

        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            machine = WeldingMachine.Instance;
            machine.ToggleMachine(false);
            weldingArea.SetActive(false); //To not let the user weld without setting the correct current
            CurrentKnob.OnTargetValueSet += InitializeEverything;
        }

        private void InitializeEverything()
        {
            weldingArea.SetActive(true);
            pointsParent.SetActive(true);
            weldingPoints.ForEach(point =>
            {
                if (point.IsExteriorWeldingPoint)
                {
                    weldingPointsToHitWHammer++;
                    point.OnHitWithHammer += () =>
                    {
                        weldingPointsToHitWHammer--;
                        CheckIfTaskCompleted();
                    };
                }
                else
                {
                    interiorWeldingPoints++;
                    point.OnWeldingDone += () =>
                    {
                        interiorWeldingPoints--;
                        CheckIfTaskCompleted();
                    };
                }
            });
            WeldingPoint.CheckWeldingElectrodeAngle += (isLeft) =>
            {
                float angle = 0f;
                if (isLeft) angle = Vector3.Angle(-electrodeT.forward, leftPointingT.forward);
                else angle = Vector3.Angle(-electrodeT.forward, rightPointingT.forward);
                machine.ShowErrorIndicator(angle >= maxAngleThreshold, _Constants.ELECTRODE_NOT_AT_CORRECT_ANGLE);
            };
        }

        public void CheckIfTaskCompleted()
        {
            if (interiorWeldingPoints <= 0 && weldingPointsToHitWHammer <= 0)
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