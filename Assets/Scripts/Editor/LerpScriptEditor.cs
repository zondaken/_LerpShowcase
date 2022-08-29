using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LerpScript))]
public class LerpScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var lerpScript = (LerpScript) target;
        var property = serializedObject.GetIterator();

        for (bool enterChildren = true; property.NextVisible(enterChildren); enterChildren = false)
        {
            if (lerpScript.type != LerpScript.LerpType.AnimationCurve &&
                (property.propertyPath is "animationCurve" or "lerpUnclamped"))
                continue;
            
            using (new EditorGUI.DisabledScope(property.propertyPath == "m_Script"))
            {
                EditorGUILayout.PropertyField(property, true);
            }
        }
            
        serializedObject.ApplyModifiedProperties();
    }
}