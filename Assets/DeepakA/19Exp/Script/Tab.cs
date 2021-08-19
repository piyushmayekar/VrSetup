using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    [SerializeField] public GameObject tablet;
    [SerializeField] public float tim;
    [SerializeField] public bool tabON;

    public void Start()
    {
        tabON = true;
    }
    private void Update()
    {
        if (tabON == true)
        {
            tim += 1 * Time.deltaTime;
            if (tim > 1)
            {
                tim = 0;
                tablet.transform.localScale = new Vector3(0, 0, 0);
                tabON = false;
            }
        }

    }
}
