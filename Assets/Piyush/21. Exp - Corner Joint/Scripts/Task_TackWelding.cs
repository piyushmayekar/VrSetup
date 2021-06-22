using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using FlatWelding;
using UnityEngine;
using UnityEngine.UI;

namespace CornerWelding
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
        [SerializeField] bool keepingTrackOfSpacersRemoval=false;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            keepingTrackOfSpacersRemoval=false;
            weldingPoints[0].transform.parent.gameObject.SetActive(true);
            machine.RequiredElectrodeType = requireElectrodeType;
            weldingPoints.ForEach(point => point.OnWeldingDone += (point) =>
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
                    keepingTrackOfSpacersRemoval=true;
                }
            });
        }

        public void OnSpacerRemoved(GameObject spacer)
        {
            if(keepingTrackOfSpacersRemoval){
            if (spacersToRemove.Contains(spacer))
                spacersToRemove.Remove(spacer);
            spacer.SetActive(false);
            if (spacersToRemove.Count == 0)
                OnTaskCompleted();
        }
    }
    }
}