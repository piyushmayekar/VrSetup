using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CenterPunch : MonoBehaviour
{
    public static event UnityAction OnHammerHit;
    public float hitThreshold = 1f;

    Rigidbody rb;
    PiyushUtils.Hammer hammer;

    #region SINGLETON
    static CenterPunch instance = null;

    public static CenterPunch Instance { get => instance; }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hammer = PiyushUtils.Hammer.Instance;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_Constants.HAMMER_TAG))
        {
            if (other.impulse.magnitude >= hitThreshold)
            {
                OnHammerHit?.Invoke();
            }
            hammer.RemoveVelocity();
        }
    }

    public void OnPunchingAreaEnter()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void OnPunchingAreaExit()
    {
        rb.constraints = RigidbodyConstraints.None;
    }
}
