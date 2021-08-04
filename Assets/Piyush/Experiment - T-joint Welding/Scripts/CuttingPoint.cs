using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TWelding
{
    public class CuttingPoint : MonoBehaviour
    {
        public UnityEvent<CuttingPoint> OnCuttingDone;
        [SerializeField] Collider _collider;
        [SerializeField] internal float timer = 1f;
        [SerializeField] bool flameInContact = false, disableAllowed = false;
        [SerializeField] Gradient gradient;

        MeshRenderer _mesh;
        Coroutine corTimer = null;
        int index = 0;
        float maxTimer = 0f;

        public bool DisableAllowed { get => disableAllowed; set => disableAllowed = value; }

        private void Start()
        {
            index = transform.GetSiblingIndex();
            maxTimer = timer;
            _mesh = GetComponent<MeshRenderer>();
        }

        public void OnFlameInContact()
        {
            flameInContact = true;
            StartTimerIfFlameIsInContact();
        }

        public void OnFlameOutOfContact()
        {
            flameInContact = false;
            corTimer = null;
        }

        public void StartTimerIfFlameIsInContact()
        {
            if (flameInContact && corTimer==null)
               corTimer = StartCoroutine(Timer());
        }

        IEnumerator Timer()
        {
            while (flameInContact && timer > 0f)
            {
                timer -= Time.deltaTime;
                _mesh.material.color = gradient.Evaluate(Mathf.InverseLerp(maxTimer, 0f, timer));
                yield return new WaitForEndOfFrame();
            }
            if (timer <= 0f && DisableAllowed)
                CuttingDone();
        }

        void CuttingDone()
        {
            OnCuttingDone?.Invoke(this);
        }


    }
}