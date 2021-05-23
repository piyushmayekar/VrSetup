using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWelding
{
    public class FinalJobPlatesManager : MonoBehaviour
    {
        public static Action<GameObject> OnSpacerRemoved;
        [SerializeField] Vector3 initPos;
        [SerializeField] Quaternion initRot;
        void Start()
        {
            initPos = transform.position;
            initRot = transform.rotation;
        }

        public void OnSelectExit()
        {
            transform.SetPositionAndRotation(initPos, initRot);
        }

        /// <summary>
        /// OnTriggerEnter is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other">The other Collider involved in this collision.</param>
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.SPACER_TAG))
                OnSpacerRemoved?.Invoke(other.gameObject);
        }
    }
}