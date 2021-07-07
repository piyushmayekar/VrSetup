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

        public override void OnTaskBegin()
        {
            //TODO
            objectsToPick.ForEach(o =>
            {
                o.GetComponent<XRGrabInteractable>().selectEntered.RemoveAllListeners();
                o.GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnObjectSelect);
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
                if (objectsToPick.Count == 0)
                    OnTaskCompleted();
            }
        }
    }
}