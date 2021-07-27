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
        [SerializeField] Button button;
        [SerializeField] string _buttonText = "Done";
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            button = PiyushUtils.TaskManager.Instance.confirmButton;
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
            button.onClick.AddListener(new UnityEngine.Events.UnityAction(OnButtonClicked));
        }

        public void OnButtonClicked()
        {
            OnTaskCompleted();
            button.gameObject.SetActive(false);
        }

        public override void OnTaskCompleted()
        {
            button.gameObject.SetActive(false);
            base.OnTaskCompleted();
        }
    }
}