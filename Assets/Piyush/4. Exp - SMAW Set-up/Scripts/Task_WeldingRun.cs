using FlatWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SMAW_Setup_4
{
    public class Task_WeldingRun : Task
    {
        [SerializeField] internal GameObject pointsParent;
        [SerializeField] GameObject highlight;
        List<CornerWelding.WeldingPoint> weldingPoints;
        [SerializeField] int pointsCount = 0;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            pointsParent.SetActive(true);
            highlight.SetActive(true);
            weldingPoints = new List<CornerWelding.WeldingPoint>(pointsParent.GetComponentsInChildren<CornerWelding.WeldingPoint>());
            pointsCount = 0;
            weldingPoints.ForEach(point => point.OnWeldingDone += OnWeldingDoneOnPoint);
        }

        private void OnWeldingDoneOnPoint(CornerWelding.WeldingPoint point)
        {
            highlight.SetActive(false);
            pointsCount++;
            if (pointsCount >= weldingPoints.Count)
                OnTaskCompleted();
        }


        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            GameObject highlight = GameObject.Find("JobHighlight");
            highlight.GetComponent<XRSocketInteractor>().socketActive = true;
            GameObject plate = pointsParent.transform.parent.gameObject;
            plate.transform.position = highlight.transform.position;
            GameObject electrode = FindObjectOfType<Electrode>().gameObject;
            WeldingMachine weldingMachine = FindObjectOfType<WeldingMachine>();
            weldingMachine.socket_Electrode.socketActive = true;
            electrode.transform.SetPositionAndRotation(weldingMachine.tip.transform.position, weldingMachine.tip.transform.rotation);
        }

        [ContextMenu(nameof(PerformWelding))]
        public void PerformWelding()
        {
            weldingPoints.ForEach(p => p.OnWeldingTimerFinish());
        }
    }
}