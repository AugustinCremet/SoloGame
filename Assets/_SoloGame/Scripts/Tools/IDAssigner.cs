#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class IDAssigner : EditorWindow
{
    [MenuItem("Tools/Assign Unique IDs")]
    public static void ShowWindow()
    {
        GetWindow(typeof(IDAssigner), false, "Unique ID Assigner");
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

        var identifiables = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

        foreach (var comp in identifiables)
        {
            if(comp is IUniqueIdentifier identifiable)
            {
                SerializedObject so = new SerializedObject(comp);
                SerializedProperty prop = so.FindProperty("_uniqueID");

                if (prop != null && string.IsNullOrEmpty(prop.stringValue))
                {
                    Undo.RecordObject(comp, "Assign Unique ID");
                    prop.stringValue = Guid.NewGuid().ToString();
                    so.ApplyModifiedProperties();
                    EditorUtility.SetDirty(comp);
                    assignedCount++;
                }
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
