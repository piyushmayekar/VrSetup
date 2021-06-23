using UnityEngine;

/// <summary>
/// Script created by Piyush M.
/// </summary>
public class DisableGameObjectOnEnter:MonoBehaviour
{
    [SerializeField] string _tagOfObject;
    
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(_tagOfObject) && other.gameObject!=gameObject)
            gameObject.SetActive(false);
    }
}