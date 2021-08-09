using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

namespace FlatWelding
{
    /// <summary>
    /// Wear the apparatus task
    /// </summary>
    public class WearApparelTask : Task
    {
        [SerializeField] List<GameObject> objectsToPick;
        [SerializeField] int _index = 0;
        [SerializeField] AudioClip pickSound;

        public override void OnTaskBegin()
        {
            //TODO
            objectsToPick.ForEach(o =>
            {
                o.GetComponent<XRGrabInteractable>().enabled = false;
                o.GetComponent<Outline>().enabled = false;
            });
            EnableObjectPick();
        }

        void EnableObjectPick()
        {
            XRGrabInteractable grab = objectsToPick[_index].GetComponent<XRGrabInteractable>();
            grab.enabled = true;
            grab.selectEntered.RemoveAllListeners();
            grab.selectEntered.AddListener(OnObjectSelect);
            objectsToPick[_index].GetComponent<Outline>().enabled = true;
        }

        public void OnObjectSelect(SelectEnterEventArgs args)
        {
            GameObject _obj = args.interactable.gameObject;
            if (objectsToPick.Contains(_obj))
            {
                _obj.SetActive(false);
                AudioSource.PlayClipAtPoint(pickSound, _obj.transform.position);
                _index++;
                if (_index >= objectsToPick.Count)
                    OnTaskCompleted();
                else
                    EnableObjectPick();
            }
        }
        

        [ContextMenu("Disable Other PPE Kit Objects")]
        public void DisableOtherPPEKitObjects()
        {
            Transform parent = objectsToPick[0].transform.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject pickable = parent.GetChild(i).gameObject;
                if (!objectsToPick.Contains(pickable) && !pickable.name.Contains("Table"))
                    pickable.SetActive(false);
            }
        }
    }
}