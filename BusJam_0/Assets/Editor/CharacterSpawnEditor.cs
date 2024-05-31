using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(CharacterController))]
public class CharacterSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CharacterController controller = (CharacterController)target;
        if (GUILayout.Button("Delete Grid"))
        {
            controller.UpdateColor();
        }
    }
}
