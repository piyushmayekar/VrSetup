using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace FlatWelding
{
    public class MeasurementTask : Task
    {
        [SerializeField] GameObject steelRule;
        [SerializeField] MeasurementPoint point;
        Vector3 rulePos;
        Quaternion ruleRot;

        public GameObject SteelRule { get => steelRule; set => steelRule = value; }

        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            //Caching ruler initial position & rotation
            rulePos = SteelRule.transform.position;
            ruleRot = SteelRule.transform.rotation;
            point.gameObject.SetActive(true);
        }

        public override void OnTaskCompleted()
        {
            base.OnTaskCompleted();
            Invoke(nameof(ResetRulerPos), 2f);
        }

        internal void ResetRulerPos()
        {
            SteelRule.transform.SetPositionAndRotation(rulePos, ruleRot);
        }
    }
}