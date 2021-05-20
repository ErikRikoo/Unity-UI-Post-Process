using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UIPostProcess.Editor
{
    [CustomEditor(typeof(Runtime.UIPostProcess))]
    public class UIPostProcessEditor : UnityEditor.Editor
    {
        private SerializedProperty m_ObjectToMoveProperty;
        
        private void OnEnable()
        {
            m_ObjectToMoveProperty = serializedObject.FindProperty("m_CanvasElementToMove");
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        private void OnSceneSaved(Scene scene)
        {
            Target?.ApplyProperties();
        }

        public Runtime.UIPostProcess Target => target as Runtime.UIPostProcess;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space(10f);
            Target.m_EditorToolsFoldoutOpened = EditorGUILayout.Foldout(
                Target.m_EditorToolsFoldoutOpened,
                "Editor Tools"
            );

            if (Target.m_EditorToolsFoldoutOpened)
            {
                EditorGUILayout.PropertyField(m_ObjectToMoveProperty);
                serializedObject.ApplyModifiedProperties();
                Target.m_SnapElementAnchorsToggleValue = EditorGUILayout.Toggle(
                    "Should Element Anchor be snaped?",
                    Target.m_SnapElementAnchorsToggleValue
                );

                if (Target.m_CanvasElementToMove == null)
                {
                    GUI.enabled = false;
                } else if (!Target.CheckObjectToMoveValidity())
                {
                    EditorGUILayout.HelpBox(OnObjectToMoveNullMessage, MessageType.Error);
                    GUI.enabled = false;
                }

                if (GUILayout.Button("Make object canvas child"))
                {
                    Target.MakeSelectedObjectCanvasChild();
                }

                GUI.enabled = true;
            }
        }

        private string OnObjectToMoveNullMessage =>
            "The object should be an object from the scene and not a child of the UIPostProcess element";
    }
}