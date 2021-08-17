using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace SMAW_Setup_4
{
    public class Task_AttachObjects : Task
    {
        [SerializeField] List<GameObject> highlights;
        [SerializeField] List<XRSocketInteractor> sockets;
        [SerializeField] List<XRGrabInteractable> attachables;
        [SerializeField] int _index = 0;
        [SerializeField] float _timeInterval = 1f;

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            StartAttaching();
        }

        internal void StartAttaching()
        {
            highlights[_index].SetActive(true);
            sockets[_index].socketActive = true;
            attachables[_index].enabled = true;
            sockets[_index].selectEntered.AddListener(OnSocketEnter);
        }

        void OnSocketEnter(SelectEnterEventArgs args)
        {
            if (args.interactable.gameObject == attachables[_index].gameObject)
            {
                highlights[_index].SetActive(false);
                StartCoroutine(TimedDisabler());
            }
        }

        IEnumerator TimedDisabler()
        {
            yield return new WaitForSeconds(_timeInterval);
            sockets[_index].socketActive = false;
            attachables[_index].enabled = false;
            sockets[_index].selectEntered.RemoveAllListeners();
            _index++;
            if (_index >= attachables.Count)
                OnTaskCompleted();
            else
                StartAttaching();
        }
    }
}