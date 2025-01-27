using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Timer))]
public class TimerDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty minutesProperty = property.FindPropertyRelative("m_defaultMinutes");
        SerializedProperty secondsProperty = property.FindPropertyRelative("m_defaultSeconds");

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        float labelWidth = 12;
        float fieldWidth = 30;
        float separationWidth = 4;

        // Minutes
        float currentX = position.x;
        Rect minutesLabelRect = new Rect(currentX, position.y, labelWidth, position.height);
        EditorGUI.LabelField(minutesLabelRect, "M");

        currentX += labelWidth + separationWidth;
        Rect minutesFieldRect = new Rect(currentX, position.y, fieldWidth, position.height);

        minutesProperty.intValue = EditorGUI.IntField(minutesFieldRect, GUIContent.none, minutesProperty.intValue);
        minutesProperty.intValue = Mathf.Max(0, minutesProperty.intValue);

        // Seconds
        labelWidth = 18;
        currentX += fieldWidth + separationWidth;
        Rect separatorRect = new Rect(currentX, position.y, labelWidth, position.height);
        EditorGUI.LabelField(separatorRect, ": S");

        currentX += labelWidth + separationWidth;
        Rect secondsFieldRect = new Rect(currentX, position.y, fieldWidth, position.height);
        secondsProperty.intValue = EditorGUI.IntField(secondsFieldRect, GUIContent.none, secondsProperty.intValue);
        secondsProperty.intValue = Mathf.Clamp(secondsProperty.intValue, 0, 59);

    }
}
