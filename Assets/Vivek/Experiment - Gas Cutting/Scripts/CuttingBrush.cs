using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBrush : MonoBehaviour
{
    public static CuttingBrush instance;
    [SerializeField] ParticleSystem dustPS;
    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] Rigidbody parentBrushRb;
    private void Awake()
    {
        instance = this;
    }
    void OnTriggerEnter(Collider other)
    {
        PlayBrushStrokeSound();
        if (other.CompareTag(_Constants.CLEANPOINT_TAG))
        {
            //Turning off the clean point collider once the cleaning is done
           // other.enabled = false;
            dustPS.Play();
            EdgeBrushed();
     //       Debug.Log("Calll");
        }
        if (other.CompareTag(_Constants.SLAG_TAG))
        {
          //  other.GetComponent<Rigidbody>().velocity = parentBrushRb.velocity;
            Destroy(other.gameObject, 5f);
        }
    }

    public void PlayBrushStrokeSound()
    {
        if (!soundPlayer.AudioSource.isPlaying)
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0]);
    }

    [SerializeField, Tooltip("Total no of cleaning points")]
  public  int cleanPointCount = 15;
    void EdgeBrushed()
    {
        Debug.Log(cleanPointCount);
        cleanPointCount--;
        
        if (cleanPointCount <= 0)
        {
            if (!GasCuttingManager.instance.flameOff)
            {
                GasCuttingManager.instance.checkBrushStep();
            }
            else
            {
                GasCuttingManager.instance.cleanBrushFinish();

            }
        }

    }
}
