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
        [SerializeField] float breakForceThreshold = 1.5f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Material weldingMat;
        Rigidbody rb;

        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool ShouldShowSlag { get => shouldShowSlag; set => shouldShowSlag = value; }
        public float BreakForceThreshold { get => breakForceThreshold; set => breakForceThreshold = value; }

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
                OnWeldingDone?.Invoke(this);
            }
        }
        internal void OnSlagHitWithHammer() => OnHitWithHammer?.Invoke();
    }
}