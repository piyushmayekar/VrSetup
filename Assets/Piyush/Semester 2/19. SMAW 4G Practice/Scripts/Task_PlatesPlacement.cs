using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2
{
    public class Task_PlatesPlacement : Task
    {
        [SerializeField] int plateIndex = -1;
        [SerializeField] List<FusedPlates> fusedPlatesList;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            fusedPlatesList = new List<FusedPlates>(FindObjectsOfType<FusedPlates>(true));
            OnSpacerPlaced();
        }

        void OnSpacerPlaced()
        {
            plateIndex++;
            if (plateIndex >= fusedPlatesList.Count)
                OnTaskCompleted();
            else
            { 
                fusedPlatesList[plateIndex].StartPlatePlacement();
                fusedPlatesList[plateIndex].OnSpacersPlaced.AddListener(OnSpacerPlaced);
            }
        }
    }
}