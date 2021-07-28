using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    public class Hacksaw : MonoBehaviour
    {
        Rigidbody _rb;

        #region SINGLETON

        static Hacksaw instance = null;
        public static Hacksaw Instance => instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void ToggleFreezeRotation(bool freeze=true)
        {
            _rb.freezeRotation = freeze;
        }

    }
}