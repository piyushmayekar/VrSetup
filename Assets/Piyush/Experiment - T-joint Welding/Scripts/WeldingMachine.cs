using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace TWelding
{
    public class WeldingMachine : MonoBehaviour
    {
        public event Action OnElectrodePlacedEvent;
        [SerializeField] Animator _AC;
        [SerializeField] bool isSqueezerTouched = false;
        [SerializeField] Collider squeezerCollider;
        [SerializeField] ParticleSystem ps;
        [SerializeField] GameObject sparkLight;
        [SerializeField] bool isOn = false, isElectrodePlaced = false, isTipInContact = false, isElectrodeAtLeft = false;
        [SerializeField] GameObject tip;
        [SerializeField] GameObject fauxElectrode;
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] GameObject indicator;
        [SerializeField] TMPro.TextMeshProUGUI errorText;
        [SerializeField] ElectrodeType requiredElectrodeType;
        [SerializeField] Electrode currentElectrode;
        [SerializeField] XRSocketInteractor socket_Electrode;
        Rigidbody _rb;
        Outline squeezerOutline;

        Vector3 _resetPos = new Vector3(-0.898199975f, 1.12199998f, -1.28900003f), _resetRot = new Vector3(-45, 0f, 0f);

        public bool IsElectrodePlaced { get => isElectrodePlaced; set => isElectrodePlaced = value; }
        #region SINGLETON
        public static WeldingMachine Instance => instance;

        public ElectrodeType RequiredElectrodeType { get => requiredElectrodeType; set => requiredElectrodeType = value; }
        public bool IsElectrodeAtLeft { get => isElectrodeAtLeft; set => isElectrodeAtLeft = value; }
        public bool IsSqueezerTouched { get => isSqueezerTouched; set => isSqueezerTouched = value; }
        static WeldingMachine instance = null;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null && instance != this)
                Destroy(gameObject);
        }
        #endregion
        public void OnGunHoldStart(SelectEnterEventArgs args)
        {

            // Debug.Log("here socket" + socket_Electrode.socketActive + " squeezerT:" + IsSqueezerTouched);
            //If squeezer is touched & is not holding an electrode, 
            //open the gun & set the e socket active
            if (IsSqueezerTouched)
            {
                IsSqueezerTouched = false;
                ToggleGunSqueezers(true);
                squeezerCollider.enabled = false;
                // Debug.Log("setting " + (!IsHoldingElectrode));
                socket_Electrode.socketActive = currentElectrode == null;

            }
            // Debug.Log("-- " + socket_Electrode.socketActive + " squeezerT:" + IsSqueezerTouched);
            //If squeezer is touched & is holding an electrode, 
            //open the gun & set the e socket inactive, so the electrode drops
        }

        public void OnGunHoldEnd(SelectExitEventArgs args)
        {
            IsSqueezerTouched = false;
            squeezerOutline.enabled = false;
            ToggleGunSqueezers(IsSqueezerTouched);
            Invoke(nameof(EnableSqueezerCollider), 1f);
            ResetTransform();
        }

        public void ResetTransform()
        {
            transform.SetPositionAndRotation(_resetPos, Quaternion.Euler(_resetRot));
        }

        void EnableSqueezerCollider()
        {
            squeezerCollider.enabled = true;
            IsSqueezerTouched = false;
        }

        public void OnSqueezerHoldStart(HoverEnterEventArgs args)
        {
            if (args.interactor)
            {
                IsSqueezerTouched = true;
                squeezerOutline.enabled = true;
            }
        }

        void ToggleGunSqueezers(bool open)
        {
            _AC.SetBool("Open", open);
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            ps = GetComponentInChildren<ParticleSystem>();
            sparkLight = ps.transform.GetChild(0).gameObject;
            squeezerOutline = squeezerCollider.transform.GetComponentInParent<Outline>();
            ToggleMachine(false);
            ToggleWeldingTip();
            WeldingArea.OnWeldingMachineTipInContact += (bool isTipAtLeft) =>
            {
                IsElectrodeAtLeft = isTipAtLeft;
                TipInContact(true);
            };
            WeldingArea.OnWeldingMachineTipLoseContact += () =>
            {
                TipInContact(false);
            };
            ResetTransform();
        }

        public void TipInContact(bool inContact)
        {
            isTipInContact = inContact;
            ToggleMachine(inContact);
        }

        public void ToggleMachine(bool on)
        {
            isOn = on;
            if (on && isTipInContact && IsElectrodePlaced)
            {
                ps.Play();
                sparkLight.SetActive(true);
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);
            }
            else
            {
                ps.Stop();
                sparkLight.SetActive(false);
                soundPlayer.StopPlayingAllSounds();
            }
        }
        /// <summary>
        /// Called when a task begins.
        /// Sets IsElectrodePlaced to false if the desired electrode type is not currently placed
        /// So as to not turn it on unnecessarily on contact.
        /// </summary>
        /// <param name="type"></param>
        public bool CheckIfRequiredElectrodePlaced(ElectrodeType type)
        {
            IsElectrodePlaced = currentElectrode != null
                && currentElectrode.ElectrodeType == type;
            return IsElectrodePlaced;
        }

        public void OnElectrodePlaced(SelectEnterEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.ELECTRODE_TAG))
            {
                _rb.velocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
                Electrode electrode = args.interactable.GetComponent<Electrode>();
                if (electrode.ElectrodeType == RequiredElectrodeType)
                {
                    IsElectrodePlaced = true;
                    OnElectrodePlacedEvent?.Invoke();
                }
                currentElectrode = electrode;
                fauxElectrode.SetActive(true);
                currentElectrode.GetComponent<MeshRenderer>().enabled = false;
                Invoke(nameof(TurnOffElectrodeGrab), 1f);
                ToggleWeldingTip();
                ShowErrorIndicator(electrode.ElectrodeType != RequiredElectrodeType, _Constants.ELECTRODE_NOT_CORRECT);
            }
        }

        public void TurnOnElectrodeGrab()
        {
            if (currentElectrode != null)
                currentElectrode.GetComponent<XRGrabInteractable>().colliders[0].enabled = true;
        }

        public void TurnOffElectrodeGrab()
        {
            if (currentElectrode != null)
                currentElectrode.GetComponent<XRGrabInteractable>().colliders[0].enabled = false;
        }

        public void OnElectrodeSocketExit(SelectExitEventArgs args)
        {
            if (args.interactable.CompareTag(_Constants.ELECTRODE_TAG))
            {
                fauxElectrode.SetActive(false);
                if (currentElectrode != null)
                {
                    currentElectrode.GetComponent<MeshRenderer>().enabled = true;
                    Rigidbody rb = currentElectrode.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    XRGrabInteractable grab = args.interactable as XRGrabInteractable;
                    grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
                    grab.colliders.ForEach(collider =>
                    {
                        collider.isTrigger = false;
                        collider.enabled = true;
                    });
                }
                currentElectrode = null;
                IsElectrodePlaced = false;
            }
        }
        void TurnOnElectrodeSocket()
        {
            socket_Electrode.socketActive = true;
        }
        /// <summary>
        /// Only turn on welding tip if the appropriate electrode is placed
        /// </summary>
        private void ToggleWeldingTip()
        {
            tip.SetActive(IsElectrodePlaced);
        }

        public void ShowErrorIndicator(bool show, string message = null)
        {
            CancelInvoke(nameof(DisableIndicator));
            if (indicator)
            {
                indicator.SetActive(show);
                if (show) errorText.text = message;
                if (show)
                {
                    Invoke(nameof(DisableIndicator), 2f);
                }
            }
        }
        void DisableIndicator() => ShowErrorIndicator(false);

    }
}