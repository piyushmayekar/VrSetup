using CornerWelding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Semester2
{
    public class FusedPlates : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent OnSpacersPlaced, OnTagWeldingDone, OnRootRunWeldingDone, OnSpacersRemoved;

        [HideInInspector, Tooltip("For disabling the machine highlight on welding done of a single point")]
        public UnityEvent OnRootRunPointWeldingDone;

        [SerializeField] internal PlateState plateState;
        
        //Placement
        [SerializeField] List<XRSocketInteractor> plateSockets, spacerSockets;
        [SerializeField] internal XRGrabInteractable grabInteractable;
        [SerializeField] int platesIndex = 0, spacerIndex = 0;
        [SerializeField] internal List<GameObject> finalSpacers, finalPlates;
        [SerializeField] float interval = .25f;

        //Welding
        [SerializeField] Transform tagWeldingPointsParent, rootRunWeldingPointsParent;
        [SerializeField] internal GameObject weldingArea;
        Transform currPointsParent = null;
        List<WeldingPoint> weldingPoints = null;
        int pointIndex = 0;

        #region PLACEMENT
        public void StartPlatePlacement()
        {
            plateSockets[platesIndex].gameObject.SetActive(true);
            plateSockets[platesIndex].socketActive = true;
            plateSockets[platesIndex].GetComponent<MeshRenderer>().enabled = true;
            plateSockets[platesIndex].selectEntered.AddListener(OnPlatePlaced);
        }

        public void OnPlatePlaced(SelectEnterEventArgs args)
        {
            StartCoroutine(OnPlateSocketSelectEnterCor(args));
        }

        IEnumerator OnPlateSocketSelectEnterCor(SelectEnterEventArgs args)
        {
            WaitForSeconds wait = new WaitForSeconds(interval);
            GameObject jobPlate = args.interactable.gameObject;
            if (jobPlate.CompareTag(_Constants.JOB_TAG))
            {
                jobPlate.gameObject.SetActive(false);
                finalPlates[platesIndex].SetActive(true);
                yield return wait;
                XRSocketInteractor socketToDisable = args.interactor as XRSocketInteractor;
                socketToDisable = args.interactor as XRSocketInteractor;
                socketToDisable.GetComponent<MeshRenderer>().enabled = false;
                socketToDisable.gameObject.SetActive(false);
                platesIndex++;
                if (platesIndex >= plateSockets.Count)
                    ToggleSpacerPlacementPoint(spacerIndex);
                else
                    StartPlatePlacement();
            }
        }

        public void OnSpacerSocketPlaced(SelectEnterEventArgs args)
        {
            StartCoroutine(OnSpacerSocketPlacedCor(args));
        }

        IEnumerator OnSpacerSocketPlacedCor(SelectEnterEventArgs args)
        {
            WaitForSeconds wait = new WaitForSeconds(interval);
            if (args.interactable.CompareTag(_Constants.SPACER_TAG))
            {
                finalSpacers[spacerIndex].SetActive(true);
                yield return wait;
                ToggleSpacerPlacementPoint(spacerIndex, false);
                Destroy(args.interactable.gameObject);
                finalSpacers[spacerIndex].SetActive(true);
                spacerIndex++;
                if (spacerIndex >= spacerSockets.Count)
                {
                    plateState = PlateState.Created;
                    OnSpacersPlaced?.Invoke();
                }
                else
                    ToggleSpacerPlacementPoint(spacerIndex, true);
            }
        }

        public void ToggleSpacerPlacementPoint(int index, bool on = true)
        {
            spacerSockets[index].socketActive = on;
            spacerSockets[index].GetComponentInChildren<MeshRenderer>().enabled = on;
            if (on) spacerSockets[index].selectEntered.AddListener(OnSpacerSocketPlaced);
            else spacerSockets[index].selectEntered.RemoveAllListeners();
        }

        #endregion

        #region SPACER REMOVAL
        public void StartSpacersRemoval()
        {
            finalSpacers.ForEach(spacer => spacer.GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnSpacerGrab));
            spacerIndex = 0;
        }

        void OnSpacerGrab(SelectEnterEventArgs args)
        {
            finalSpacers[spacerIndex].gameObject.SetActive(false);
            spacerIndex++;
            if (spacerIndex >= finalSpacers.Count)
            {
                plateState = PlateState.ReadyForRootRunWelding;
                grabInteractable.enabled = true;
                OnSpacersRemoved?.Invoke();
            }
        }

        #endregion

        #region WELDING
        public void StartTagWelding()
        {
            currPointsParent = tagWeldingPointsParent;
            currPointsParent.gameObject.SetActive(true);
            weldingArea.SetActive(true);
            AssignPoints();
            weldingPoints.ForEach(point => point.OnWeldingDone += OnTackPointWeldingDone);
        }

        public void StartRootWelding()
        {
            currPointsParent = rootRunWeldingPointsParent;
            currPointsParent.gameObject.SetActive(true);
            weldingArea.SetActive(true);
            AssignPoints();
            weldingPoints.ForEach(point => point.OnHitWithHammer += OnRootRunPointHitWithHammer);
            weldingPoints.ForEach(point => point.OnWeldingDone += OnRootRunPointWeldingDone_);
        }

        void OnTackPointWeldingDone(WeldingPoint point)
        {
            pointIndex++;
            if (pointIndex >= weldingPoints.Count)
            {
                weldingArea.SetActive(false);
                FindObjectOfType<WeldingMachine>().TipInContact(false);
                plateState = PlateState.ReadyForRootRunWelding;
                OnTagWeldingDone?.Invoke();
            }
        }

        void OnRootRunPointWeldingDone_(WeldingPoint point)
        {
            weldingPoints.ForEach(point => point.OnWeldingDone -= OnRootRunPointWeldingDone_);
            OnRootRunPointWeldingDone?.Invoke();
        }

        void OnRootRunPointHitWithHammer()
        {
            pointIndex++;
            if (pointIndex >= weldingPoints.Count)
                CheckIfTaskCompleted();
        }

        public void CheckIfTaskCompleted()
        {
            if (pointIndex >= weldingPoints.Count)
            {
                StartCoroutine(SlagChecker());
            }
        }

        IEnumerator SlagChecker()
        {
            while (GameObject.FindGameObjectsWithTag(_Constants.SLAG_TAG).Length > 0)
                yield return new WaitForEndOfFrame();
            StopAllCoroutines();
            weldingArea.SetActive(false);
            grabInteractable.enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            plateState = PlateState.WeldingDone;
            OnRootRunWeldingDone?.Invoke();
        }

        void AssignPoints()
        {
            pointIndex = 0;
            weldingPoints = new List<WeldingPoint>(currPointsParent.GetComponentsInChildren<WeldingPoint>());
        }
        #endregion

        [System.Serializable]
        public enum PlateState
        {
            NotCreated, Created, ReadyForRootRunWelding, WeldingDone
        }
    }
}