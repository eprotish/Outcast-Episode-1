using UnityEngine;
using UnityEditor;


#if UNITY_EDITOR
public class EditorTab : EditorWindow
{
    private string[] a;
    private string[] b;

    [MenuItem("Window/BarootTab")]
    public static void ShowWindow ()
    {
        GetWindow<EditorTab>("BarootTab");
    }

    private void OnGUI()
    {       
        if (GUILayout.Button("Clear Inventory"))
        {
             GameObject.FindObjectOfType<GameDataController>().gameData.itemIds.Clear();

              Debug.Log("Clear Inventory Done");  
        }

        if (GUILayout.Button("Clear Step"))
        {
            for (var i = 0; i < 70; i++)
            {
                GameObject.FindObjectOfType<GameDataController>().gameData.steps[i] = false;
            }

           Debug.Log("Clear Step Done");   
        } 

        
    }
}
#endif
