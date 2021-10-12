using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2.Exp19
{
    public class Task_FixFusedPlatesOnFixture : Task
    {
        [SerializeField] XRSocketInteractor socket;
        [SerializeField] List<GameObject> highlights;
        [SerializeField] Task_WeldingRun weldingRun;
        [SerializeField] WeldPositionerScrew screw;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            socket.socketActive = true;
            highlights.ForEach(highlight => highlight.SetActive(true));
            socket.selectEntered.AddListener(OnPlatesPlaced);
        }

        void OnPlatesPlaced(SelectEnterEventArgs args)
        {
            FusedPlates fusedPlates = args.interactable.gameObject.GetComponent<FusedPlates>();
            if (fusedPlates!=null && fusedPlates.plateState==FusedPlates.PlateState.ReadyForRootRunWelding)
            {
                highlights.ForEach(highlight => highlight.SetActive(false));
                weldingRun.platesToPerformWeldingOn = fusedPlates;
                fusedPlates.transform.SetPositionAndRotation(socket.attachTransform.position, socket.attachTransform.rotation);
                socket.socketActive = false;
                args.interactable.enabled = false;
                screw.TurnTheScrew(false);
                screw.OnTargetValueReach.AddListener(OnScrewClosed);
            }
        }

        void OnScrewClosed()
        {
            OnTaskCompleted();
        }
    }
}