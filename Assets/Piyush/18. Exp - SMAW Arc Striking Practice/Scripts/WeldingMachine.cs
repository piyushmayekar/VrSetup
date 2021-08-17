using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMAW_Arc_18
{
    public class WeldingMachine : FlatWelding.WeldingMachine
    {
        [SerializeField] float tipAngle = 0f, defreezeTime=0.5f;
        [SerializeField] float minTipAngle, maxTipAngle;
        const string ANGLE_NOT_CORRECT = "Angle not correct: ";
        public override void TipInContact(bool inContact)
        {
            base.TipInContact(inContact);
            if (inContact)
            {
                tipAngle = Vector3.SignedAngle(tip.transform.up, Vector3.up, Vector3.up);
                ShowErrorIndicator(false);
                if (tipAngle < minTipAngle || tipAngle > maxTipAngle)
                {
                    ShowErrorIndicator(true, ANGLE_NOT_CORRECT + Mathf.FloorToInt(tipAngle) +" degrees");
                    _rb.constraints = RigidbodyConstraints.FreezeAll;
                    Invoke(nameof(UnfreezeRb), defreezeTime);
                }
            }
            else
            {
                UnfreezeRb();
            }
        }

        void UnfreezeRb()
        {
            _rb.constraints = RigidbodyConstraints.None;
        }
    }
}