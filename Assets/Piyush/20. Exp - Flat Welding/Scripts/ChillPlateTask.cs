using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class ChillPlateTask : Task
    {

        [SerializeField, Tooltip("List of all the chill plates")] List<GameObject> chillPlates;
        [SerializeField, Tooltip("Keeping track of the the plate placed in the desired socket")] List<bool> platesPlaced;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            chillPlates.ForEach(plate => platesPlaced.Add(false));
        }


        public void OnChillPlateSelect(SelectEnterEventArgs args)
        {
            int index = chillPlates.FindIndex(plate => plate.gameObject == args.interactable.gameObject);
            if (index >= 0)
            {
                platesPlaced[index] = true;
                if (platesPlaced.TrueForAll(platePlaced => platePlaced))
                    OnTaskCompleted();
            }
        }
    }
}