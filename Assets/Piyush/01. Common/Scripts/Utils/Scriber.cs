using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    public class Scriber : MonoBehaviour
    {
        #region SINGLETON
        static Scriber instance = null;

        public static Scriber Instance { get => instance; }

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
            {
                Destroy(this);
            }
        }
        #endregion
        Rigidbody rb;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void OnMarkingAreaEnter()
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        public void OnMarkingAreaExit()
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}