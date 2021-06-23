using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grinding;

public class Task_GrindTool : Task
{
    [SerializeField] CuttingTool cuttingTool;
    [SerializeField] GrindingWheelType grindingType;
    public override void OnTaskBegin()
    {
        base.OnTaskBegin();
        if (grindingType == GrindingWheelType.Rough)
            cuttingTool.OnGrindingComplete += (GrindingWheelType type) =>
            {
                if (type == GrindingWheelType.Rough)
                    OnTaskCompleted();
            };
        if (grindingType == GrindingWheelType.Surface)
            cuttingTool.OnGrindingComplete += (GrindingWheelType type) =>
            {
                if (type == GrindingWheelType.Surface)
                    OnTaskCompleted();
            };
    }
}
