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
        [SerializeField] AudioClip pickSound;

        public override void OnTaskBegin()
        {
            //TODO
            objectsToPick.ForEach(o =>
            {
                o.GetComponent<XRGrabInteractable>().selectEntered.RemoveAllListeners();
                o.GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnObjectSelect);
                o.GetComponent<Outline>().enabled = true;
            });
        }

        public void OnObjectSelect(SelectEnterEventArgs args)
        {
            OnObjectSelect(args.interactable.gameObject);
        }

        public void OnObjectSelect(GameObject _obj)
        {
            if (objectsToPick.Contains(_obj))
            {
                objectsToPick.Remove(_obj);
                _obj.SetActive(false);
                AudioSource.PlayClipAtPoint(pickSound, _obj.transform.position);
                if (objectsToPick.Count == 0)
                    OnTaskCompleted();
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