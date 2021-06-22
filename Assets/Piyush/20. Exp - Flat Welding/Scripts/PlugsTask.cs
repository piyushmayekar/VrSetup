using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class PlugsTask : Task
    {
        [SerializeField] List<bool> plugsInserted = new List<bool>(2);
        [SerializeField] List<GameObject> plugs;
        [SerializeField] List<GameObject> plugsFinalPos;
        [SerializeField] bool currentSet = false;
        [SerializeField] TMPro.TextMeshProUGUI errorText;
        const string ERROR_TEXT = "Plugs inserted incorrectly!";

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            plugs.ForEach(x => plugsInserted.Add(false));
            CurrentKnob.OnTargetValueSet += () =>
            {
                currentSet = true;
                if (CheckIfAllTasksCompleted()) OnTaskCompleted();
            };
        }

        public void PluggedInSocket(SelectEnterEventArgs args)
        {
            GameObject plug = args.interactable.gameObject;
            GameObject finalPos = args.interactor.gameObject;
            int plugIndex = plugs.FindIndex(x => x == plug);
            int plugFinalPosIndex = plugsFinalPos.FindIndex(x => x == finalPos);
            if (plugIndex >= 0) //Plug is inserted & no other object is
            {
                if (plugIndex == plugFinalPosIndex) //Plug inserted correctly
                {
                    plugsInserted[plugIndex] = true;
                    errorText.text = string.Empty;
                    if (CheckIfAllTasksCompleted())
                        OnTaskCompleted();
                }
                else
                {
                    errorText.text = ERROR_TEXT;
                }
            }
        }

        bool CheckIfAllTasksCompleted()
        {
            return plugsInserted.TrueForAll(x => x) && currentSet;
        }
    }
}