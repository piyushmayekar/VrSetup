using PiyushUtils;
using System.Collections;
using UnityEngine;

namespace VWelding
{
    public class Task_FilingJob : Task
    {
        [SerializeField] int platesCount = 0;
        [SerializeField] GameObject requiredPlatePrefab, bevelledPlatePrefab;
        SMAW_MechanicalVise mechanicalVise;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            mechanicalVise = FindObjectOfType<SMAW_MechanicalVise>();
            mechanicalVise.ToggleFilingCanvas(true);
            SMAWJobPlate.jobPlates.ForEach(plate => plate.OnFilingDone += ()=>{
                OnJobFilingDone(plate as JobPlate);
                    });
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
                mechanicalVise.OnTaskCompleted();
                OnTaskCompleted();
            }
            Invoke(nameof(TurnOnViseSwitch), 1f);
        }

        void TurnOnViseSwitch() => mechanicalVise.ShouldSwitchAttachTransforms = true;
    }
}