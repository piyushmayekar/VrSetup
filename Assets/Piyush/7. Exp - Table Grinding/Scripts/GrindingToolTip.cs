using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Grinding
{
    public class GrindingToolTip : MonoBehaviour
    {
        [SerializeField] GrindingWheelType type;
        public UnityAction OnGrindingComplete;
        public float timer = 1f;

        void OnTriggerEnter(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel && wheel.Type == type && wheel.IsWheelSpinning)
                _OnCollisionEnterWithWheel();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.GRINDING_WHEEL_TAG))
                _OnCollisionExitWithWheel();
        }

        public void _OnCollisionEnterWithWheel()
        {
            if (timer > 0f)
                StartCoroutine(InContactWithWheel());
        }

        IEnumerator InContactWithWheel()
        {
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (timer <= 0f)
                OnGrindingComplete?.Invoke();
        }

        public void _OnCollisionExitWithWheel()
        {
            StopCoroutine(InContactWithWheel());
        }
    }
}