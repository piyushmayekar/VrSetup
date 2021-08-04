using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PiyushUtils 
{
    public class Tablet : MonoBehaviour
    {
        public UnityEvent OnLanguageButtonClick;

        [SerializeField] public TMP_FontAsset[] languagesFont;
        [SerializeField] public TextMeshProUGUI taskDetailText;
        [SerializeField] public UnityEngine.UI.Button confirmButton;
        [SerializeField] bool onState = true;
        [SerializeField] GameObject tablet;

        public void ToggleTabletOn()
        {
            onState = !onState;
            tablet.SetActive(onState);
        }

        public void OnLanguageButtonClicked()
        {
            OnLanguageButtonClick?.Invoke();
        }
    }
}