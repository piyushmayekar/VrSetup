using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class PlateMarkingSocket : MonoBehaviour
    {
        [SerializeField] List<Transform> attachTransforms;
        [SerializeField] List<GameObject> plateHighlights=new List<GameObject>();
        [SerializeField] int _plateHighlightIndex = 0;
        [SerializeField] XRSocketInteractor socket;
        [Tooltip("Set to false if center punch marking is being done")]
        public bool isScriberMarkingBeingDone = true;
        [SerializeField] GameObject waitForObjectToExitTrigger = null;
        [SerializeField] GameObject messageCanvas;
        [SerializeField] Outline outline;

        private void Start()
        {
            attachTransforms.ForEach(t => plateHighlights.Add(t.GetChild(0).gameObject));
        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.JOB_TAG))
            {
                ToggleAttentionGrab(false);
                if (isScriberMarkingBeingDone)
                {
                    Transform parentT = args.interactable.GetComponentInChildren<ScriberMarking>().transform.parent;
                    ScriberMarking[] markings = parentT.GetComponentsInChildren<ScriberMarking>();
                    for (int i = 0; i < markings.Length; i++)
                    {
                        if(!markings[i].IsLineMarkingDone)
                        {
                            socket.attachTransform = attachTransforms[i];
                            _plateHighlightIndex = i;
                            plateHighlights.ForEach(hlt => hlt.SetActive(false));
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
                            _plateHighlightIndex = i;
                            plateHighlights.ForEach(hlt => hlt.SetActive(false));
                            break;
                        }
                    }
                }
            }
            else
            {
                //if (!(args.interactable is XRSimpleInteractable))
                //{
                //    socket.socketActive = false;
                //    waitForObjectToExitTrigger = args.interactable.gameObject;
                //    StartCoroutine(WaitForObjectToExitTrigger());
                //}
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
                plateHighlights.ForEach(hlt => hlt.SetActive(false));
                int nextIndex = _plateHighlightIndex + 1;
                if (nextIndex >= 0 && nextIndex < plateHighlights.Count)
                    plateHighlights[nextIndex].SetActive(true);
            }
        }

        public void SetScriberMarkingMode(bool scriber) => isScriberMarkingBeingDone = scriber;

        public void ToggleAttentionGrab(bool attention = true)
        {
            outline.enabled = attention;
            messageCanvas.SetActive(attention);
            if(attention)
            {
                _plateHighlightIndex = 0;
                plateHighlights[_plateHighlightIndex].SetActive(true);
            }
        }
    }
}