using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2.Exp19
{
    public class Task_FusedPlatesPlacement : Task
    {
        [SerializeField] XRSocketInteractor socket;
        [SerializeField] GameObject highlight;
        [SerializeField] Task_WeldingRun weldingRun;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            socket.socketActive = true;
            highlight.SetActive(true);
            socket.selectEntered.AddListener(OnSocketEnter);
        }

        void OnSocketEnter(SelectEnterEventArgs args)
        {
            FusedPlates fusedPlates = args.interactable.gameObject.GetComponent<FusedPlates>();
            if (fusedPlates!=null && fusedPlates.plateState == FusedPlates.PlateState.ReadyForRootRunWelding)
            {
                fusedPlates.transform.SetPositionAndRotation(socket.attachTransform.position, socket.attachTransform.rotation);
                socket.socketActive = false;
                args.interactable.enabled = false;
                weldingRun.platesToPerformWeldingOn = fusedPlates;
                highlight.SetActive(false);
                OnTaskCompleted();
            }
        }

        //Debug Functions

        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            var fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            fusedPlatesList.ForEach(fusedPlate =>
            {
                fusedPlate.finalPlates.ForEach(plate => plate.SetActive(true));
                fusedPlate.grabInteractable.enabled = true;
                fusedPlate.plateState = FusedPlates.PlateState.ReadyForRootRunWelding;
            });
        }
    }
}