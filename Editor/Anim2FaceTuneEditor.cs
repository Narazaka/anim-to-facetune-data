using Aoyon.FaceTune;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Narazaka.VRChat.Anim2FaceTune.Editor
{
    [CustomEditor(typeof(Anim2FaceTune))]
    public class Anim2FaceTuneEditor : UnityEditor.Editor
    {
        SerializedProperty clips;

        void OnEnable()
        {
            clips = serializedObject.FindProperty("clips");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component converts AnimationClips to FaceTune ExpressionDataComponents as child GameObjects when clips are assigned.", MessageType.Info);
            serializedObject.UpdateIfRequiredOrScript();
            EditorGUILayout.PropertyField(clips, true);
            if (clips.arraySize > 0)
            {
                var transform = (target as MonoBehaviour).transform;
                var created = new List<GameObject>();
                for (var i = 0; i < clips.arraySize; i++)
                {
                    var clipProp = clips.GetArrayElementAtIndex(i);
                    var clip = clipProp.objectReferenceValue as AnimationClip;
                    if (clip == null) continue;
                    var go = new GameObject(clip.name);
                    var expressionData = go.AddComponent<ExpressionDataComponent>();
                    expressionData.Clip = clip;
                    go.transform.SetParent(transform, false);
                    created.Add(go);
                }
                clips.ClearArray();
                EditorGUIUtility.PingObject(created[0]);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
