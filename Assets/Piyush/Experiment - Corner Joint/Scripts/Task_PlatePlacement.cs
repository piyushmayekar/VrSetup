using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CornerWelding
{
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The highlights of both the job positions")]
        List<XRSocketInteractor> jobSockets;
        [SerializeField] List<GameObject> finalSpacers;

        [SerializeField, Tooltip("The final job plates which will be shown after the plates are placed in the sockets")]
        List<GameObject> finalJobs;

        [SerializeField, Tooltip("The highlights of spacer positions")]
        List<XRSocketInteractor> spacerSockets;
        [SerializeField] int spacerIndex = 0;

        //Step 1: Place one job plate at the flat position
        //Step 2: Place the spacers in place
        //Step 3: Place the other job plate at the vertical position

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            TogglePlatePlacementPoint(0);
        }

        public void OnPlatePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                int index = jobSockets.FindIndex(socket => args.interactor.gameObject == socket.gameObject);
                if (args.interactor == jobSockets[index])
                {
                    Destroy(args.interactable.gameObject);
                    TogglePlatePlacementPoint(index, false);
                    finalJobs[index].SetActive(true);
                }
                if (index == 0)
                {
                    //Start Spacer placement
                    ToggleSpacerPlacementPoint(spacerIndex);
                }
                else if (index == 1)
                {
                    OnTaskCompleted();
                }
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
                    TogglePlatePlacementPoint(1);
                else
                    ToggleSpacerPlacementPoint(spacerIndex, true);
            }
        }

        public void ToggleSpacerPlacementPoint(int index, bool on = true)
        {
            spacerSockets[index].socketActive = on;
            spacerSockets[index].GetComponentInChildren<MeshRenderer>().enabled = on;
        }

        public void TogglePlatePlacementPoint(int index, bool on = true)
        {
            jobSockets[index].socketActive = on;
            jobSockets[index].GetComponent<MeshRenderer>().enabled = on;
        }
    }
}