using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class CuttingPoint : MonoBehaviour
    {
        [SerializeField] JobPlate job;
        [SerializeField] GameObject indicator;
        [SerializeField] Collider _collider;
        [SerializeField] List<Collider> bladePoints;
        [SerializeField] List<bool> pointTouched = new List<bool>() { false, false, false };


        public void OnContactWithBladePoint(Collider point)
        {
            pointTouched[bladePoints.FindIndex(x => x == point)] = true;
            if (pointTouched.TrueForAll(x => x))
            {
                _collider.enabled = false;
                indicator.SetActive(false);
                job.CuttingDone();
            }
        }

    }
}