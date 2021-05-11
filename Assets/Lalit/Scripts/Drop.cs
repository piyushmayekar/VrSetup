using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public GameObject _splash;
    public static int count;
    private void Start()
    {
        Destroy(gameObject, 1.5f);

    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.collider.gameObject.name == "Bottle")
        {
            //Destroy(gameObject);
        }
        else if (collision.collider.tag == "Job")
        {
            ContactPoint p = collision.GetContact(0);

            GameObject splash = Instantiate(_splash, p.point, Quaternion.identity);
            Splash S = splash.GetComponent<Splash>();
            S.wait = true;
            //PlayAudio.ins.PlayDropSound();
            Splash.count++;
        }
        else
        {
            GameObject splash = Instantiate(_splash, transform.position, Quaternion.identity);

        }





    }
}
