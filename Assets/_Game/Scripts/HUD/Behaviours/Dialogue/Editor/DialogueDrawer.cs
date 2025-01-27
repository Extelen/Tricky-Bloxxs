using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Dialogue))]
public class DialoguePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var titleProp = property.FindPropertyRelative("m_title");
        var textProp = property.FindPropertyRelative("m_text");

        var showPictureProp = property.FindPropertyRelative("m_showPicture");

        EditorGUI.BeginProperty(position, label, property);

        var lineHeight = EditorGUIUtility.singleLineHeight;
        var yOffset = position.y;

        GUIContent foldoutLabel;


        if (showPictureProp.boolValue) foldoutLabel = new GUIContent("Picture");
        else
        {
            string str = titleProp.stringValue + ": " + textProp.stringValue;
            str = str.Substring(0, Mathf.Min(100, str.Length));
            str = str.Replace('\n', ' ');

            foldoutLabel = textProp.stringValue == "" ? label : new GUIContent(str);
        }

        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, yOffset, position.width, lineHeight), property.isExpanded, foldoutLabel);

        if (property.isExpanded)
        {
            // Pictures
            EditorGUILayout.PropertyField(showPictureProp, new GUIContent("Show Picture"));

            if (showPictureProp.boolValue)
            {
                var pictureProp = property.FindPropertyRelative("m_picture");
                EditorGUILayout.PropertyField(pictureProp);
            }

            else
            {
                EditorGUILayout.Space();

                // Text
                EditorGUILayout.PropertyField(titleProp);
                EditorGUILayout.PropertyField(textProp);
            }

            EditorGUILayout.Space();

            // Times
            var holdTimeProp = property.FindPropertyRelative("m_holdTime");
            EditorGUILayout.PropertyField(holdTimeProp, new GUIContent("Hold Time"));

            var nextOnTime = property.FindPropertyRelative("m_triggerNextOnTime");
            EditorGUILayout.PropertyField(nextOnTime, new GUIContent("Trigger next on time"));

            EditorGUILayout.Space();

            // Voice
            var hasVoiceProp = property.FindPropertyRelative("m_hasVoice");
            EditorGUILayout.PropertyField(hasVoiceProp, new GUIContent("Voice"));
            if (hasVoiceProp.boolValue)
            {
                var voiceProp = property.FindPropertyRelative("m_voice");
                EditorGUILayout.PropertyField(voiceProp, new GUIContent("Clip"));
                var waitUntilVoiceEndProp = property.FindPropertyRelative("m_waitUntilVoiceEnd");
                EditorGUILayout.PropertyField(waitUntilVoiceEndProp, new GUIContent("Wait Until End"));
                EditorGUILayout.Space();
            }

            // Events
            var executeEventsProp = property.FindPropertyRelative("m_executeEvents");
            EditorGUILayout.PropertyField(executeEventsProp, new GUIContent("Events"));

            if (executeEventsProp.boolValue)
            {
                var onStartProp = property.FindPropertyRelative("m_onStart");
                EditorGUILayout.PropertyField(onStartProp);

                var onEndProp = property.FindPropertyRelative("m_onEnd");
                EditorGUILayout.PropertyField(onEndProp);
            }
        }

        EditorGUI.EndProperty();
    }
}
