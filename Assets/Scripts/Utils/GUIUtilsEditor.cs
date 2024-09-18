#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class GUIUtilsEditor
    {
        public static T Popup<T>(Rect position, GUIContent content, T selected, T[] options, GUIStyle guiStyle = null)
        {
            var color = GUI.color;

            if (options.Length == 0)
            {
                GUI.color = Color.red;

                var c = new GUIContent(content.text)
                {
                    text = $"{content.text} (Not Available)!",
                    tooltip = "Add items to options"
                };

                EditorGUI.TextField(position, c, $"{selected}");

                GUI.color = color;

                return selected;
            }

            var style = guiStyle ?? EditorStyles.popup;

            var index = Array.IndexOf(options, selected);

            GUI.color = index == -1 ? Color.red : color;

            if (index == -1)
                content.tooltip = $"Selected item {index} not found";

            index = EditorGUI.Popup(position, content, index, options.Select(i => new GUIContent(i.ToString())).ToArray(), style);

            GUI.color = color;

            return index == -1 ? selected : options[index];
        }
    }
}

#endif