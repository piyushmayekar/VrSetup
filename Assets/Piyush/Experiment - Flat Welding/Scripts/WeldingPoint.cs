using System;
using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;

namespace FlatWelding
{
    /// <summary>
    /// 2 types of welding point. exterior ones after welding done, the collider turns into non trigger one.
    /// </summary>
    public class WeldingPoint : MonoBehaviour
    {
        public event Action OnWeldingDone, OnHitWithHammer;

        [SerializeField] bool isWeldingDone = false, shouldShowSlag = false;
        [SerializeField] float breakForceThreshold = 1.5f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] ParticleSystem residueThrowPS;
        [SerializeField] Material indicatorMaterial, weldingMat, afterHammerHitMat;
        Rigidbody rb;

        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool ShouldShowSlag { get => shouldShowSlag; set => shouldShowSlag = value; }

        static WeldingMachine machine;
        static ChippingHammer chippingHammer;
        static WeldingArea weldingArea;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            machine = WeldingMachine.Instance;
            chippingHammer = ChippingHammer.Instance;
            weldingArea = WeldingArea.Instance;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG) && !isWeldingDone)
            {
                IsWeldingDone = true;
                _renderer.material = weldingMat;
                _renderer.enabled = true;
                OnWeldingDone?.Invoke();
                if (ShouldShowSlag)
                {
                    GetComponent<Collider>().isTrigger = false;
                }
            }
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