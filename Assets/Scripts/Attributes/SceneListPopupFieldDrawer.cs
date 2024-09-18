#if UNITY_EDITOR

using System.Linq;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Attributes
{
    [CustomPropertyDrawer(typeof(SceneListPopupFieldAttribute))]
    public class SceneListPopupFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scenes = EditorBuildSettings.scenes.Where(i => i.enabled).Select(i => i.path).ToArray();
            property.stringValue = GUIUtilsEditor.Popup(position, label, property.stringValue, scenes);
        }
    }
}

#endif