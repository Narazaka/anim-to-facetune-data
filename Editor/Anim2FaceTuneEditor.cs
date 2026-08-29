using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

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
            EnsureTypes();
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
                    
                    var expressionData = go.AddComponent(ExpressionDataComponent);
                    var facialBlendShapes = FacialBlendShapes.GetValue(expressionData);
                    if (facialBlendShapes == null)
                    {
                        facialBlendShapes = Activator.CreateInstance(FacialBlendShapeData);
                        FacialBlendShapes.SetValue(expressionData, facialBlendShapes);
                    }
                    Clip.SetValue(facialBlendShapes, clip);
                    
                    go.transform.SetParent(transform, false);
                    created.Add(go);
                }
                clips.ClearArray();
                EditorGUIUtility.PingObject(created[0]);
            }
            serializedObject.ApplyModifiedProperties();
        }

        static Type ExpressionDataComponent;
        static Type FacialBlendShapeData;
        static FieldInfo FacialBlendShapes;
        static FieldInfo Clip;

        public static void EnsureTypes()
        {
            if (ExpressionDataComponent != null) return;
            ExpressionDataComponent = Type.GetType("Aoyon.FaceTune.ExpressionDataComponent, Aoyon.FaceTune.Runtime");
            FacialBlendShapeData = Type.GetType("Aoyon.FaceTune.FacialBlendShapeData, Aoyon.FaceTune.Runtime");
            FacialBlendShapes = ExpressionDataComponent.GetField("FacialBlendShapes", BindingFlags.Instance | BindingFlags.Public);
            Clip = FacialBlendShapeData.GetField("Clip", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
