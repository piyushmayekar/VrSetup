using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class FlatFile : MonoBehaviour
    {
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] string toolAnimToPlayOnSecondHandPoint = "Flat File Top";
        [SerializeField] CustomXRGrabInteractable grabInteractable;
        /// <summary>
        /// OnCollisionEnter is called when this collider/rigidbody has begun
        /// touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains(_Constants.FILING_POINT_TAG))
                soundPlayer.PlayClip(soundPlayer.Clips[0], true);

        }

        /// <summary>
        /// OnCollisionExit is called when this collider/rigidbody has
        /// stopped touching another rigidbody/collider.
        /// </summary>
        /// <param name="other">The Collision data associated with this collision.</param>
        void OnCollisionExit(Collision other)
        {
            soundPlayer.StopPlayingAllSounds();
        }

        public void OnSecondGrabPointHoverEnter(HoverEnterEventArgs args)
        {
            HandPresence handPresence = args.interactor.GetComponentInChildren<HandPresence>();
            if (handPresence && grabInteractable.isSelected)
            {
                handPresence.OnSecondHandPointHoverEnter(toolAnimToPlayOnSecondHandPoint);
            }
        }

        public void OnSecondGrabPointHoverExit(HoverExitEventArgs args)
        {
            HandPresence handPresence = args.interactor.GetComponentInChildren<HandPresence>();
            if (handPresence)
            {
                handPresence.OnSecondHandPointHoverExit();
            }
        }
    }
}