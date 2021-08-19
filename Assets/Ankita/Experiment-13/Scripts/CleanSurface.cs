using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CleanSurface : MonoBehaviour
{
    private int countdotpoint = 20;
    public SoundPlayer soundPlayer;
    public ParticleSystem dustPS;
    public TMP_Text sheetCntText;

    void Start()
    {
        countdotpoint = 20;
    }
    public void SetText()
    {
        int i = countdotpoint / 2;
        sheetCntText.text = "Clean MS sheets welding surface : " + i + " times";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "DotPoint")
        {
            countdotpoint--;
            SetText();
            //other.transform.GetComponent<SphereCollider>().enabled = false;
            PlayBrushStrokeSound();

            if (countdotpoint <= 0)
            {
                sheetCntText.text = "";

                if (SceneManager.GetActiveScene().name.Contains("15"))
                {
                    Experiment15FlowManager.instance.Step17_EnableHighlighting(2);
                }
                else if (SceneManager.GetActiveScene().name.Contains("14"))
                {
                    Experiment14FlowManager.instance.Step17_EnableHighlighting(2);
                }
                else if (SceneManager.GetActiveScene().name.Contains("13"))
                {
                    ExperimentFlowManager.instance.Step21_EnableHighlighting(2);
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
