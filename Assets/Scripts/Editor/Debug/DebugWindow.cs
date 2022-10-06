#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DebugWindow : EditorWindow
{
    [MenuItem("Cleansing Roots/Debug Window")]
    public static void ShowExample()
    {
        DebugWindow wnd = GetWindow<DebugWindow>();
        wnd.titleContent = new GUIContent("Debug Window");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Debug/DebugWindow.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);
        
        rootVisualElement.Query<Button>("boss").First().RegisterCallback<ClickEvent>(GotoBoss);
        rootVisualElement.Query<Button>("stairs").First().RegisterCallback<ClickEvent>(GotoStairs);
        
    }

    private void GotoBoss(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            findPlayer().transform.position = new Vector3(-5.3f,25.45f,695.5f);

        }
    }

    private void GotoStairs(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            findPlayer().transform.position = new Vector3(304,2,99);
        }
        
    }

    private GameObject findPlayer()
    {
        return GameObject.FindWithTag("Player");
    }
}
#endif