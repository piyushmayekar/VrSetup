using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushPlate : MonoBehaviour
{
    public string trigName;
    public float tim;
    public float timA;
    public GameObject objA;
    public GameObject objB;

    private void Start()
    {
        tim = 0;
        objB.SetActive(true);
        objA.SetActive(false);

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == trigName)
        {
            tim += 1 * Time.deltaTime;
            timA += 1 * Time.deltaTime;
            if (tim > 5)
            {
                tim = 0;
                objA.SetActive(true);
                objB.SetActive(false);
                transform.name = "Done";
            }
            if (timA > 1)
            {
                timA = 0;
                Instantiate(Resources.Load("rubA"), transform.position, transform.rotation);
            }
        }
    }

}
