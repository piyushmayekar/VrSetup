using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
{
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The highlights of both the job positions")]
        List<XRSocketInteractor> jobSockets;
        [SerializeField] XRGrabInteractable finalPlates;
        [SerializeField] int platesCount = 0;
        [SerializeField] List<GameObject> finalSpacers;

        [SerializeField, Tooltip("The final job plates which will be shown after the plates are placed in the sockets")]
        List<GameObject> finalJobs;

        [SerializeField, Tooltip("The highlights of spacer positions")]
        List<XRSocketInteractor> spacerSockets;
        [SerializeField] int spacerIndex = 0;

        //Step 1: Place both job plates in specified position
        //Step 2: Place the spacers in place

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            finalPlates.gameObject.SetActive(true);
        }

        public void OnPlatePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                int index = jobSockets.FindIndex(socket => args.interactor == socket);
                if (args.interactor == jobSockets[index])
                {
                    Destroy(args.interactable.gameObject);
                    finalJobs[index].SetActive(true);
                    platesCount++;
                    jobSockets[index].socketActive = false;
                    jobSockets[index].GetComponent<MeshRenderer>().enabled = false;
                }
                if (platesCount >= JobPlate.jobPlates.Count)
                    ToggleSpacerPlacementPoint(spacerIndex);
            }
        }

        public void OnSpacerSocketPlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.SPACER_TAG))
            {
                Destroy(args.interactable.gameObject);
                ToggleSpacerPlacementPoint(spacerIndex, false);
                finalSpacers[spacerIndex].SetActive(true);
                spacerIndex++;
                if (spacerIndex == spacerSockets.Count)
                {
                    OnTaskCompleted();
                }
                else
                    ToggleSpacerPlacementPoint(spacerIndex, true);
            }
        }

        public void ToggleSpacerPlacementPoint(int index, bool on = true)
        {
            spacerSockets[index].socketActive = on;
            spacerSockets[index].GetComponentInChildren<MeshRenderer>().enabled = on;
        }
    }
}