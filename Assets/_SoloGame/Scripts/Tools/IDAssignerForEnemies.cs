#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

public class IDAssignerForEnemies : EditorWindow
{
    [MenuItem("Tools/Assign Unique IDs to Enemies")]
    public static void ShowWindow()
    {
        GetWindow(typeof(IDAssignerForEnemies), false, "Enemy ID Assigner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Bulk Assign Unique IDs", EditorStyles.boldLabel);
        if (GUILayout.Button("Assign IDs in Current Scene"))
        {
            AssignIDsInScene();
        }
    }

    private void AssignIDsInScene()
    {
        int assignedCount = 0;

        var enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)
        {
            SerializedObject so = new SerializedObject(enemy);
            SerializedProperty prop = so.FindProperty("_uniqueID");

            if (string.IsNullOrEmpty(prop.stringValue))
            {
                Undo.RecordObject(enemy, "Assign Unique ID");
                prop.stringValue = Guid.NewGuid().ToString();
                so.ApplyModifiedProperties();
                EditorUtility.SetDirty(enemy);
                assignedCount++;
            }
        }

        if (assignedCount > 0)
        {
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            Debug.Log($"Assigned {assignedCount} unique IDs.");
        }
        else
        {
            Debug.Log("All enemies already have unique IDs.");
        }
    }
}
#endif
