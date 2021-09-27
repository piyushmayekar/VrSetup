using System.Collections.Generic;
using UnityEngine;

namespace CornerWelding
{
    public class Slag : MonoBehaviour
    {
        [SerializeField] WeldingPoint weldingPoint;
        [SerializeField] MeshRenderer _renderer;
        [SerializeField] Rigidbody _rb;
        [SerializeField] List<GameObject> slagPrefabs;
        [SerializeField] Vector2 residueThrowForce;
        [SerializeField] Collider _collider;
        [SerializeField] float autoDestructTimer = 20f;

        float breakForceThreshold = 0f;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (breakForceThreshold == 0f) breakForceThreshold = weldingPoint.BreakForceThreshold;
            if (weldingPoint.ShouldShowSlag)
            {
                weldingPoint.OnWeldingDone += OnWeldingDone;
            }
        }

        void OnWeldingDone(WeldingPoint point)
        {
            weldingPoint.OnWeldingDone -= OnWeldingDone;
            _renderer.enabled = true;
            _collider.enabled = true;
        }

        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag(_Constants.CHIPPING_HAMMER_TAG) && other.relativeVelocity.magnitude >= breakForceThreshold)
            {
                _renderer.enabled = false;
                slagPrefabs.ForEach(prefab =>
                {
                    prefab.SetActive(true);
                    prefab.transform.parent = null;
                    prefab.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere
                    * UnityEngine.Random.Range(residueThrowForce.x, residueThrowForce.y));
                    Destroy(prefab, autoDestructTimer);
                });
                weldingPoint.OnSlagHitWithHammer();
                gameObject.SetActive(false);
            }
        }
    }
}