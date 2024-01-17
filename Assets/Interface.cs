using UnityEditor;
using UnityEngine;

public class Interface : EditorWindow
{
    private int numberOfPoints = 0;
    private int numberOfGeneration = 0; 
    private AlgoGénétique scriptAlgo;

    [MenuItem("Window/Algorithme genetique")]
    public static void ShowWindow()
    {
        GetWindow<Interface>("Algorithme genetique");
    }

    private void OnEnable()
    {
        
        scriptAlgo = FindObjectOfType<AlgoGénétique>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Editor Window", EditorStyles.boldLabel);

        numberOfPoints = EditorGUILayout.IntSlider("Number of points", numberOfPoints, 0, 100);
        numberOfGeneration = EditorGUILayout.IntSlider("Number of generation", numberOfGeneration, 0, 500); 
        
        if (GUILayout.Button("Apply"))
        {
  
            if (scriptAlgo != null)
            {
                scriptAlgo.setPoints(numberOfPoints);
                scriptAlgo.setGeneration(numberOfGeneration);
            }
            else
            {
                Debug.LogError("Script cible introuvable");
            }
        }
    }
}