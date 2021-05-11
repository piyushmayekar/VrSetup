using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PourDetector : MonoBehaviour
{
    public int pourThershold = 45;
    public Transform origin = null;
    public GameObject streaPrefab = null;

    private bool isPouring = false;
    private WaterStream currentStream = null;
    public string cylinderkey;
    public UnityEvent NextStep;
    private void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThershold;
        if(isPouring!= pourCheck)
        {
            isPouring = pourCheck;
            if(isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }
    void StartPour()
    {
        currentStream = CreatStream();
        currentStream.Begin();
        currentStream.hitcollidername = cylinderkey;
        currentStream.NextStep = NextStep;
        print("Start");

    }
    void EndPour()
    {
        currentStream.End();
        currentStream = null;
        print("end");

    }
    float CalculatePourAngle()
    {
        return transform.forward.y * Mathf.Rad2Deg; 
    }
    private WaterStream CreatStream()    {
        GameObject streamObject = Instantiate(streaPrefab, origin.position, Quaternion.identity, transform);

        return streamObject.GetComponent<WaterStream>();
    }
}