using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlatWelding;

namespace CornerWelding
{
    public class Task_RootWelding : Task
    {
        [SerializeField] GameObject weldingArea;
        [SerializeField] GameObject tackingPointsParent;
        [SerializeField] int weldingRemainingCount = 0, slagRemaining = 0; //Exterior points
        [SerializeField] List<WeldingPoint> weldingPoints;
        [SerializeField] GameObject machineAnimation;
        WeldingMachine machine;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            machine = WeldingMachine.Instance;
            InitializeEverything();
        }

        private void InitializeEverything()
        {
            weldingArea.SetActive(true);
            weldingPoints[0].transform.parent.gameObject.SetActive(true);
            machineAnimation.SetActive(true);
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
                point.OnWeldingDone += (point) =>
                {
                    weldingRemainingCount--;
                    CheckIfTaskCompleted();
                };
            });
            weldingRemainingCount = weldingPoints.Count;
            slagRemaining = weldingPoints.Count;
        }

        public void CheckIfTaskCompleted()
        {
            machineAnimation.SetActive(false);
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