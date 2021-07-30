using System.Collections;
using UnityEngine;

namespace Grinding
{
    public class HandPresenceExt : PiyushUtils.HandPresence
    {
        [SerializeField] Collider openHandCollider, closedFistCollider;
        [SerializeField] GrindingMachine grindingMachine;

        private void Awake()
        {
            StartCoroutine(HandColliderManager());
        }

        IEnumerator HandColliderManager()
        {
            while (gameObject.activeSelf)
            {
                openHandCollider.enabled = gripValue <= gripThreshold;
                closedFistCollider.enabled = gripValue > gripThreshold;
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel != null)
                grindingMachine.ToggleHandProximityCanvas(wheel.transform, true);
        }

        private void OnTriggerExit(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel != null)
                grindingMachine.ToggleHandProximityCanvas(wheel.transform, false);
        }
    }
}