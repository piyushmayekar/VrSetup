using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PiyushUtils
{
    public class PlateMeasurement : MonoBehaviour
    {
        public UnityEvent OnMeasurementDone;

        [SerializeField] List<SteelRuleHighlight> steelRuleHighlights;
        [SerializeField] int index = 0;
        [SerializeField] Collider _collider;

        private void Start()
        {
            steelRuleHighlights = new List<SteelRuleHighlight>(GetComponentsInChildren<SteelRuleHighlight>(true));
            _collider = GetComponent<Collider>();
            _collider.enabled = false;
        }
        public void StartMeasurement()
        {
            if (index < steelRuleHighlights.Count)
            {
                steelRuleHighlights[index].gameObject.SetActive(true);
                _collider.enabled = false;
                steelRuleHighlights[index].OnSteelRuleSelectEnter.AddListener(() =>
                {
                    steelRuleHighlights[index].OnSteelRuleSelectEnter.RemoveAllListeners();
                    _collider.enabled = true;
                    steelRuleHighlights[index].TurnOnRulerGrab();
                });
            }
        }


        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.STEEL_RULER_TAG))
            {
                index++;
                if (index >= steelRuleHighlights.Count)
                { 
                    OnMeasurementDone?.Invoke();
                    gameObject.SetActive(false);
                }
                else
                    StartMeasurement();
            }
        }

    }
}