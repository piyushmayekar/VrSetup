using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BrushDetectionExp14 : MonoBehaviour
{
    private int cnt = 10;
    public static bool sheet1, sheet2;
    public SoundPlayer soundPlayer;
    public ParticleSystem dustPS;
    public TMP_Text sheetCntText;

    private void Start()
    {
        cnt = 10;
        sheet1 = sheet2 = false;
    }
    public void SetText(int sheet)
    {
        sheetCntText.text = "Clean MS Sheet"+ sheet +" : " + cnt + " times";
    }
    public void ClearText()
    {
        sheetCntText.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SceneManager.GetActiveScene().name.Contains("15"))
        {
            Experiment15FlowManager.instance.brush.GetComponent<Outline>().enabled = false;
        }
        else if (SceneManager.GetActiveScene().name.Contains("14"))
        {
            Experiment14FlowManager.instance.brush.GetComponent<Outline>().enabled = false;
        }

        if (collision.transform.CompareTag("WireBrush"))
        {
            Debug.Log(collision.transform.name + " --> " + gameObject.name);

            PlayBrushStrokeSound();

            if (gameObject.name.Contains("Sheet1"))
            {
                if(cnt > 0)
                    cnt--;
                if (cnt >= 0 && sheet1 == false)
                {
                    SetText(1);
                }
                if (cnt <= 0 && sheet1 == false)
                {
                    sheetCntText.text = "MS Sheet1 Cleaned";
                    gameObject.GetComponent<BrushDetectionExp14>().enabled = false;
                    sheet1 = true;
                }
            }
            else
            {
                if (cnt > 0)
                    cnt--;
                if (cnt >= 0 && sheet2 == false)
                {
                    SetText(2);
                }
                if (cnt <= 0 && sheet2 == false)
                {
                    sheetCntText.text = "MS Sheet2 Cleaned";
                    gameObject.GetComponent<BrushDetectionExp14>().enabled = false;
                    sheet2 = true;
                }
            }

            if (sheet1 && sheet2)
            {
                sheet1 = sheet2 = false;
                
                if (SceneManager.GetActiveScene().name.Contains("15"))
                {
                    Experiment15FlowManager.instance.Step3_EnableHighlighting(2);
                }
                else if (SceneManager.GetActiveScene().name.Contains("14"))
                {
                    Experiment14FlowManager.instance.Step3_EnableHighlighting(2);
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
}
