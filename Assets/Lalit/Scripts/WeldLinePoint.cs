using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldLinePoint : MonoBehaviour
{
    MeshRenderer meshRenderer;
    public  GameObject slag;
    public bool done;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    public void ShowPoint()
    {
        meshRenderer.enabled = true;
        slag.SetActive(true);
        done = true;
    }
}
