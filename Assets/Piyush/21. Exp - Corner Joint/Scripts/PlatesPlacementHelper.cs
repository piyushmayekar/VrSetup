using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

namespace Semester2
{
    public class PlatesPlacementHelper : MonoBehaviour
    {
        public UnityEvent OnPlatesPlacedEvent;

        [SerializeField] XRSocketInteractor placementSocket;
        const string attachedPlates = "Attached Plates";
        
        private void OnTriggerExit(Collider other)
        {
            if (other.name.Contains(attachedPlates))
            {
                placementSocket.enabled = true;
                placementSocket.selectEntered.AddListener(OnPlatesPlaced);
            }
        }

        void OnPlatesPlaced(SelectEnterEventArgs args)
        {
            GameObject platesFused = args.interactable.gameObject;
            if (platesFused.name.Contains(attachedPlates))
            {
                placementSocket.selectEntered.RemoveAllListeners();
                platesFused.transform.SetPositionAndRotation(placementSocket.attachTransform.position, placementSocket.attachTransform.rotation);
                OnPlatesPlacedEvent?.Invoke();
            }
        }
    }
}