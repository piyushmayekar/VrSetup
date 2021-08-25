using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterTweldingEffect : MonoBehaviour
{
    float speed = 3f;
    public Material red, brown, original;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        GetComponent<Renderer>().material = red;
        yield return new WaitForSeconds(speed);
        GetComponent<Renderer>().material = brown;
        yield return new WaitForSeconds(speed - 1);
        GetComponent<Renderer>().material = original;
    }
}
