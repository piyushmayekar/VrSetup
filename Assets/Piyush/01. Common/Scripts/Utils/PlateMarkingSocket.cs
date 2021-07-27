using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VWelding;

namespace PiyushUtils
{
    public class PlateMarkingSocket : MonoBehaviour
    {
        [SerializeField] List<Transform> attachTransforms;
        [SerializeField] XRSocketInteractor socket;
        [Tooltip("Set to false if center punch marking is being done")]
        public bool isScriberMarkingBeingDone = true;
        [SerializeField] GameObject waitForObjectToExitTrigger = null;
        [SerializeField] GameObject messageCanvas;
        [SerializeField] Outline outline;
        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                if (isScriberMarkingBeingDone)
                {
                    Transform parentT = args.interactable.GetComponentInChildren<ScriberMarking>().transform.parent;
                    ScriberMarking[] markings = parentT.GetComponentsInChildren<ScriberMarking>();
                    for (int i = 0; i < markings.Length; i++)
                    {
                        if(!markings[i].IsLineMarkingDone)
                        {
                            socket.attachTransform = attachTransforms[i];
                            break;
                        }
                    }
                }
                else
                {
                    Transform parentT = args.interactable.GetComponentInChildren<CenterPunchMarking>().transform.parent;
                    CenterPunchMarking[] markings = parentT.GetComponentsInChildren<CenterPunchMarking>();
                    for (int i = 0; i < markings.Length; i++)
                    {
                        if (!markings[i].IsMarkingDone)
                        {
                            socket.attachTransform = attachTransforms[i];
                            break;
                        }
                    }
                }
            }
            else
            {
                socket.socketActive = false;
                waitForObjectToExitTrigger = args.interactable.gameObject;
                StartCoroutine(WaitForObjectToExitTrigger());
            }
        }

        IEnumerator WaitForObjectToExitTrigger()
        {
            while (waitForObjectToExitTrigger != null)
                yield return null;
            socket.socketActive = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == waitForObjectToExitTrigger)
                waitForObjectToExitTrigger = null;
        }

        public void OnSocketExit(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                socket.attachTransform = attachTransforms[0];
            }
        }

        public void SetScriberMarkingMode(bool scriber) => isScriberMarkingBeingDone = scriber;

        public void ToggleAttentionGrab(bool attention = true)
        {
            outline.enabled = attention;
            messageCanvas.SetActive(attention);
        }
    }
}