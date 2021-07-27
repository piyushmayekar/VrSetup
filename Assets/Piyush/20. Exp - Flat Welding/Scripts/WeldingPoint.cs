using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using FlatWelding;
using UnityEngine;
using UnityEngine.Events;

namespace FlatWelding
{
    /// <summary>
    /// 2 types of welding point. exterior ones after welding done, the collider turns into non trigger one.
    /// </summary>
    public class WeldingPoint : MonoBehaviour
    {
        public event UnityAction OnWeldingDone, OnHitWithHammer;

        [SerializeField] bool isWeldingDone = false, shouldShowSlag = false;
        [SerializeField] float breakForceThreshold = 1.5f, weldingTimer = 1f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] ParticleSystem residueThrowPS;
        [SerializeField] Material indicatorMaterial, weldingMat, afterHammerHitMat;
        Rigidbody rb;
        Coroutine cor = null;

        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool ShouldShowSlag { get => shouldShowSlag; set => shouldShowSlag = value; }

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
            OnWeldingDone?.Invoke();
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(_Constants.CHIPPING_HAMMER_TAG) && other.relativeVelocity.magnitude >= breakForceThreshold)
            {
                rb.detectCollisions = false;
                _renderer.material = afterHammerHitMat;
                residueThrowPS.Play();
                OnHitWithHammer?.Invoke();
            }
        }
    }
}