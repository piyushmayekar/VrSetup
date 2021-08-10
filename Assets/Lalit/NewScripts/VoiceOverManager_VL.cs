using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOverManager_VL : MonoBehaviour
{
    public static VoiceOverManager_VL instance;
    private AudioSource audioSource;
    public List<AudioClip> englishClips, gujratiClips;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayVOForStepIndex(int index)
    {
        if (audioSource != null)
        {
            if (audioSource.clip != null)
            {
                audioSource.Stop();
                if (ReadStepsAndVideoManager.instance.isChangeFont)
                {
                    if (englishClips[index] != null)
                    {
                        audioSource.PlayOneShot(englishClips[index]);
                    }
                    
                }
                else
                {
                    if (gujratiClips[index] != null)
                    {
                        audioSource.PlayOneShot(gujratiClips[index]);
                    }
                    
                }

            }
        }
    }
}
