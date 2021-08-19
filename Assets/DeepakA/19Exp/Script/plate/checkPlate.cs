using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkPlate : MonoBehaviour
{
    public GameObject[] obj;
    public GameObject[] LineDown;
    public float rool= 0;
    public GameObject MainBase;
    public TextMeshProUGUI txt;

    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        rool = 0;
        LineDown[0].SetActive(false);
        LineDown[1].SetActive(false);
        LineDown[2].SetActive(false);
        LineDown[3].SetActive(false);
        LineDown[4].SetActive(false);
        LineDown[5].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (rool >= 180 && CheckFinalTask.Tasknum >= 2)
        {
            MainBase.SetActive(false);
        }
        else
        {
            MainBase.SetActive(true);
        }
        //transform.localEulerAngles = new Vector3(0, 0, rool);
        if (rool > 180)
        {
            rool = 180;
        }
        if (obj[0].transform.name == "halfDone" && obj[1].transform.name == "halfDone" && obj[2].transform.name == "halfDone" && obj[3].transform.name == "halfDone" && obj[4].transform.name == "halfDone" && obj[5].transform.name == "halfDone")
        {
            
            if (rool < 180)
            {
                rool += 50 * Time.deltaTime;
                txt.text = "Turn Your Plate";

            }
            else
            {
                txt.text = " ";
            }
            LineDown[0].SetActive(true);
            LineDown[1].SetActive(true);
            LineDown[2].SetActive(true);
            LineDown[3].SetActive(true);
            LineDown[4].SetActive(true);
            LineDown[5].SetActive(true);
        }
        else
        {
            txt.text = " ";
        }


    }
}
