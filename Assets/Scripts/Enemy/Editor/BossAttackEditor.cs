using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BossAttack))]
public class BossAttackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackNo"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldown"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("baseWeight"));

        EditorGUILayout.Space();
        DrawList(serializedObject.FindProperty("conditions"), typeof(AttackCondition), "Conditions (all must be true)");

        EditorGUILayout.Space();
        DrawList(serializedObject.FindProperty("modifiers"), typeof(WeightModifier), "Weight Modifiers");

        serializedObject.ApplyModifiedProperties();
    }

    private static void DrawList(SerializedProperty list, Type baseType, string label)
    {
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        int removeIndex = -1;
        for (int i = 0; i < list.arraySize; i++)
        {
            if (DrawEntry(list.GetArrayElementAtIndex(i))) removeIndex = i;
        }
        if (removeIndex >= 0) list.DeleteArrayElementAtIndex(removeIndex);

        DrawAddButton(list, baseType);
    }

    private static void DrawSingle(SerializedProperty prop, Type baseType)
    {
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(prop.name), EditorStyles.boldLabel);

        if (prop.managedReferenceValue == null)
            DrawAddButton(prop, baseType);
        else if (DrawEntry(prop))
            prop.managedReferenceValue = null;
    }

    // Returns true if the remove button was clicked
    private static bool DrawEntry(SerializedProperty element)
    {
        bool remove = false;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();

        string title = element.managedReferenceValue == null
            ? "(empty - remove me)"
            : ObjectNames.NicifyVariableName(element.managedReferenceValue.GetType().Name);

        element.isExpanded = EditorGUILayout.Foldout(element.isExpanded, title, true);
        if (GUILayout.Button("X", GUILayout.Width(22))) remove = true;
        EditorGUILayout.EndHorizontal();

        if (element.isExpanded && element.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            DrawChildren(element);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndVertical();
        return remove;
    }

    private static void DrawChildren(SerializedProperty element)
    {
        SerializedProperty end = element.GetEndProperty();
        SerializedProperty child = element.Copy();
        bool enter = true;

        while (child.NextVisible(enter) && !SerializedProperty.EqualContents(child, end))
        {
            enter = false;

            if (child.isArray && child.name == "conditions")
                DrawList(child.Copy(), typeof(AttackCondition), "Conditions");
            else if (child.propertyType == SerializedPropertyType.ManagedReference && child.name == "condition")
                DrawSingle(child.Copy(), typeof(AttackCondition));
            else
                EditorGUILayout.PropertyField(child, true);
        }
    }

    private static void DrawAddButton(SerializedProperty prop, Type baseType)
    {
        if (!GUILayout.Button("+ Add " + ObjectNames.NicifyVariableName(baseType.Name))) return;

        SerializedObject so = prop.serializedObject;
        string path = prop.propertyPath;
        bool isList = prop.isArray;

        var menu = new GenericMenu();
        foreach (Type type in TypeCache.GetTypesDerivedFrom(baseType).Where(t => !t.IsAbstract))
        {
            Type captured = type;
            menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(type.Name)), false, () =>
            {
                so.Update();
                SerializedProperty p = so.FindProperty(path);

                if (isList)
                {
                    p.arraySize++;
                    p.GetArrayElementAtIndex(p.arraySize - 1).managedReferenceValue = Activator.CreateInstance(captured);
                }
                else
                {
                    p.managedReferenceValue = Activator.CreateInstance(captured);
                }

                so.ApplyModifiedProperties();
            });
        }
        menu.ShowAsContext();
    }
}