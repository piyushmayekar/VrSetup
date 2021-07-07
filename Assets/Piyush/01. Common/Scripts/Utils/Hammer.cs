using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{
    public class Hammer : MonoBehaviour
    {
        #region SINGLETON
        static Hammer instance = null;

        public static Hammer Instance { get => instance; }

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

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void RemoveVelocity()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }
}