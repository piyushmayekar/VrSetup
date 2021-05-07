using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class PunchMarkingPoint : MonoBehaviour
    {
        [SerializeField] Task_CenterPunch task;
        [SerializeField] List<GameObject> highlights;
        [SerializeField] bool isCenterPunchInside = false;

        public bool IsCenterPunchInside { get => isCenterPunchInside; set => isCenterPunchInside = value; }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DotPoint"))
            {
                IsCenterPunchInside = true;
            }
        }

        /// <summary>
        /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("DotPoint"))
            {
                IsCenterPunchInside = false;
            }
        }

        public void MarkingDone()
        {
            highlights.ForEach(x => x.SetActive(false));
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}