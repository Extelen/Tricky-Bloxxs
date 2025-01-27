using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueManager))]
public class DialogueManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty useVariationsProp = serializedObject.FindProperty("m_useVariations");
        SerializedProperty dialogueControllerProp = serializedObject.FindProperty("m_dialogueController");
        SerializedProperty dialogueVariationsProp = serializedObject.FindProperty("m_dialogueVariations");

        EditorGUILayout.PropertyField(useVariationsProp, new GUIContent("Use Variations"));

        if (useVariationsProp.boolValue)
        {
            EditorGUILayout.PropertyField(dialogueVariationsProp, new GUIContent("Dialogue Variations"), true);
        }
        else
        {
            EditorGUILayout.PropertyField(dialogueControllerProp, new GUIContent("Dialogue Controller"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
