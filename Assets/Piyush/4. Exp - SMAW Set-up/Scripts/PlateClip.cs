using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMAW_Setup_4
{
    public class PlateClip : MonoBehaviour
    {
        [SerializeField] Transform unattachedPosHolder, attachedPosHolder;

        private void Start()
        {
            SetPositionToTransform(unattachedPosHolder);
        }

        void SetPositionToTransform(Transform t)
        {
            transform.SetPositionAndRotation(t.position, t.rotation);
        }
    }
}