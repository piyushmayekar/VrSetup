using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace PiyushUtils
{
    public class WireV2 : MonoBehaviour
    {
        [SerializeField] PositionConstraint _start, _end;
        [SerializeField] Transform finalStart, finalEnd;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            AttachConstraint(_start, finalStart);
            AttachConstraint(_end, finalEnd);
        }

        private void AttachConstraint(PositionConstraint constraint, Transform t)
        {
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = t;
            source.weight = 1f;
            constraint.AddSource(source);
            constraint.translationOffset = Vector3.zero;
            constraint.constraintActive = true;
        }
    }
}