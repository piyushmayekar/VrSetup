using System.Collections;
using System.Collections.Generic;
using TWelding;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class CuttingTorch : MonoBehaviour
    {
        public UnityEvent TorchOnStateChanged;

        [SerializeField] bool isFlameOn = false;
        [SerializeField] int index_Flame = 0;
        [SerializeField] List<GameObject> flames;
        [SerializeField] List<Vector3> flameScales;
        [SerializeField] Transform leverT;
        [SerializeField] float leverRotationLerpSpeed = 0.3f;
        [SerializeField] List<Vector3> levelRotations;
        [SerializeField] bool isInCuttingArea=false, detectLighter = false;
        [SerializeField] AudioSource audioSource;
        Rigidbody rb;
        XRGrabInteractable grab;
        Vector3 cuttingRotation;
        public bool IsFlameOn { get => isFlameOn; private set => isFlameOn = value; }

        #region SINGLETON
        static CuttingTorch instance = null;

        public static CuttingTorch Instance => instance;

        private void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(this);
        }
        #endregion

        void Start()
        {
            grab = GetComponent<XRGrabInteractable>();
            rb = GetComponent<Rigidbody>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains(_Constants.LIGHTER_TAG) && detectLighter)
            {
                if (!IsFlameOn)
                    ToggleSwitch();
            }
            if (other.CompareTag(_Constants.CUTTINGPOINT_TAG) && IsFlameOn)
            {
                other.GetComponent<CuttingPoint>().OnFlameInContact();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_Constants.CUTTINGPOINT_TAG) && IsFlameOn)
            {
                other.GetComponent<CuttingPoint>().OnFlameOutOfContact();
            }
        }

        public void ToggleLighterDetection(bool on = true)
        {
            detectLighter = on;
        }

        public void ToggleSwitch(bool on = true)
        {
            IsFlameOn = on;
            SwitchToFlame(on ? index_Flame : -1);
            TorchOnStateChanged?.Invoke();
            if (on) audioSource.Play();
            else audioSource.Stop();
        }

        public void SwitchToFlame(int index)
        {
            flames.ForEach(flame => flame.SetActive(false));
            if (index >= 0 && index < flames.Count)
            {
                index_Flame = index;
                flames[index].SetActive(true);
            }
        }


        public void OnTorchEnterCuttingArea()
        {
            isInCuttingArea = true;
            grab.trackRotation = false;
            cuttingRotation = transform.eulerAngles;
            flames[index_Flame].transform.localScale = flameScales[1];
            StartCoroutine(RotationSetter());
            StopCoroutine(LeverAnimator());
            StartCoroutine(LeverAnimator(true));
        }

        public void OnTorchExitCuttingArea()
        {
            isInCuttingArea = false;
            grab.trackRotation = true;
            flames[index_Flame].transform.localScale = flameScales[0];
            StopCoroutine(LeverAnimator());
            StartCoroutine(LeverAnimator(false));
        }

        IEnumerator RotationSetter()
        {
            while (isInCuttingArea)
            {
                transform.rotation = Quaternion.Euler(cuttingRotation);
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator LeverAnimator(bool pressed=true)
        {
            float t = 0f;
            Vector3 initRot = !pressed ? levelRotations[1] : levelRotations[0];
            Vector3 finalRot = !pressed ? levelRotations[0] : levelRotations[1];
            while (t<=1f)
            {
                t += leverRotationLerpSpeed;
                leverT.localEulerAngles = Vector3.Lerp(initRot, finalRot, t);
                yield return new WaitForEndOfFrame();
            }
        }

    }
}