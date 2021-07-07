using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grinding
{
    public class GrindingPlate : MonoBehaviour
    {
        public Action<GrindingWheelType> OnGrindingComplete;
        [SerializeField] WeldedPlates weldedPlates;
        [SerializeField, Tooltip("x:min, y:max")] Vector2 scaleLimits;
        [SerializeField] float grindSpeed = 1f, surfaceGrindingTimer = 3f;
        [SerializeField] GameObject roughEdge;
        [SerializeField, Tooltip("1: rough grinding done, 2: surface grinding done")]
        int grindingStage = 0;
        [SerializeField] int maxGrindingStage = 2;
        [SerializeField] Vector3 _localScale;
        Coroutine _grinderCoroutine = null;

        void OnTriggerEnter(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (grindingStage >= maxGrindingStage) return;
            if (wheel && wheel.IsWheelSpinning && grindingStage == (int)GrindingWheelType.Rough)
            {
                _grinderCoroutine = StartCoroutine(RoughGrinder());
                if (roughEdge)
                    roughEdge.SetActive(true);
            }
            if (wheel && wheel.IsWheelSpinning && grindingStage == (int)GrindingWheelType.Surface)
            {
                _grinderCoroutine = StartCoroutine(SurfaceGrinder());
            }
        }

        void OnTriggerExit(Collider other)
        {
            GrinderWheel wheel = other.GetComponent<GrinderWheel>();
            if (wheel && wheel.IsWheelSpinning)
            {
                StopAllCoroutines();
            }
        }

        IEnumerator RoughGrinder()
        {
            _localScale = transform.localScale;
            while (_localScale.z > scaleLimits.x)
            {
                _localScale = transform.localScale;
                _localScale.z -= grindSpeed * Time.deltaTime;
                transform.localScale = _localScale;
                yield return new WaitForEndOfFrame();
            }
            if (_localScale.z <= scaleLimits.x)
            {
                grindingStage = 1;
                if (_grinderCoroutine != null)
                    StopCoroutine(_grinderCoroutine);
                _grinderCoroutine = null;
                OnGrindingComplete?.Invoke(GrindingWheelType.Rough);
            }
        }

        IEnumerator SurfaceGrinder()
        {
            while (surfaceGrindingTimer > 0f)
            {
                surfaceGrindingTimer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (surfaceGrindingTimer <= 0f)
            {
                OnGrindingComplete?.Invoke(GrindingWheelType.Surface);
                grindingStage = 2;
                if (roughEdge)
                    roughEdge.SetActive(false);
            }
        }
    }
}