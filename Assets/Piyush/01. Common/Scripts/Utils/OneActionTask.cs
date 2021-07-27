using System.Collections;
using UnityEngine;

namespace PiyushUtils
{
    public class OneActionTask : Task
    {
        
        public void OnActionDone()
        {
            OnTaskCompleted();
        }
    }
}