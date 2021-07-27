using System;
using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;


namespace TWelding
{
    /// <summary>
    /// 2 types of welding point. exterior ones after welding done, the collider turns into non trigger one.
    /// </summary>
    public class WeldingPoint : MonoBehaviour
    {
        public event Action OnWeldingDone, OnHitWithHammer;

        /// <summary>
        /// Used to check the angle between the electrode and the plates on the receiver side. 
        /// true if the point is at left
        /// </summary>
        public static event Action<bool> CheckWeldingElectrodeAngle;

        [SerializeField] bool isWeldingDone = false, shouldShowSlag = false;
        [SerializeField] float breakForceThreshold = 1.5f, weldingTimer = 1f;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Material indicatorMaterial, weldingMat;
        [SerializeField] bool isPointOnLeft = false;
        Rigidbody rb;
        Coroutine cor = null;

        public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
        public bool ShouldShowSlag { get => shouldShowSlag; set => shouldShowSlag = value; }
        public float BreakForceThreshold { get => breakForceThreshold; set => breakForceThreshold = value; }
        public bool IsPointOnLeft { get => isPointOnLeft; set => isPointOnLeft = value; }

        static WeldingMachine machine;
        public ChippingHammer chippingHammer;
        static WeldingArea weldingArea;

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            rb = GetComponent<Rigidbody>();
            machine = WeldingMachine.Instance;
            weldingArea = WeldingArea.Instance;
            IsPointOnLeft = Vector3.Distance(weldingArea.LeftT.position, transform.position) <
                    Vector3.Distance(weldingArea.RightT.position, transform.position);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.WELDING_TAG) && !isWeldingDone && (IsPointOnLeft == machine.IsElectrodeAtLeft))
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
            if (ShouldShowSlag)
            {
                GetComponent<Collider>().isTrigger = false;
                CheckWeldingElectrodeAngle?.Invoke(IsPointOnLeft);
            }
        }

        internal void OnSlagHitWithHammer() => OnHitWithHammer?.Invoke();
    }
}