using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChippingHammer_VL : MonoBehaviour
{
    public Material weldMat;
    public int countPoint;
    public int currentCount = 0;

    private UnityEvent CallMethodOnRemoveSlagDone = new UnityEvent();
    private AudioSource Audio;

    private void Start()
    {
        Audio = GetComponent<AudioSource>();
    }
    public void SetChippingHammerParams(int _countPoints)
    {
        countPoint = _countPoints;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChipHammerHL")
        {
            HLSound.player.PlayHighlightSnapSound();
            other.gameObject.SetActive(false);
        }

        if (other.tag == "Slag")
        {
            Audio.Play();
            currentCount++;
            other.gameObject.GetComponent<MeshRenderer>().sharedMaterial = weldMat;
            other.gameObject.GetComponent<CapsuleCollider>().enabled = false;

            if (currentCount == countPoint)
            {
                EmptyParams();
                if (CallMethodOnRemoveSlagDone != null)
                {
                    CallMethodOnRemoveSlagDone.Invoke();
                }
            }
        }
    }

    public void EmptyParams()
    {
        countPoint = 0;
        currentCount = 0;
    }

    public void AssignMethodOnSlagRemoveDone(UnityAction method) 
    {
        if (CallMethodOnRemoveSlagDone != null)
        {
            CallMethodOnRemoveSlagDone.RemoveAllListeners();
        }
        CallMethodOnRemoveSlagDone.AddListener(method);
    }
}
