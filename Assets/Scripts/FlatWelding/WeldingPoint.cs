using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    /// <summary>
    /// 2 types of welding point. exterior ones after welding done, the collider turns into non trigger one.
    /// </summary>
    public class WeldingPoint : MonoBehaviour
    {
        public event Action OnWeldingDone, OnHitWithHammer;

        [SerializeField] Task task;
        [SerializeField] bool isWeldingDone = false, isExteriorWeldingPoint = false;
        [SerializeField] float breakForceThreshold = 1.5f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] ParticleSystem residueThrowPS;
        [SerializeField] Material indicatorMaterial, weldingMat;

        Rigidbody rb;

        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool IsExteriorWeldingPoint { get => isExteriorWeldingPoint; set => isExteriorWeldingPoint = value; }


        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WeldingTip") && !isWeldingDone)
            {
                IsWeldingDone = true;
                _renderer.material = weldingMat;
                _renderer.enabled = true;
                OnWeldingDone?.Invoke();
                if (IsExteriorWeldingPoint)
                {
                    GetComponent<Collider>().isTrigger = false;
                }
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("ChippingHammer") && other.relativeVelocity.magnitude >= breakForceThreshold)
            {
                rb.detectCollisions = false;
                _renderer.enabled = false;
                residueThrowPS.Play();
                OnHitWithHammer?.Invoke();
            }
        }
    }
}