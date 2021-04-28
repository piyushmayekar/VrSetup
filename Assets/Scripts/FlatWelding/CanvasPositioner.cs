using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace FlatWelding
{
    public class CanvasPositioner : MonoBehaviour
    {
        [SerializeField] Transform xrRigT, displayer;
        [SerializeField] float yHeight = 1f, lerpSpeed = 0.1f;
        [SerializeField] Vector3 finalPos;

        LayerMask wall;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            wall = LayerMask.GetMask("Wall");
        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        IEnumerator Start()
        {
            while (true)
            {
                if (Physics.Raycast(xrRigT.position, xrRigT.forward, out RaycastHit hit, 20f, wall))
                {
                    finalPos = hit.point + Vector3.up * yHeight;
                    displayer.rotation = Quaternion.LookRotation(-Vector3.forward);
                }
                displayer.position = Vector3.Lerp(displayer.position, finalPos, lerpSpeed);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}