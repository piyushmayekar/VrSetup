using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils 
{
    public class Tablet : MonoBehaviour
    {
        [SerializeField] public TMPro.TextMeshProUGUI taskDetailText;
        [SerializeField] public UnityEngine.UI.Button confirmButton;
        [SerializeField] bool onState = true;
        [SerializeField] GameObject tablet;

        public void ToggleTabletOn()
        {
            onState = !onState;
            tablet.SetActive(onState);
        }
    }
}