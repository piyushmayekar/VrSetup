using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldHeadPonit : MonoBehaviour
{
    public Material slagMat;

    private float speed = 0.02f;
    public bool speedUp = false;

    private void Start()
    {
        if (speedUp)
        {

            Color color = gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;
            color.r = 0.99f;
            gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;
        }
    }

    void Update()
    {
        Color color = gameObject.GetComponent<MeshRenderer>().sharedMaterial.color;
        color.r -= Time.deltaTime * speed;
        gameObject.GetComponent<MeshRenderer>().sharedMaterial.color = color;

        if (color.r <= 0.3f)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = slagMat;
            this.enabled = false;
        }
    }
}
