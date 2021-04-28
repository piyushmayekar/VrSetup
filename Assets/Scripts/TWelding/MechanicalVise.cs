using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class MechanicalVise : MonoBehaviour
    {
        [SerializeField] Transform handle, slider;
        [SerializeField] float sliderParameter;

        [SerializeField, Tooltip("The angle limits of the handle with respect to Left World Axis")]
        Vector2 angleLimits = new Vector2(-125f, 125f);

        [SerializeField, Tooltip("Slider positions")] Vector3 openPos, closedPos;

        public void OnHandleSelectEnter() => StartCoroutine(TrackHandleRotation());
        public void OnHandleSelectExit() => StopCoroutine(TrackHandleRotation());

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            slider.localPosition = openPos;
        }

        IEnumerator TrackHandleRotation()
        {
            while (true)
            {
                float angle = Vector3.SignedAngle(Vector3.forward, handle.right, Vector3.left);
                sliderParameter = Mathf.InverseLerp(angleLimits.x, angleLimits.y, angle);
                slider.transform.localPosition = Vector3.Lerp(openPos, closedPos, sliderParameter);
                yield return null;
            }

        }

        public void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag("Job"))
            {
                args.interactable.GetComponent<JobPlate>().SetupForCutting();
            }
        }

        public void OnSocketExit(SelectEnterEventArgs args)
        {

        }
    }
}