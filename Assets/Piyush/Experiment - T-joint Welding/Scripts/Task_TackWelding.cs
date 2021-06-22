using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using FlatWelding;
using UnityEngine;
using UnityEngine.UI;

namespace TWelding
{
    public class Task_TackWelding : Task
    {
        [SerializeField] WeldingMachine machine;
        [SerializeField] ElectrodeType requireElectrodeType = ElectrodeType._315mm;
        [SerializeField] XRGrabInteractable jobPlatesGrabInteractable;
        [SerializeField] List<WeldingPoint> weldingPoints;
        [SerializeField] int weldingDoneOnPoints = 0;
        [SerializeField] GameObject button;
        [SerializeField] string _buttonText = "Done";
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingPoints[0].transform.parent.gameObject.SetActive(true);
            weldingPoints.ForEach(point => point.OnWeldingDone += () =>
            {
                weldingDoneOnPoints++;
                if (weldingDoneOnPoints >= weldingPoints.Count)
                {
                    EnableDoneButton();
                    jobPlatesGrabInteractable.enabled = true;
                }
            });
            machine.CheckIfRequiredElectrodePlaced(requireElectrodeType);
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
            OnTaskCompleted();
            button.SetActive(false);
        }

        public override void OnTaskCompleted()
        {
            button.gameObject.SetActive(false);
            base.OnTaskCompleted();
        }
    }
}