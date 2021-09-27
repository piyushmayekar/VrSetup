using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2.Exp19
{
    public class Task_RemoveSpacers : Task
    {
        [SerializeField] int plateIndex = 0;
        [SerializeField] List<FusedPlates> fusedPlatesList;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            fusedPlatesList.ForEach(fusedPlates =>
            {
                fusedPlates.StartSpacersRemoval();
                fusedPlates.OnSpacersRemoved.AddListener(OnTaskDone);
            });
        }

        void OnTaskDone()
        {
            plateIndex++;
            if (plateIndex >= fusedPlatesList.Count)
                OnTaskCompleted();
        }

        [ContextMenu(nameof(SkipToThisStep))]
        public void SkipToThisStep()
        {
            fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            fusedPlatesList.ForEach(fusedPlate =>
            {
                fusedPlate.finalPlates.ForEach(plate => plate.SetActive(true));
                fusedPlate.finalSpacers.ForEach(spacer => spacer.SetActive(true));
            });
        }
    }
}