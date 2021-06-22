using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class Task_Measurement : Task
    {
        [SerializeField] GameObject button;
        [SerializeField] string _buttonText = "Done";
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            Invoke(nameof(EnableDoneButton), 2f);
        }

        private void EnableDoneButton()
        {
            button.gameObject.SetActive(true);
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = _buttonText;
            XRGrabInteractable interactable = button.GetComponent<XRGrabInteractable>();
            interactable.firstHoverEntered.RemoveAllListeners();
            interactable.firstHoverEntered.AddListener(new UnityEngine.Events.UnityAction<HoverEnterEventArgs>(OnButtonClicked));
        }

        public void OnButtonClicked(HoverEnterEventArgs arg)
        {
            button.SetActive(false);
            OnTaskCompleted();
        }

    }
}