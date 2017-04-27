
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnitedSolution
{
    public class BaseInspector : Editor
    {
        protected Dictionary<string, bool> showFoldOutFlags = new Dictionary<string, bool>();

        public override void OnInspectorGUI()
        {
            Space();
            DrawFoldOut("Show Default Inspector", () => DrawDefaultInspector());
        }

        protected void DrawFoldOut(string label, Action draw)
        {
            if (!showFoldOutFlags.ContainsKey(label))
            {
                showFoldOutFlags.Add(label, false);
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));
            showFoldOutFlags[label] = EditorGUILayout.Foldout(showFoldOutFlags[label], label);
            EditorGUILayout.EndHorizontal();
            if (showFoldOutFlags[label] && draw != null) draw();
        }
        protected void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        protected void BeginHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
        }

        protected void Space()
        {
            EditorGUILayout.Space();
        }

        protected void DrawObject(string label, UnityEngine.Object obj, bool allowObjectInScene = true)
        {
            obj = EditorGUILayout.ObjectField(label, obj, obj.GetType(), allowObjectInScene);
        }

        protected void DrawButton(string label, System.Action onClicked)
        {
            if (GUILayout.Button(label))
            {
                onClicked();
            }
        }

        protected void DrawTextfield(string label, ref string text)
        {
            text = EditorGUILayout.TextField(label, text);
        }

        protected void DrawLayer(string label, ref LayerMask customMask)
        {
            customMask = EditorGUILayout.LayerField(label, customMask.value);
        }

        protected void DrawToggle(string label, ref bool value)
        {
            value = EditorGUILayout.Toggle(label, value);
        }
        protected void DrawFloat(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value);
        }

        protected void DrawInt(string label, ref int value)
        {
            value = EditorGUILayout.IntField(label, value);
        }

        protected void DrawSlider(string label, ref float value, float min, float max)
        {
            value = EditorGUILayout.Slider(label, value, min, max);
        }
    }
}
