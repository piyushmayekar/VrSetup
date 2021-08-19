using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taskk : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Main;
    public GameObject[] Tsk;
    public GameObject base_box;

    public static int scene_Num;
    private int timA;
    private int trigNum;
    public int i = 0;

    public int i_start = 0;
    private string trigname = "1";
    private bool getNum = true;
    private GameObject scaleRuler;

    void Start()
    {
        scaleRuler = GameObject.Find("Steel Rule");
        i_start = CheckFinalTask.Tasknum;
        if (i_start == 0)
        {
            Tsk[0].SetActive(false);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(false);
            base_box.SetActive(true);
        }
        if (i_start == 1)
        {
            Tsk[0].SetActive(true);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(false);
            base_box.SetActive(true);
        }
        if (i_start == 2)
        {
            Tsk[0].SetActive(false);
            Tsk[1].SetActive(true);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(false);
            base_box.SetActive(true);
        }
        if (i_start == 3)
        {
            Tsk[0].SetActive(false);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(true);
            Tsk[3].SetActive(false);
            base_box.SetActive(false);
        }
        if (i_start == 4)
        {
            Tsk[0].SetActive(false);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(true);
            base_box.SetActive(false);
        }
    }

    private void Update()
    {
        transform.name = Main.transform.name;
        if (getNum == true)
        {
            if (i < 50)
            {
                i++;
                if (i == 10)
                {
                    trigNum = int.Parse(transform.name);
                }
                if (i == 20)
                {
                    trigNum++;
                    // print("     " + transform.name +": " + trigNum);
                }
                if (i == 30)
                {
                    trigname = "" + trigNum;
                    // print("         "+transform.name + "trigname: " + trigname);
                }
            }
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MarkPoint" && other.gameObject.name == transform.name && scaleRuler.transform.GetChild(0).name == "trigon")
        {
            Main.transform.name = "Done";
            GetComponent<Collider>().enabled = false;

            Tsk[0].SetActive(true);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(false);
            base_box.SetActive(true);
            Instantiate(Resources.Load("mark"), transform.position, transform.rotation);
            other.transform.name = trigname;

        }
        if (other.gameObject.tag == "DotPoint" && other.gameObject.name == transform.name)
        {
            Main.transform.name = "Done";
            GetComponent<Collider>().enabled = false;

            Tsk[0].SetActive(false);
            Tsk[1].SetActive(true);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(false);
            base_box.SetActive(true);
            Instantiate(Resources.Load("punch"), transform.position, transform.rotation);
            other.transform.name = trigname;
        }
        if (other.gameObject.tag == "Spark" && other.gameObject.name == transform.name)
        {
            Main.transform.name = "Done";
            GetComponent<Collider>().enabled = false;

            Tsk[0].SetActive(false);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(true);
            Tsk[3].SetActive(false);
            base_box.SetActive(false);
            Instantiate(Resources.Load("Spark"), transform.position, transform.rotation);
            other.transform.name = trigname;
        }
        if (other.gameObject.tag == "Hammer" && other.gameObject.name == transform.name)
        {
            Main.transform.name = "Done";
            GetComponent<Collider>().enabled = false;

            Tsk[0].SetActive(false);
            Tsk[1].SetActive(false);
            Tsk[2].SetActive(false);
            Tsk[3].SetActive(true);
            base_box.SetActive(false);
            Instantiate(Resources.Load("Hammer"), transform.position, transform.rotation);
            other.transform.name = trigname;
        }
    }

}
