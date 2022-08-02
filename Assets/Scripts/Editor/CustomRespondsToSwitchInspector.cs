using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RespondsToSwitch),true)]
public class CustomRespondsToSwitchInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        RespondsToSwitch switchable = (RespondsToSwitch)target;

        if (GUILayout.Button("Switch On"))
        {
            switchable.SwitchOn();
        }
        if (GUILayout.Button("Switch Off"))
        {
            switchable.SwitchOff();
        }
        if (GUILayout.Button("Toggle Switch"))
        {
            switchable.ToggleSwitch();
        }
    }
}
