using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FindObjectReference : EditorWindow
{
    private static GameObject selectedObject = null; // Selected game object for finding reference
    private List<GameObject> referenceBindObjects = new List<GameObject>(); // List of GameObjects that has selected game object reference

    private static bool autoFind; // Set flag to find reference from hierarchy

    // Open editor window from menu section
    [MenuItem("Window/Custom Tool/Check Reference")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FindObjectReference));
    }

    // Open window from selected game object
    [MenuItem("GameObject/Find Reference", false, -10)]
    public static void ShowWindowInGameObject()
    {
        selectedObject = Selection.activeGameObject;
        autoFind = true;
        GetWindow(typeof(FindObjectReference));
    }

    private void OnGUI()
    {
        selectedObject = EditorGUILayout.ObjectField("Find Reference", selectedObject, typeof(GameObject), true) as GameObject;
        if (selectedObject)
        {
            // Storing all the component fo referenced object to find reference
            var components = selectedObject.GetComponents(typeof(Component));


            if (GUI.Button(new Rect(3, 25, position.width - 6, 20), "Check Reference") || autoFind)
            {
                referenceBindObjects = new List<GameObject>();
                var objList = FindObjectsOfType<MonoBehaviour>();

                foreach (var obj in objList)
                {
                    // Storing aal fields values to match the reference object 
                    var fInfos = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (var fInfo in fInfos)
                    {
                        var data = fInfo.GetValue(obj) as Object;
                        if (data == null) continue;
                        if (CheckForInstanceID(components, data)) referenceBindObjects.Add(obj.gameObject);
                    }
                }

                autoFind = false;
            }

            if (referenceBindObjects.Count > 0)
            {
                DrawReferencesData();
            }
            else
            {
                EditorGUILayout.Space(25);
                EditorGUILayout.LabelField("No Reference found");
            }
        }
        else
        {
            EditorGUILayout.LabelField("Please add GameObject in input field!");
        }
    }

    private void DrawReferencesData()
    {
        EditorGUILayout.Space(25);
        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        var i = 1; // Used for indexing to show in window
        foreach (var sObj in referenceBindObjects)
        {
            EditorGUILayout.ObjectField(i.ToString(), sObj, typeof(Object), true);
            i++;
        }

        EditorGUILayout.EndVertical();
    }

    private static bool CheckForInstanceID(Component[] components, Object data)
    {
        return components.Any(tmpComp => tmpComp.GetInstanceID() == data.GetHashCode());
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}