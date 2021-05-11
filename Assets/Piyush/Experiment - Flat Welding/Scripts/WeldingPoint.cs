using System;
using System.Collections;
using System.Collections.Generic;
using FlatWelding;
using UnityEngine;

/// <summary>
/// 2 types of welding point. exterior ones after welding done, the collider turns into non trigger one.
/// </summary>
public class WeldingPoint : MonoBehaviour
{
    public event Action OnWeldingDone, OnHitWithHammer;

    /// <summary>
    /// Used to check the angle between the electrode and the plates on the receiver side. 
    /// true if the point is at left
    /// </summary>
    public static event Action<bool> CheckWeldingElectrodeAngle;

    [SerializeField] bool isWeldingDone = false, isExteriorWeldingPoint = false;
    [SerializeField] float breakForceThreshold = 1.5f;
    [SerializeField] MeshRenderer _renderer;
    [SerializeField] ParticleSystem residueThrowPS;
    [SerializeField] Material indicatorMaterial, weldingMat;
    [SerializeField] List<GameObject> slagPrefabs;
    [SerializeField] Vector2 residueThrowForce;
    [SerializeField] bool isPointOnLeft = false;
    Rigidbody rb;

    public bool IsWeldingDone { get => isWeldingDone; set => isWeldingDone = value; }
    public bool IsExteriorWeldingPoint { get => isExteriorWeldingPoint; set => isExteriorWeldingPoint = value; }

    static WeldingMachine machine;
    static ChippingHammer chippingHammer;
    static WeldingArea weldingArea;

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        machine = WeldingMachine.Instance;
        chippingHammer = ChippingHammer.Instance;
        weldingArea = WeldingArea.Instance;
        isPointOnLeft = Vector3.Distance(weldingArea.LeftT.position, transform.position) <
                Vector3.Distance(weldingArea.RightT.position, transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_Constants.WELDING_TAG) && !isWeldingDone && (isPointOnLeft == machine.IsElectrodeAtLeft))
        {
            IsWeldingDone = true;
            _renderer.material = weldingMat;
            _renderer.enabled = true;
            OnWeldingDone?.Invoke();
            if (IsExteriorWeldingPoint)
            {
                GetComponent<Collider>().isTrigger = false;
                CheckWeldingElectrodeAngle?.Invoke(isPointOnLeft);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(_Constants.CHIPPING_HAMMER_TAG) && other.relativeVelocity.magnitude >= breakForceThreshold &&
        (isPointOnLeft == chippingHammer.IsHittingLeftPoints))
        {
            rb.detectCollisions = false;
            _renderer.enabled = false;
            if (residueThrowPS != null)
                residueThrowPS.Play();
            else
            {
                slagPrefabs.ForEach(prefab =>
                {
                    prefab.SetActive(true);
                    prefab.transform.parent = null;
                    prefab.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere
                    * UnityEngine.Random.Range(residueThrowForce.x, residueThrowForce.y));
                    Destroy(prefab, 20f);
                });
            }
            OnHitWithHammer?.Invoke();
        }
    }
}