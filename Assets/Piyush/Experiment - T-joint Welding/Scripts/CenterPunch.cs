using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPunch : MonoBehaviour
{
    public static event Action OnHammerHit;


    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Hammer"))
        {
            OnHammerHit?.Invoke();
        }
    }
}
