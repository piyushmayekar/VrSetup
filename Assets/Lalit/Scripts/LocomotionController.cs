using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{

    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;
    public bool enableLeftTeleport { get; set; } = true;
    public bool enableRightTeleport { get; set; } = true;

    void Update()
    {
        Vector3 pos;
        Vector3 norm;
        int index;
        bool validTarget;

        if (leftTeleportRay && enableLeftTeleport)
        {
            bool isleftRayInteractorHovering = leftInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay) && !isleftRayInteractorHovering);
        }

        if (rightTeleportRay && enableRightTeleport)
        {
            bool isRightRayInteractorHovering = rightInteractorRay.TryGetHitInfo(out pos, out norm, out index, out validTarget);

            rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay) && !isRightRayInteractorHovering);
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
