using UnityEngine;
using System;
using System.Collections.Generic;

namespace FlatWelding
{
    public class BrushTask : Task
    {
        [SerializeField, Tooltip("Total no of cleaning points")]
        int cleanPointCount = 15;

        internal void EdgeBrushed()
        {
            cleanPointCount--;
            if (cleanPointCount <= 0)
            {
                OnTaskCompleted();
            }

        }

    }
}