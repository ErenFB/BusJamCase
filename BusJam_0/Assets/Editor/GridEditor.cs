using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(CreateGrid))]
public class GridEditor : Editor
{
    private Dictionary<string, int> buttonClickCounts = new Dictionary<string, int>();
    private Dictionary<string, GameObject> createdCharacters = new Dictionary<string, GameObject>();
    private const string ButtonClickCountsKey = "ButtonClickCounts";
    public PathfindingData pathfindingData;
    private void OnEnable()
    {
        LoadButtonClickCounts();
    }

    private void OnDisable()
    {
        SaveButtonClickCounts();
    }

    private void LoadButtonClickCounts()
    {
        string savedData = EditorPrefs.GetString(ButtonClickCountsKey, "");
        if (!string.IsNullOrEmpty(savedData))
        {
            string[] entries = savedData.Split(',');
            buttonClickCounts.Clear();
            foreach (string entry in entries)
            {
                string[] keyValue = entry.Split(':');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0];
                    int value;
                    if (int.TryParse(keyValue[1], out value))
                    {
                        buttonClickCounts[key] = value;
                    }
                }
            }
        }
    }

    private void SaveButtonClickCounts()
    {
        List<string> entries = new List<string>();
        foreach (var kvp in buttonClickCounts)
        {
            entries.Add($"{kvp.Key}:{kvp.Value}");
        }
        EditorPrefs.SetString(ButtonClickCountsKey, string.Join(",", entries));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CreateGrid createGrid = (CreateGrid)target;

        GUILayout.BeginHorizontal();
        GUILayout.Label("Final X");
         CreateGrid.FinalX = (int)EditorGUILayout.Slider(CreateGrid.FinalX, 1, 30);       
        GUILayout.Label("Final Y");
        CreateGrid.FinalY = (int)EditorGUILayout.Slider(CreateGrid.FinalY, 1, 30);
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Grid X");
        createGrid.x = (int)EditorGUILayout.Slider(createGrid.x, 1, 30);
        GUILayout.Label("Grid Y");
        createGrid.y = (int)EditorGUILayout.Slider(createGrid.y, 1, 30);
        GUILayout.EndVertical();


        if (GUILayout.Button("Create Grid"))
        {
            createGrid.CreateGrids();
            pathfindingData.SavePathfinding(createGrid.pathfinding); // Pathfinding verilerini kaydet
            EditorUtility.SetDirty(pathfindingData); // Deðiþiklikleri kaydet
            //pathfindingData.height = createGrid.x;
            //pathfindingData.width = createGrid.y;
            //EditorUtility.SetDirty(pathfindingData); // ScriptableObject deðiþikliklerini kaydet
        }
        if (GUILayout.Button("Delete Grid"))
        {
            createGrid.DeleteAll();
        }
        if (GUILayout.Button("Reset"))
        {
            createGrid.CreateCharacter(0, 0);
        }
        if (GUILayout.Button("ResetZERO"))
        {
            createGrid.CreateCharacterZERO();
        }

        for (int y = createGrid.y - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < createGrid.x; x++)
            {
                string buttonText = $"({x},{y})";
                string key = $"{x},{y}";

                if (buttonClickCounts.ContainsKey(key))
                {
                    buttonText = ((CharacterColor)buttonClickCounts[key]).ToString();
                }

                if (GUILayout.Button(buttonText, GUILayout.Width(70), GUILayout.Height(40)))
                {
                    if (!buttonClickCounts.ContainsKey(key))
                    {
                        buttonClickCounts[key] = 0;
                    }
                    else
                    {
                        buttonClickCounts[key]++;
                        if (buttonClickCounts[key] > (int)CharacterColor.Empty)
                        {
                            buttonClickCounts[key] = 0;
                        }
                    }

                    if (createdCharacters.ContainsKey(key) && createdCharacters[key] != null)
                    {
                        DestroyImmediate(createdCharacters[key]);
                        createdCharacters[key] = null;
                    }

                    GameObject newCharacter = createGrid.CreateCharacter(x, y);
                    createGrid.WalkableClose(x, y);
                    CharacterController characterController = newCharacter.GetComponent<CharacterController>();
                    newCharacter.GetComponent<CharacterPathfindingMovementHandler>().createGrid = createGrid;

                    if (characterController != null)
                    {
                        characterController.characterColor = (CharacterColor)buttonClickCounts[key];
                        
                        if (characterController.characterColor == CharacterColor.Empty || characterController.characterColor == CharacterColor.Destroy)
                        {
                            characterController.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            characterController.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                            characterController.gameObject.GetComponent<CapsuleCollider>().radius = 0;
                        }
                        else
                        {
                            characterController.UpdateColor();
                        }


                    }

                    createdCharacters[key] = newCharacter;

                    Repaint();
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
 