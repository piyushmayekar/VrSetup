using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatWelding
{
    public class CleanPoint : MonoBehaviour
    {
        public Action OnBrushBristleEnter;

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<BrushBristles>())
            {
                OnBrushBristleEnter?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}