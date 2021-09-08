using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFeedbackDisplayer : MonoBehaviour
{
    [SerializeField, Tooltip("The final scale of the model to be scaled from zero")] Vector3 finalModelScale;
    [SerializeField] float scaleSpeed = 0.5f, destroyInterval = 5f;

    private void OnEnable()
    {
        
    }

    IEnumerator Manager()
    {
        while (gameObject.activeSelf)
        {

            yield return new WaitForEndOfFrame();
        }
    }
}
