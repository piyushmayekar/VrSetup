using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Semester2
{
    public class Task_FlipThePlates : Task
    {
        [SerializeField] PlatesPlacementHelper placementHelper;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            placementHelper.gameObject.SetActive(true);
            //Wait for the plates to exit the placement area ie. for the user to lift up the plates, & then turn on the socket which will help in flipping.
            placementHelper.OnPlatesPlacedEvent.AddListener(OnPlatesPlaced);
        }

        void OnPlatesPlaced()
        {
            placementHelper.OnPlatesPlacedEvent.RemoveListener(OnPlatesPlaced);
            OnTaskCompleted();
        }
    }
}