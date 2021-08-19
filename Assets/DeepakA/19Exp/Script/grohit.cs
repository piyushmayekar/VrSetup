using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grohit : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject pos;
    public string trigName = "ground";
    private Vector3 Curent_pos;
    private Vector3 Curent_roll;

    private void Start()
    {
        Curent_pos = transform.position;
        Curent_roll = transform.eulerAngles;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == trigName)
        {
            transform.position = Curent_pos;
            transform.eulerAngles = Curent_roll;
        }
    }
}
