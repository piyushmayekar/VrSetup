using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingLineAtJob : MonoBehaviour
{
    public CuttingType cuttingType;
    public int CurrentLine, countlinePoint;
    public Transform[] LineCutPoints;//, Line2CutPoints,Line3CutPoints;
    public GameObject[] DrawLine, cutModel;
    //  public GameObject simpleCutModel;
    public bool iscutting;
    // Start is called before the first frame update
    void Start()
    {
        CurrentLine = 0;
        if (cuttingType == CuttingType.Gascut)
        {
            for (int i = 0; i < LineCutPoints[0].transform.childCount; i++)
            {
                LineCutPoints[1].transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                LineCutPoints[2].transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;

            }
           
        }
        countlinePoint = LineCutPoints[CurrentLine].childCount;
        for (int i = 0; i < LineCutPoints.Length; i++)
        {

            LineCutPoints[i].gameObject.SetActive(true);
        }

        LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).GetChild(0).gameObject.SetActive(true);
        Debug.Log(LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).name);

        if (cuttingType==CuttingType.CircularCut)
        {
         //   if (countlinePoint != 0)
            {

                LineCutPoints[CurrentLine].transform.GetChild(countlinePoint -1).GetComponent<MeshRenderer>().enabled = (true);
                LineCutPoints[CurrentLine].transform.GetChild(countlinePoint-1 ).GetComponent<Outline>().enabled = (true);
            }
        }
        else
        {

        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!iscutting)
        {
            if (LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).name == other.gameObject.name)
            {
                countlinePoint--;
                LineCutPoints[CurrentLine].GetComponent<LineRenderer>().positionCount++;
                LineCutPoints[CurrentLine].GetComponent<LineRenderer>().SetPosition(LineCutPoints[CurrentLine].GetComponent<LineRenderer>().positionCount - 1, other.gameObject.transform.localPosition);
                other.gameObject.SetActive(false);
                if(cuttingType== CuttingType.Gascut)
                {
                    checkGasCutLine();
                }
                else
                {
                    if (cuttingType == CuttingType.CircularCut)
                    {
                        if (countlinePoint != 0)
                        {
                            LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).GetComponent<MeshRenderer>().enabled = (true);
                            LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).GetComponent<Outline>().enabled = (true);
                        }
                    }
                    checkGasCircularLine();
                }
                
            }
        }
    }
    void checkGasCutLine()
    {
        if (countlinePoint <= 0)
        {
            CurrentLine++;
            if (CurrentLine == 3)
            {
                GasCuttingManager.instance.CheckCuttingLine();
                iscutting = true;
                DrawLine[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine].SetActive(true);
                LineCutPoints[CurrentLine - 1].gameObject.SetActive(false);
            }
            else
            {

                for (int i = 0; i < LineCutPoints[0].transform.childCount; i++)
                {
                    LineCutPoints[CurrentLine].transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                }
                DrawLine[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine].SetActive(true);
                LineCutPoints[CurrentLine - 1].gameObject.SetActive(false);
                countlinePoint = LineCutPoints[CurrentLine].childCount;
                LineCutPoints[CurrentLine].transform.GetChild(countlinePoint - 1).GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    void checkGasCircularLine()
    {
        if (countlinePoint <= 0)
        {
            CurrentLine++;
                GasCuttingManager.instance.CheckCuttingLine();
                iscutting = true;
                DrawLine[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine - 1].SetActive(false);
                cutModel[CurrentLine].SetActive(true);
            LineCutPoints[CurrentLine - 1].gameObject.SetActive(false);

        }
    }
}
