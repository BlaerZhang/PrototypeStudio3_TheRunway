using System;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.MotionBlur.Runtime
{
    [Serializable, VolumeComponentMenuForRenderPipeline("OccaSoftware/Better Motion Blur", typeof(UniversalRenderPipeline))]
    public sealed class BetterMotionBlur : VolumeComponent, IPostProcessComponent
    {
        private void OnValidate()
        {
            depthSeparationDistance.value = Mathf.Max(0, depthSeparationDistance.value);
            shutterSpeed.value = Mathf.Max(0, shutterSpeed.value);
        }

        [Tooltip("Set to On to enable Motion Blur processing.")]
        public ModeParameter mode = new ModeParameter(Mode.Off);

        [Tooltip("Quality level used by the motion blur. Higher quality levels give better results but require more processing power.")]
        public QualityLevelParameter qualityLevel = new QualityLevelParameter(QualityLevel.Medium);

        [Tooltip(
            "Simulated shutter speed. Affects the intensity of the motion blur effect. [units: seconds per frame]. For reference values, see: https://en.wikipedia.org/wiki/Shutter_speed."
        )]
        public FloatParameter shutterSpeed = new FloatParameter(0.01f);

        [Tooltip(
            "Enables Depth Separation. Depth Separation will force objects in the background to ignore foreground objects during blurring. This improves quality but has a performance cost."
        )]
        public ModeParameter depthSeparationMode = new ModeParameter(Mode.On);

        [Tooltip("The world-space distance for the maximum depth limit.")]
        public FloatParameter depthSeparationDistance = new FloatParameter(1f);

        public bool IsActive()
        {
            if (mode.value == Mode.Off)
                return false;

            return true;
        }

        public bool IsTileCompatible() => false;

        public int QualityValue(QualityLevel qualityLevel)
        {
            switch (qualityLevel)
            {
                case QualityLevel.Low:
                    return 8;
                case QualityLevel.Medium:
                    return 16;
                case QualityLevel.High:
                    return 32;
                default:
                    return 16;
            }
        }
    }

    [Serializable]
    public sealed class ModeParameter : VolumeParameter<Mode>
    {
        public ModeParameter(Mode value, bool overrideState = false)
            : base(value, overrideState) { }
    }

    [Serializable]
    public sealed class QualityLevelParameter : VolumeParameter<QualityLevel>
    {
        public QualityLevelParameter(QualityLevel value, bool overrideState = false)
            : base(value, overrideState) { }
    }

    public enum Mode
    {
        Off,
        On
    }

    public enum QualityLevel
    {
        Low,
        Medium,
        High
    }
}
