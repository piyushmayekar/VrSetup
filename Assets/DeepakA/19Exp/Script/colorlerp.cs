using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorlerp : MonoBehaviour
{
    // Start is called before the first frame update
    public Color startColor;
    public Color endcolor;
    public float col = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.black, col);
        if (col < 1.0f)
        {
            col += 1 * Time.deltaTime;
        }
    }
}
