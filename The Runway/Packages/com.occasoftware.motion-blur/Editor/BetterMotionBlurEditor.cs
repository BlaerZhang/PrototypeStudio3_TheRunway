using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using OccaSoftware.MotionBlur.Runtime;

namespace OccaSoftware.MotionBlur.Editor
{
    [CustomEditor(typeof(BetterMotionBlur))]
    public class BetterMotionBlurEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter qualityLevel;
        SerializedDataParameter shutterSpeed;

        SerializedDataParameter depthSeparationMode;

        SerializedDataParameter depthSeparationDistance;

        public override void OnEnable()
        {
            PropertyFetcher<BetterMotionBlur> o = new PropertyFetcher<BetterMotionBlur>(serializedObject);

            mode = Unpack(o.Find(x => x.mode));
            qualityLevel = Unpack(o.Find(x => x.qualityLevel));
            shutterSpeed = Unpack(o.Find(x => x.shutterSpeed));
            depthSeparationMode = Unpack(o.Find(x => x.depthSeparationMode));
            depthSeparationDistance = Unpack(o.Find(x => x.depthSeparationDistance));
        }

        public override void OnInspectorGUI()
        {
            PropertyField(mode);

            if (mode.value.intValue == (int)Mode.On)
            {
                PropertyField(qualityLevel, new GUIContent("Quality Level"));
                PropertyField(shutterSpeed, new GUIContent("Shutter Speed"));

                PropertyField(depthSeparationMode, new GUIContent("Depth Separation Mode"));
                if (depthSeparationMode.value.intValue == (int)Mode.On)
                {
                    PropertyField(depthSeparationDistance, new GUIContent("Depth Separation Distance"));
                }
            }
        }
    }
}
