using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PiyushUtils
{

    public class TorchLightingTask : Task
    {
        [SerializeField] Knob acetyleneKnob, oxygenKnob;
        [SerializeField] float targetValue;
        [SerializeField] CuttingTorch torch;
        [SerializeField] Outline torchOutline, lighterOutline;
        
        public override void OnTaskBegin()
        {
            base.OnTaskBegin();
            acetyleneKnob.EnableTurning(targetValue, true);
            acetyleneKnob.OnTargetValueReached.AddListener(LightTheTorch);
        }

        void LightTheTorch()
        {
            acetyleneKnob.OnTargetValueReached.RemoveListener(LightTheTorch);
            torch.ToggleLighterDetection();
            torchOutline.enabled = true;
            lighterOutline.enabled = true;
            torch.TorchOnStateChanged.AddListener(OnTorchIgnited);
        }

        void OnTorchIgnited()
        {
            torch.TorchOnStateChanged.RemoveListener(OnTorchIgnited);
            torch.ToggleLighterDetection(false);
            torchOutline.enabled = false;
            lighterOutline.enabled = false;
            OpenOxygenKnob();
        }

        void OpenOxygenKnob()
        {
            oxygenKnob.EnableTurning(targetValue, true);
            oxygenKnob.OnTargetValueReached.AddListener(OnOxygenKnobTurnComplete);
        }

        void OnOxygenKnobTurnComplete()
        {
            torch.SwitchToFlame(1);
            ReduceAcetyleneKnob();
        }

        void ReduceAcetyleneKnob()
        {
            acetyleneKnob.EnableTurning(1, false);
            acetyleneKnob.OnTargetValueReached.AddListener(OnAcetyleneReduction);
        }

        void OnAcetyleneReduction()
        {
            torch.SwitchToFlame(2);
            OnTaskCompleted();
        }
    }
}