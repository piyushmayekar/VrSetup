using System;
using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;


namespace CornerWelding
{
    public class WeldingPoint : MonoBehaviour
    {
        public event Action OnHitWithHammer;
        public event Action<WeldingPoint> OnWeldingDone;

        [SerializeField] bool isWeldingDone = false, shouldShowSlag = false;
        [SerializeField] float breakForceThreshold = 1.5f, weldingTimer = 1f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Material weldingMat;
        Rigidbody rb;
        Coroutine cor=null;
        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool ShouldShowSlag { get => shouldShowSlag; set => shouldShowSlag = value; }
        public float BreakForceThreshold { get => breakForceThreshold; set => breakForceThreshold = value; }


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG) && !isWeldingDone)
            {
                if (cor == null)
                    cor = StartCoroutine(Timer());
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG) && !isWeldingDone)
            {
                if (cor != null)
                { 
                    StopCoroutine(cor);
                    cor = null;
                }
            }
        }

        IEnumerator Timer()
        {
            while (weldingTimer > 0f)
            {
                weldingTimer -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            if (weldingTimer <= 0f && !IsWeldingDone)
                OnWeldingTimerFinish();
        }

        void OnWeldingTimerFinish()
        {
            IsWeldingDone = true;
            _renderer.material = weldingMat;
            _renderer.enabled = true;
            OnWeldingDone?.Invoke(this);
        }

        internal void OnSlagHitWithHammer() => OnHitWithHammer?.Invoke();
    }
}