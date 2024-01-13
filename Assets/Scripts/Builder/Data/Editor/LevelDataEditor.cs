using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelData levelData = (LevelData)target;

        if (GUILayout.Button("Save"))
        {
            levelData.Save();
        }

        if (GUILayout.Button("Load"))
        {
            levelData.Load();
        }
    }
}
