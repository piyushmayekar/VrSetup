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
        [SerializeField] SpannerTrigger nutSpannerTrigger, screwSpannerTrigger;
        [SerializeField] List<Transform> spanners;
        [SerializeField] Transform nutFittingSpannerT;
        [SerializeField] bool isSpannerInNutRange = false, isSpannerInScrewRange = false;
        [SerializeField] float spannerInitAngle = 0f, rotateSpeed = 2f, repositionSpeed = 1f;
        [SerializeField] Vector3 nutFitPos;
        Transform nutTransform => objectsToPlace[objectsToPlace.Count - 1].transform;
        public void StartConnecting()
        {
            sockets[socketIndex].socketActive = true;
            if (socketIndex == 0)
                objectsToPlace.ForEach(o => o.GetComponent<Outline>().enabled = true);
        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.gameObject == objectsToPlace[socketIndex])
            {
                objectsToPlace[socketIndex].GetComponent<Outline>().enabled = false;
                args.interactor.selectEntered.RemoveAllListeners();
                StartCoroutine(TimedSocketEnabler());
            }
            else
            {
                sockets[socketIndex].socketActive = false;
                Invoke(nameof(StartConnecting), 1f);
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
                spanners.ForEach(spanner => spanner.GetComponent<Outline>().enabled = true);
                //Start screw motion
                nutSpannerTrigger.OnSpannerEnter += () =>
                {
                    isSpannerInNutRange = true;
                    nutFittingSpannerT = nutSpannerTrigger.spannerT;
                    StartCoroutine(NutFitter());
                };
                nutSpannerTrigger.OnSpannerExit += () =>
                {
                    isSpannerInNutRange = false;
                    StopCoroutine(NutFitter());
                    spannerInitAngle = 0f;
                };
                screwSpannerTrigger.OnSpannerEnter += () =>
                {
                    isSpannerInScrewRange = true;
                };
                nutSpannerTrigger.OnSpannerExit += () =>
                {
                    isSpannerInScrewRange = false;
                };
            }
        }

        IEnumerator NutFitter()
        {
            while (isSpannerInNutRange)
            {
                if (isSpannerInScrewRange)
                {
                    float angle = Vector3.SignedAngle(transform.right, nutFittingSpannerT.transform.up,
                 transform.right);
                    if (spannerInitAngle == 0f) spannerInitAngle = angle;
                    else
                    {
                        var dt = angle - spannerInitAngle;
                        nutTransform.Rotate(new Vector3(-Mathf.Abs(dt) * rotateSpeed, 0f, 0f), Space.Self);
                        var newNutPosition = Vector3.MoveTowards(nutTransform.localPosition,
                        nutFitPos, dt * repositionSpeed);
                        if (Vector3.Distance(newNutPosition, nutFitPos) < Vector3.Distance(nutTransform.localPosition, nutFitPos))
                            nutTransform.localPosition = Vector3.MoveTowards(nutTransform.localPosition,
                            nutFitPos, dt * repositionSpeed);
                        spannerInitAngle = angle;
                        if (nutTransform.localPosition == nutFitPos)
                        {
                            OnNutFittingDone();
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnNutFittingDone()
        {
            nutSpannerTrigger.GetComponent<Collider>().enabled = false;
            spanners.ForEach(spanner => spanner.GetComponent<Outline>().enabled = false);
            StopAllCoroutines();
            OnConnectionDone?.Invoke();
        }
    }
}