using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PiyushUtils
{
    /// <summary>
    /// The v2 of scriber marking process
    /// </summary>
    public class ScriberMarking : MonoBehaviour
    {
        [Tooltip("For freezing & unfreezing the job plate on steel rule placement")]
        public UnityEvent OnSteelRulerPlacedEvent, OnSteelRulerRemovedEvent;
        public UnityAction OnMarkingDone;
        [SerializeField] List<SteelRuleHighlight> steelRuleHighlights =new List<SteelRuleHighlight>();
        [SerializeField] List<GameObject> scriberHighlights, dotPoints;
        [SerializeField] int dotPointIndex = 0, linePointIndex=0;
        [SerializeField] bool isSteelRulerPlaced = false, isScriberTipOnPlate = false, isDotMarkingDone = false, isLineMarkingDone=false;
        [SerializeField, Tooltip("The min distance from the point & the scriber tip to be considered as marked")] 
        float dotDistanceThreshold = 0.01f, lineDistThreshold=0.011f;
        [SerializeField] Transform startLinePos, endLinePos;
        [SerializeField] LineRenderer lineRenderer;
        [SerializeField, Range(0f,1f)] float lineMoveStep = 0.05f;
        [SerializeField] List<Vector3> linePositions=new List<Vector3>();
        [SerializeField] float tipDist = 0f;
        [SerializeField] Collider detectorCollider;
        [SerializeField] SoundPlayer soundPlayer;
        
        Transform tipT;
        Scriber scriber;

        public bool IsDotMarkingDone { get => isDotMarkingDone; set => isDotMarkingDone = value; }
        public bool IsLineMarkingDone { get => isLineMarkingDone; set => isLineMarkingDone = value; }

        internal void StartMarkingProcess()
        {
            if ((steelRuleHighlights[dotPointIndex]) != null)
            {
                steelRuleHighlights[dotPointIndex].gameObject.SetActive(true);
                Vector3 pos = startLinePos.localPosition;
                float t = 0f;
                do
                {
                    linePositions.Add(pos);
                    t += lineMoveStep;
                    pos = Vector3.Lerp(startLinePos.localPosition, endLinePos.localPosition, t);
                }
                while (t < 1f);
                detectorCollider.enabled = true;
            }
        }

        private void Start()
        {
            scriber = Scriber.Instance;
        }

        public void OnSteelRulerPlaced()
        {
            isSteelRulerPlaced = true;
            scriberHighlights[dotPointIndex].SetActive(true);
            OnSteelRulerPlacedEvent?.Invoke();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.SCRIBER_TIP_TAG) && isSteelRulerPlaced)
            {
                scriber.OnMarkingAreaEnter();
                isScriberTipOnPlate = true;
                tipT = other.transform;
                StartCoroutine(TipTracker());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.SCRIBER_TIP_TAG))
            {
                scriber.OnMarkingAreaExit();
                isScriberTipOnPlate = false;
                soundPlayer.StopPlayingAllSounds();
            }
            if (other.CompareTag(_Constants.STEEL_RULER_TAG))
            {
                if(dotPointIndex >= 0 && dotPointIndex < steelRuleHighlights.Count)
                    steelRuleHighlights[dotPointIndex].gameObject.SetActive(true);
                OnSteelRulerRemovedEvent?.Invoke();
            }
        }

        IEnumerator TipTracker()
        {
            while (isScriberTipOnPlate)
            {
                if (!IsDotMarkingDone)
                {
                    tipDist = Vector3.Distance(dotPoints[dotPointIndex].transform.position, tipT.position);
                    if (tipDist <= dotDistanceThreshold)
                    {
                        isScriberTipOnPlate = false;
                        dotPoints[dotPointIndex].GetComponent<SpriteRenderer>().enabled = true;
                        steelRuleHighlights[dotPointIndex].TurnOnRulerGrab();
                        isSteelRulerPlaced = false;
                        scriberHighlights[dotPointIndex].SetActive(false);
                        dotPointIndex++;
                        IsDotMarkingDone = dotPointIndex >= dotPoints.Count;
                    }
                }
                else if(!IsLineMarkingDone)
                {
                    if (lineRenderer.positionCount == 0)
                    {
                        tipDist = Vector3.Distance(startLinePos.position, tipT.position);
                        if (tipDist <= lineDistThreshold)
                        {
                            lineRenderer.positionCount = 2;
                            lineRenderer.SetPosition(0, startLinePos.localPosition);
                            lineRenderer.SetPosition(1, startLinePos.localPosition);
                            scriberHighlights[dotPointIndex].SetActive(false);
                        }
                    }
                    else
                    {
                        tipDist = Vector3.Distance(transform.TransformPoint(linePositions[linePointIndex]), tipT.position);
                        if (tipDist <= lineDistThreshold)
                        {
                            lineRenderer.SetPosition(1, linePositions[linePointIndex]);
                            if (!soundPlayer.AudioSource.isPlaying)
                                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
                            linePointIndex++;
                            if (linePointIndex >= linePositions.Count)
                            {
                                AllMarkingsDone();
                            }
                        }
                    }
                }
                yield return new WaitForEndOfFrame();
            }
        }

        void AllMarkingsDone()
        {
            IsLineMarkingDone = true;
            detectorCollider.enabled = false;
            steelRuleHighlights[dotPointIndex].TurnOnRulerGrab();
            OnMarkingDone?.Invoke();
            scriber.OnMarkingAreaExit();
            StopAllCoroutines();
            soundPlayer.StopPlayingAllSounds();
        }

        [ContextMenu("Set Scriber Highlight Positions")]
        /// <summary>
        /// For Editor
        /// </summary>
        public void SetScriberHighlightPositions()
        {
            scriberHighlights[0].transform.localPosition = dotPoints[0].transform.localPosition;
            scriberHighlights[1].transform.localPosition = dotPoints[1].transform.localPosition;
            scriberHighlights[2].transform.localPosition = startLinePos.localPosition;
        }

        [ContextMenu("Draw Line")]
        /// <summary>
        /// For Editor
        /// </summary>
        public void DrawLine()
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startLinePos.transform.localPosition);
            lineRenderer.SetPosition(1, endLinePos.transform.localPosition);
        }

        [ContextMenu("Remove Line")]
        /// <summary>
        /// For Editor
        /// </summary>
        public void RemoveLine()
        {
            lineRenderer.positionCount = 0;
        }

        [ContextMenu("Instant Complete Marking")]
        /// <summary>
        /// For Editor
        /// </summary>
        public void InstantCompleteMarking()
        {
            DrawLine();
            IsDotMarkingDone = true;
            AllMarkingsDone();
        }

    }
}