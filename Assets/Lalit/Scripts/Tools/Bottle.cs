using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public GameObject dropPrefab;
    public Transform point;

    public float dropInterval = 0.5f;
    public float timeElapsed;

    public bool enableWorking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BottleHighlight")
        {
            other.gameObject.SetActive(false);

        }
    }

    public void StartDropping()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > dropInterval)
        {
            GameObject drop = Instantiate(dropPrefab, point.position, point.localRotation);
            timeElapsed = 0;
        }
    }

    public void onGrab()
    {
        enableWorking = true;
    }

    public void OnUnGrab()
    {
        enableWorking = false;
    }
}
