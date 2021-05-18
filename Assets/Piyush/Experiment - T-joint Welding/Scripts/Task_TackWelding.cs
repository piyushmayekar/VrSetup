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
        [SerializeField] List<GameObject> finalSpacers;
        [SerializeField] GameObject spacerGrabbablePrefab;
        [SerializeField] List<GameObject> spacersToRemove = new List<GameObject>();
        [SerializeField] int weldingDoneOnPoints = 0;
        [SerializeField] Button doneButton;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            weldingPoints[0].transform.parent.gameObject.SetActive(true);
            weldingPoints.ForEach(point => point.OnWeldingDone += () =>
            {
                weldingDoneOnPoints++;
                if (weldingDoneOnPoints >= weldingPoints.Count)
                {
                    for (var i = 0; i < finalSpacers.Count; i++)
                    {
                        GameObject spacer = finalSpacers[i];
                        spacersToRemove.Add(Instantiate(spacerGrabbablePrefab, spacer.transform.position,
                        spacer.transform.rotation, spacer.transform.parent));
                        Destroy(spacer);
                    }
                }
            });
            machine.CheckIfRequiredElectrodePlaced(requireElectrodeType);
        }

        public override void OnTaskCompleted()
        {
            doneButton.gameObject.SetActive(false);
            base.OnTaskCompleted();
        }
    }
}