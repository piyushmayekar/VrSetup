using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VWelding
{
    public class Task_Hacksaw : Task
    {
        [SerializeField] GameObject requiredPlatePrefab, bevelledPlatePrefab;
        [SerializeField] int platesCount = 0;
        [SerializeField] MechanicalVise mechanicalVise;
        // [SerializeField] bool firstIteration = true;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            IsTaskComplete = false;
            platesCount = 0;
            mechanicalVise.OnTaskBegin();
            JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += () =>
            {
                mechanicalVise.ShouldSwitchAttachTransforms = false;
                GameObject prefabToSpawn = null;
                if (plate.PlateType == PlateType.Length || plate.PlateType == PlateType.Breadth)
                    prefabToSpawn = requiredPlatePrefab;
                else if (plate.PlateType == PlateType.Required)
                    prefabToSpawn = bevelledPlatePrefab;
                GameObject iPlate = Instantiate(prefabToSpawn, plate.transform.position, plate.transform.rotation);
                plate.gameObject.SetActive(false);
                platesCount++;
                JobPlate.jobPlates.Remove(plate);
                if (platesCount == JobPlate.jobPlates.Count)
                {
                    // if (firstIteration)
                    // {
                    //     firstIteration = false;
                    //     FlatWelding.TaskManager.Instance.CurrentTaskIndex = 1;
                    // }
                    OnTaskCompleted();
                }
                Invoke(nameof(TurnOnViseSwitch), 1f);
            });
        }

        void TurnOnViseSwitch() => mechanicalVise.ShouldSwitchAttachTransforms = true;

        public override void OnTaskCompleted()
        {
            base.OnTaskCompleted();
            mechanicalVise.OnTaskCompleted();
        }

    }
}