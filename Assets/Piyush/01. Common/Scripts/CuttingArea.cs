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
        CuttingTorch cuttingTorch;
        public int CuttingDoneOnPoints { get => cuttingDoneOnPoints; set => cuttingDoneOnPoints = value; }

        private void OnEnable()
        {
            cuttingTorch = CuttingTorch.Instance;
            cuttingPoints = new List<CuttingPoint>(GetComponentsInChildren<CuttingPoint>());
            cuttingPoints.ForEach(point => point.OnCuttingDone.AddListener(OnPointCuttingDone));
            cuttingPoints[0].DisableAllowed = true;
            cuttingPoints[cuttingPoints.Count - 1].DisableAllowed = true;
        }

        void OnPointCuttingDone(CuttingPoint point)
        {
            point.OnCuttingDone.RemoveAllListeners();
            point.gameObject.SetActive(false);
            int i = cuttingPoints.IndexOf(point);
            if (i > 0) cuttingPoints[i - 1].DisableAllowed = true;
            if (i < cuttingPoints.Count - 1) cuttingPoints[i + 1].DisableAllowed = true;
            CuttingDoneOnPoints++;
            if (CuttingDoneOnPoints >= cuttingPoints.Count)
                OnCuttingComplete?.Invoke();
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
    }
}