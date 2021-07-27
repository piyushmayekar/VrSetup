using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class SpannerTrigger : MonoBehaviour
    {
        public UnityAction OnSpannerEnter, OnSpannerExit;
        const string spannerName = "Spanner";
        public Transform spannerT;

        void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains(spannerName))
            {
                spannerT = other.transform;
                OnSpannerEnter?.Invoke(); 
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name.Contains(spannerName))
                OnSpannerExit?.Invoke();
        }
    }
}