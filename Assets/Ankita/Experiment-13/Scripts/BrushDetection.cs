using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrushDetection : MonoBehaviour
{
    private int cnt = 10, hammerCnt;
    public static bool sheet1, sheet2;
    public GameObject nextObj;

    public SoundPlayer soundPlayer, hammerPlayer;
    public ParticleSystem dustPS;
    public TMP_Text sheetCntText;

    private void Start()
    {
        cnt = 10;
        sheet1 = sheet2 = false;
    }

    public void SetText(int sheet)
    {
        sheetCntText.text = "Clean MS Sheet" + sheet + " : " + cnt + " times";
    }
    public void ClearText()
    {
        sheetCntText.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (ExperimentFlowManager.instance.cntSteps == 5)
        {
            if (collision.transform.name.Contains("Hammer"))
            {
                hammerCnt++;
                PlayHammerStrokeSound();
            }
            if (hammerCnt >= 10)
            {
                if (gameObject.name.Contains("2ndMold"))
                {
                    ExperimentFlowManager.instance.innerStep++;
                    ExperimentFlowManager.instance.Step6_EnableHighlighting(ExperimentFlowManager.instance.innerStep);
                }
                else
                {
                    nextObj.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
        else
        {
            ExperimentFlowManager.instance.brush.GetComponent<Outline>().enabled = false;
            if (collision.transform.CompareTag("WireBrush"))
            {
                Debug.Log(collision.transform.name +" --> "+ gameObject.name);

                PlayBrushStrokeSound();

                if (gameObject.name.Contains("Sheet1"))
                {
                    //cnt++;
                    //if (cnt > 5)
                    //    sheet1 = true;
                    if (cnt > 0)
                        cnt--;
                    if (cnt >= 0 && sheet1 == false)
                    {
                        SetText(1);
                    }
                    if (cnt <= 0 && sheet1 == false)
                    {
                        sheetCntText.text = "MS Sheet1 Cleaned";
                        gameObject.GetComponent<BrushDetection>().enabled = false;
                        sheet1 = true;
                    }
                }
                else
                {
                    //cnt++;
                    //if (cnt > 5)
                    //    sheet2 = true;
                    if (cnt > 0)
                        cnt--;
                    if (cnt >= 0 && sheet2 == false)
                    {
                        SetText(2);
                    }
                    if (cnt <= 0 && sheet2 == false)
                    {
                        sheetCntText.text = "MS Sheet2 Cleaned";
                        gameObject.GetComponent<BrushDetection>().enabled = false;
                        sheet2 = true;
                    }
                }

                if (sheet1 && sheet2)
                {
                    if (ExperimentFlowManager.instance.cntSteps == 2)
                    {
                        ExperimentFlowManager.instance.Step3_EnableHighlighting(2);
                    }
                    else if (ExperimentFlowManager.instance.cntSteps == 7)
                    {
                        ExperimentFlowManager.instance.Step8_EnableHighlighting(2);
                    }
                }
            }
        }
    }

    public void PlayBrushStrokeSound()
    {
        if (!soundPlayer.AudioSource.isPlaying)
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0]);

        dustPS.Play();
    }

    public void PlayHammerStrokeSound()
    {
        if (!hammerPlayer.AudioSource.isPlaying)
            hammerPlayer.PlayRandomClip(hammerPlayer.ClipLists[0]);
    }
}
