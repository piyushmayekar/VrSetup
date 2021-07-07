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
            // platesCount = 0;
            mechanicalVise.OnTaskBegin();
            // for (int i = 0; i < JobPlate.jobPlates.Count; i++)
            //     Debug.Log(i + JobPlate.jobPlates[i].name);
            JobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += () =>
            {
                OnJobFilingDone(plate);
            }
            );
            // Debug.Log(" count: " + JobPlate.jobPlates.Count);
        }

        void OnJobFilingDone(JobPlate plate)
        {
            // Debug.Log("here " + plate.name + " count: " + platesCount);
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
                OnTaskCompleted();
            }
            Invoke(nameof(TurnOnViseSwitch), 1f);
            string count = "here " + plate.name + " count: " + platesCount;
        }

        void TurnOnViseSwitch() => mechanicalVise.ShouldSwitchAttachTransforms = true;

        public override void OnTaskCompleted()
        {
            base.OnTaskCompleted();
            mechanicalVise.OnTaskCompleted();
        }

    }
}