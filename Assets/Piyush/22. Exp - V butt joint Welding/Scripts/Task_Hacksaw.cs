using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
{
    public class Task_Hacksaw : Task
    {
        [SerializeField] List<JobPlate> jobPlates;
        [SerializeField] GameObject requiredPlatePrefab, bevelledPlatePrefab;
        [SerializeField] int platesCount = 0;
        [SerializeField] MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            IsTaskComplete = false;
            mechanicalVise.OnTaskBegin();
            jobPlates = new List<JobPlate>(FindObjectsOfType<JobPlate>());
            jobPlates.ForEach(plate => plate.OnFilingDone += () =>
            {
                OnJobFilingDone(plate);
            }
            );
        }

        void OnJobFilingDone(JobPlate plate)
        {
            mechanicalVise.ShouldSwitchAttachTransforms = false;
            plate.gameObject.SetActive(false);
            GameObject prefabToSpawn = null;
            if (plate.PlateType == PlateType.Length || plate.PlateType == PlateType.Breadth)
                prefabToSpawn = requiredPlatePrefab;
            else if (plate.PlateType == PlateType.Required)
                prefabToSpawn = bevelledPlatePrefab;
            GameObject iPlate = Instantiate(prefabToSpawn, plate.transform.position, plate.transform.rotation);
            platesCount++;
            JobPlate.jobPlates.Remove(plate);

            if (platesCount == JobPlate.jobPlates.Count)
            {
                OnTaskCompleted();
            }
            Invoke(nameof(TurnOnViseSwitch), 1f);
        }

        void TurnOnViseSwitch() => mechanicalVise.ShouldSwitchAttachTransforms = true;

        public override void OnTaskCompleted()
        {
            base.OnTaskCompleted();
            mechanicalVise.OnTaskCompleted();
        }

    }
}