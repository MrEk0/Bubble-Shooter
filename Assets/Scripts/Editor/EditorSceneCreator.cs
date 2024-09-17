using Game.Level;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelDataLoader))]
    public class EditorSceneCreator : UnityEditor.Editor
    {
        private LevelDataLoader _levelDataLoader;

        private void Awake()
        {
            _levelDataLoader = target as LevelDataLoader;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (GUILayout.Button("LoadFromGoogle"))
            {
                _levelDataLoader.LoadFromGoogle();
            }
            
            if (GUILayout.Button("OpenForEdit"))
            {
                _levelDataLoader.OpenForEdit();
            }
        }
    }
}