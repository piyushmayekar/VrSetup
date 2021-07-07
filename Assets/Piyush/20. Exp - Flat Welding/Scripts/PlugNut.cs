using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class PlugNut : MonoBehaviour
    {
        public UnityAction OnSpannerEnter, OnSpannerExit;
        const string spannerName = "Spanner";

        void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains(spannerName))
                OnSpannerEnter?.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.name.Contains(spannerName))
                OnSpannerExit?.Invoke();
        }
    }
}