using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace PiyushUtils
{
    public class UserMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        public InputDeviceCharacteristics controllerCharacteristics;
        [SerializeField] Vector2 movementIP;
        [SerializeField] CharacterController characterController;
        [SerializeField] Transform cameraT;
        InputDevice targetDevice;

        private void Start()
        {
            TryInitialize();
        }

        private void Update()
        {
            if (!targetDevice.isValid) TryInitialize();
            else
            {
                targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out movementIP);
                characterController.SimpleMove(new Vector3(movementIP.x, 0f, movementIP.y) * moveSpeed * Time.deltaTime);

            }
        }

        void TryInitialize() 
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices); //Get the devices with char like left controller
            if (devices.Count > 0)
            {
                targetDevice = devices[0];
            }
        }
    }
}