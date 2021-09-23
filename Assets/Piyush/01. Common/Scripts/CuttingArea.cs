using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.Events;

namespace PiyushUtils
{
    public class CuttingArea : MonoBehaviour
    {
        public UnityEvent OnCuttingComplete;
        [SerializeField] public bool isCuttingDone = false;
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] int cuttingDoneOnPoints = 0;
        [SerializeField] List<CuttingPoint> cuttingPoints;
        [SerializeField] List<CPMarkingPoint> cpMarkingPoints;

        List<GameObject> closestCuttingPoints = new List<GameObject>();
        CuttingTorch cuttingTorch;
        public int CuttingDoneOnPoints { get => cuttingDoneOnPoints; set => cuttingDoneOnPoints = value; }

        private void OnEnable()
        {
            cuttingTorch = CuttingTorch.Instance;
            cuttingPoints = new List<CuttingPoint>(GetComponentsInChildren<CuttingPoint>());
            cuttingPoints.ForEach(point => point.OnCuttingDone.AddListener(OnPointCuttingDone));
            cuttingPoints[0].DisableAllowed = true;
            cuttingPoints[cuttingPoints.Count - 1].DisableAllowed = true;

            cpMarkingPoints = new List<CPMarkingPoint>(transform.parent.gameObject.GetComponentsInChildren<CPMarkingPoint>());
            cpMarkingPoints.ForEach(mp =>
            {
                CuttingPoint closestCuttingPoint = cuttingPoints[0];
                float minDist = 0.01f;
                for(int i = 0; i < cuttingPoints.Count; i++)
                {
                    float dist = Vector3.Distance(mp.transform.position, cuttingPoints[i].transform.position);
                    if (dist< minDist)
                    {
                        minDist = dist;
                        closestCuttingPoint = cuttingPoints[i];
                    }
                }
                closestCuttingPoints.Add(closestCuttingPoint.gameObject);
            });
        }

        void OnPointCuttingDone(CuttingPoint point)
        {
            point.OnCuttingDone.RemoveAllListeners();
            point.gameObject.SetActive(false);
            int i = cuttingPoints.IndexOf(point);
            if (i > 0) cuttingPoints[i - 1].DisableAllowed = true;
            if (i < cuttingPoints.Count - 1) cuttingPoints[i + 1].DisableAllowed = true;
            CuttingDoneOnPoints++;

            //Turn off closest cp marking point
            int cpIndex = closestCuttingPoints.FindIndex(cp => cp == point.gameObject);
            if (cpIndex >= 0 && cpIndex < cpMarkingPoints.Count)
                cpMarkingPoints[cpIndex].gameObject.SetActive(false);

            if (CuttingDoneOnPoints >= cuttingPoints.Count)
                OnCuttingComplete?.Invoke();
        }

        public void TurnOffCPMarkingPointsInsideRadius(Vector3 pos)
        {
            float dist = 0.1f;
            for (int i = 0; i < cpMarkingPoints.Count; i++)
            {
                 //cpMarkingPoints[i]
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == cuttingTorch.gameObject)
                cuttingTorch.OnTorchEnterCuttingArea();
            if (other.CompareTag(_Constants.FLATFILE_TAG))
                soundPlayer.PlayClip(soundPlayer.Clips[1], true);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == cuttingTorch.gameObject)
                cuttingTorch.OnTorchExitCuttingArea();
            if (other.CompareTag(_Constants.FLATFILE_TAG))
                soundPlayer.StopPlayingAllSounds();
        }

        [ContextMenu(nameof(Hack_CompleteCutting))]
        public void Hack_CompleteCutting()
        {
            cuttingPoints.ForEach(point => point.gameObject.SetActive(false));
            OnCuttingComplete?.Invoke();
        }
    }
}