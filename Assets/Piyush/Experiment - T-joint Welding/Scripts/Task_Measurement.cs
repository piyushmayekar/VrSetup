using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TWelding
{
    public class Task_Measurement : Task
    {
        [SerializeField] Button button;
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            Invoke(nameof(EnableDoneButton), 2f);
        }

        private void EnableDoneButton()
        {
            button.gameObject.SetActive(true);
        }

        public void OnButtonClick()
        {
            button.gameObject.SetActive(false);
            OnTaskCompleted();
        }
    }
}