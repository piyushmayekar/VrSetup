using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resets the last stable position on a table if this object touches the ground
/// </summary>
public class PositionResetter : MonoBehaviour
{
    public bool shouldReset = true;
    public Vector3 lastStablePos, lastStableRotEulerAngles;
    public Rigidbody rb;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        lastStablePos = transform.position;
        lastStableRotEulerAngles = transform.rotation.eulerAngles;
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.name == _Constants.TABLE_TAG && rb.velocity.sqrMagnitude <= 1f)
        {
            lastStablePos = transform.position;
            lastStableRotEulerAngles = transform.rotation.eulerAngles;
        }
        if (other.collider.gameObject.name == _Constants.FLOOR_TAG)
        {
            ResetPos();
        }
    }

    public void ResetPos()
    {
        if (!shouldReset) return;
        if (rb)
        {
            rb.MovePosition(lastStablePos);
            rb.MoveRotation(Quaternion.Euler(lastStableRotEulerAngles));
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
