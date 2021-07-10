using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBrush : MonoBehaviour
{
    public static CuttingBrush instance;
    [SerializeField] ParticleSystem dustPS;
    [SerializeField] SoundPlayer soundPlayer;
    [SerializeField] Rigidbody parentBrushRb;
    public experimentType type;
    public bool isStop;
    private void Awake()
    {
        instance = this;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_Constants.CLEANPOINT_TAG))
        {
        PlayBrushStrokeSound();

            dustPS.Play();
            EdgeBrushed();
        }
        if (other.CompareTag(_Constants.SLAG_TAG))
        {
            Destroy(other.gameObject, 5f);
        }
    }

    public void PlayBrushStrokeSound()
    {
        if (!soundPlayer.AudioSource.isPlaying)
            soundPlayer.PlayRandomClip(soundPlayer.ClipLists[0]);
    }

    [SerializeField, Tooltip("Total no of cleaning points")]
    public int cleanPointCount = 15;
    void EdgeBrushed()
    {
        cleanPointCount--;
        if (cleanPointCount <= 0&& !isStop)
        {
            if (type == experimentType.GasJointPlate)
            {
                if (!GasJointweldingManager.instance.flameOff)
                {
                    //Debug.Log("call brush 2");
                    GasJointweldingManager.instance.checkBrushStep();
                }
                else
                {
                //    Debug.Log("call brush2");
                    GasJointweldingManager.instance.cleanBrushFinish();

                }
            }
            else
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
            isStop = true;
        }
        else
        {
            ReadStepsFromJson.instance.tablet.SetActive(true);
            ReadStepsFromJson.instance.stepText.text = "\nPick up C.S. brush and clean the surface.\n" + cleanPointCount.ToString() + "/15";
        }

    }
}
