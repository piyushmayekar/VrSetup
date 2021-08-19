using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class tabReset : MonoBehaviour
{
    [SerializeField] public bool tabON;
    public void Reset()
    {
        tabON = true;
    }
    private void Update()
    {
        if (tabON == true)
        {
            transform.localScale = new Vector3(1, 1, 1);
            tabON = false;
        }
    }
}
