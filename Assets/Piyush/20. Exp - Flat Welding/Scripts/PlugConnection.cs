using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class PlugConnection : MonoBehaviour
    {
        public UnityEvent OnConnectionDone;
        [SerializeField] List<XRSocketInteractor> sockets;
        [SerializeField] List<GameObject> objectsToPlace;
        [SerializeField] int socketIndex;
        [SerializeField] float waitTime = 1f;
        [SerializeField] PlugNut plugNut;
        [SerializeField] Transform spannerT;
        [SerializeField] bool isSpannerInScrewRange = false;
        [SerializeField] float spannerInitAngle = 0f, rotateSpeed = 2f, repositionSpeed = 1f;
        [SerializeField] Vector3 nutFitPos;
        Transform nutTransform => objectsToPlace[objectsToPlace.Count - 1].transform;
        public void StartConnecting()
        {
            sockets[socketIndex].socketActive = true;
        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.gameObject == objectsToPlace[socketIndex])
            {
                args.interactor.selectEntered.RemoveAllListeners();
                StartCoroutine(TimedSocketEnabler());
            }
        }

        IEnumerator TimedSocketEnabler()
        {
            yield return new WaitForSeconds(.1f);
            objectsToPlace[socketIndex].GetComponent<XRGrabInteractable>().enabled = false;
            objectsToPlace[socketIndex].GetComponent<Rigidbody>().isKinematic = true;
            sockets[socketIndex].socketActive = false;
            yield return new WaitForSeconds(waitTime);
            socketIndex++;
            if (socketIndex < sockets.Count)
                StartConnecting();
            else
            {
                //Start screw motion
                plugNut.OnSpannerEnter += () =>
                {
                    isSpannerInScrewRange = true;
                    StartCoroutine(NutFitter());
                };
                plugNut.OnSpannerExit += () =>
                {
                    isSpannerInScrewRange = false;
                    StopCoroutine(NutFitter());
                    spannerInitAngle = 0f;
                };
            }
        }

        IEnumerator NutFitter()
        {
            while (isSpannerInScrewRange)
            {
                float angle = Vector3.SignedAngle(transform.right, spannerT.transform.up,
                 transform.right);
                if (spannerInitAngle == 0f) spannerInitAngle = angle;
                else
                {
                    var dt = angle - spannerInitAngle;
                    nutTransform.Rotate(new Vector3(Mathf.Abs(dt) * rotateSpeed, 0f, 0f), Space.Self);
                    nutTransform.localPosition = Vector3.MoveTowards(nutTransform.localPosition,
                    nutFitPos, dt * repositionSpeed);
                    spannerInitAngle = angle;
                    if (nutTransform.localPosition == nutFitPos)
                    {
                        OnNutFittingDone();
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnNutFittingDone()
        {
            plugNut.GetComponent<Collider>().enabled = false;
            StopAllCoroutines();
            OnConnectionDone?.Invoke();
        }
    }
}