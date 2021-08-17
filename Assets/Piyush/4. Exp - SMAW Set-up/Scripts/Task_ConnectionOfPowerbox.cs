using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SMAW_Setup_4
{
    public class Task_ConnectionOfPowerbox : Task_AttachObjects
    {
        [SerializeField] Transform switchT;
        [SerializeField] XRSimpleInteractable interactable_Switch;
        [SerializeField] Outline outline_Switch;
        [SerializeField] Animator anim_Switch;
        [SerializeField] Vector3 finalSwitchRotation;
        [SerializeField] float rotationSpeed = 1f;

        public override void OnTaskCompleted()
        {
            outline_Switch.enabled = true;
            interactable_Switch.selectEntered.AddListener(OnSwitchSelectEnter);
        }

        void OnSwitchSelectEnter(SelectEnterEventArgs args)
        {
            anim_Switch.SetBool("IsOn", true);
            outline_Switch.enabled = false;
            base.OnTaskCompleted();
        }
    }
}