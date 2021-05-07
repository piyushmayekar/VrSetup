using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class MarkingPoint : MonoBehaviour
    {
        public Action OnMarkingDone;

        [SerializeField] Renderer _renderer;
        [SerializeField] List<GameObject> highlights;

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MarkPoint"))
            {
                GetComponent<Collider>().enabled = false;
                _renderer.enabled = true;
                highlights.ForEach(x => x.SetActive(false));
                OnMarkingDone?.Invoke();
            }
        }
    }
}