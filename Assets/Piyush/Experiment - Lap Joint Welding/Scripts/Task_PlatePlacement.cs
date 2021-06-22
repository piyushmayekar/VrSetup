using System.Collections;
using System.Collections.Generic;
using VWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace LapWelding
{
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The highlights of both the job positions")]
        List<XRSocketInteractor> plateSockets;
        [SerializeField] XRGrabInteractable finalPlates;
        [SerializeField] int platesCount = 0;

        [SerializeField, Tooltip("The final job plates which will be shown after the plates are placed in the sockets")]
        List<GameObject> finalJobs;

        [SerializeField, Tooltip("The highlights of spacer positions")]
        List<XRSocketInteractor> standSockets;
        [SerializeField] int standIndex = 0;
        [SerializeField] Material standMaterial;
        [SerializeField] ScriberMarking scriberMarking;

        //Step 1: Place one job plate in the bottom position
        //Step 2: Place the plate stands
        //Step 3: Place the second plate on top of it

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            TogglePlatePlacementPoint(platesCount);
        }

        public void OnPlatePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                int index = plateSockets.FindIndex(socket => args.interactor == socket);
                if (args.interactor == plateSockets[index])
                {
                    Destroy(args.interactable.gameObject);
                    finalJobs[index].SetActive(true);
                    platesCount++;
                    TogglePlatePlacementPoint(index, false);
                }
                if (platesCount == 1)
                    ToggleStandPlacementPoint(standIndex);
                if (platesCount == 2)
                    OnTaskCompleted();
            }
        }

        public void OnStandSocketPlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.name.Contains(_Constants.PLATE_STAND))
            {
                Destroy(args.interactable.gameObject);
                ToggleStandPlacementPoint(standIndex, false);
                standIndex++;
                if (standIndex == standSockets.Count)
                {
                    scriberMarking.StartMarkingProcess();
                    scriberMarking.OnMarkingDone += () =>
                    {
                        TogglePlatePlacementPoint(1);
                    };
                }
                else
                    ToggleStandPlacementPoint(standIndex, true);
            }
        }

        public void ToggleStandPlacementPoint(int index, bool on = true)
        {
            standSockets[index].socketActive = on;
            if (on)
                standSockets[index].GetComponentInChildren<MeshRenderer>().enabled = on;
            else
                standSockets[index].GetComponentInChildren<MeshRenderer>().material = standMaterial;
        }

        public void TogglePlatePlacementPoint(int index, bool on = true)
        {
            plateSockets[index].socketActive = on;
            plateSockets[index].GetComponentInChildren<MeshRenderer>().enabled = on;
        }
    }
}