using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomColourPicker))]
public class CustomColourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RandomColourPicker colourPicker = (RandomColourPicker)target;

        if(GUILayout.Button("Generate Colours"))
        {
            colourPicker.GenerateColours();
        }
    }
}