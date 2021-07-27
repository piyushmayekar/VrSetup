using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class SteelRuleHighlight : MonoBehaviour
    {
        [SerializeField] public UnityEvent OnSteelRulerEnter, OnSteelRuleSelectEnter;

        [SerializeField] Collider _collider;
        [SerializeField] MeshRenderer _mesh;
        [SerializeField] Material rulerMat, highlightMat;
        [SerializeField] XRSimpleInteractable simpleInteractable;

        public static GameObject steelRuler = null;

        private void Start()
        {
            if (steelRuler == null) steelRuler = GameObject.FindGameObjectWithTag(_Constants.STEEL_RULER_TAG);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_Constants.STEEL_RULER_TAG))
            {
                steelRuler = other.gameObject;
                other.gameObject.SetActive(false);
                other.transform.SetPositionAndRotation(transform.position, transform.rotation);
                _mesh.material = rulerMat;
                _collider.isTrigger = false;
                OnSteelRulerEnter?.Invoke();
                simpleInteractable.enabled = true;
            }    
        }

        public void TurnOnRulerGrab()
        {
            steelRuler.transform.SetPositionAndRotation(transform.position, transform.rotation);
            steelRuler.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnRulerSelectEnter(SelectEnterEventArgs args)
        {
            OnSteelRuleSelectEnter?.Invoke();
        }
    }
}