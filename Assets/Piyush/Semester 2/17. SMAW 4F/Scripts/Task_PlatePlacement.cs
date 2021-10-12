using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2.Exp17
{
    /// <summary>
    /// Place plate 100 in vertical position, then the 4 plate stands. Place the 2 45 plates on top of the stands.
    /// </summary>
    public class Task_PlatePlacement : Task
    {
        [SerializeField, Tooltip("The sockets to place plates & stands in")] 
        List<XRSocketInteractor> plateSockets, standSockets;

        [SerializeField, Tooltip("The type of plates to place in each socket")] 
        List<PlateType> plateTypes;

        [SerializeField, Tooltip("Current index of plate & stand")]
        int plateIndex = 0, standIndex = 0;

        [SerializeField, Tooltip("Final plates to display")] 
        List<GameObject> finalPlates;

        [SerializeField] float interval = .25f;

        const string STAND = "Stand";

        XRSocketInteractor socketToDisable = null;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            StartPlatePlacement();
        }

        void StartPlatePlacement()
        {
            plateSockets[plateIndex].gameObject.SetActive(true);
            plateSockets[plateIndex].selectEntered.AddListener(OnPlateSocketSelectEnter);
        }

        void OnPlateSocketSelectEnter(SelectEnterEventArgs args)
        {
            StartCoroutine(OnPlateSocketSelectEnterCor(args));
        }

        IEnumerator OnPlateSocketSelectEnterCor(SelectEnterEventArgs args)
        {
            WaitForSeconds wait = new WaitForSeconds(interval);
            JobPlate jobPlate = args.interactable.GetComponent<JobPlate>();
            if (jobPlate && jobPlate.PlateType == plateTypes[plateIndex])
            {
                jobPlate.gameObject.SetActive(false);
                finalPlates[plateIndex].SetActive(true);
                yield return wait;
                socketToDisable = args.interactor as XRSocketInteractor;
                socketToDisable.GetComponent<MeshRenderer>().enabled = false;
                socketToDisable.gameObject.SetActive(false);
                plateIndex++;
                if (plateIndex >= plateSockets.Count)
                    OnTaskCompleted();
                else if (plateIndex == 1)
                    StartStandPlacement();
                else if (plateIndex == 2)
                    StartPlatePlacement();
            }
        }

        void StartStandPlacement()
        {
            standSockets[standIndex].gameObject.SetActive(true);
            standSockets[standIndex].selectEntered.AddListener(OnStandSocketSelectEnter);
        }

        void OnStandSocketSelectEnter(SelectEnterEventArgs args)
        {
            StartCoroutine(OnStandSocketEnterCor(args));
        }

        IEnumerator OnStandSocketEnterCor(SelectEnterEventArgs args)
        {
            WaitForSeconds wait = new WaitForSeconds(interval);
            GameObject _stand = args.interactable.gameObject;
            if (_stand.name.Contains(STAND))
            {
                _stand.transform.SetPositionAndRotation(standSockets[standIndex].transform.position, standSockets[standIndex].transform.rotation);
                args.interactable.enabled = false;
                _stand.GetComponent<Rigidbody>().isKinematic = true;
                yield return wait;
                socketToDisable = standSockets[standIndex];
                socketToDisable.gameObject.SetActive(false);
                standIndex++;
                if (standIndex >= standSockets.Count)
                    StartPlatePlacement();
                else
                    StartStandPlacement();
            }
        }
    }
}