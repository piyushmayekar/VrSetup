using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    /// <summary>
    /// Wear the apparatus task
    /// </summary>
    public class WearApparelTask : Task
    {
        [SerializeField] List<GameObject> objectsToPick;

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