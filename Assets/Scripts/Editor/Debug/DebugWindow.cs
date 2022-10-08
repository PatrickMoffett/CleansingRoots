#if UNITY_EDITOR

using Combat.Boss;
using Player;
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
        rootVisualElement.Query<Button>("killBoss").First().RegisterCallback<ClickEvent>(EndBossFight);
        rootVisualElement.Query<Button>("giveMaxAmmo").First().RegisterCallback<ClickEvent>(GiveMaxAmmo);
        rootVisualElement.Query<Button>("togglePlayerDamage").First().RegisterCallback<ClickEvent>(TogglePlayerDamage);

    }

    private void GotoBoss(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            findPlayer().transform.position = new Vector3(-18, 27.5f, 746.5f);

        }
    }
    private void TogglePlayerDamage(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            var health = findPlayer().GetComponent<Health>();
            health.canTakeDamage = !health.canTakeDamage;
        }
    }
    private void GiveMaxAmmo(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            var character = findPlayer().GetComponent<PlayerCharacter>();
            character.SetEffectivelyUnlimitedAmmo();
        }
    }

    private void GotoStairs(ClickEvent evt)
    {
        if (EditorApplication.isPlaying)
        {
            findPlayer().transform.position = new Vector3(304, 2, 99);
        }
    }

    private void EndBossFight(ClickEvent evt)
    {
        FindObjectOfType<FactoryBoss>().EndFight();
    }

    private GameObject findPlayer()
    {
        return GameObject.FindWithTag("Player");
    }
}
#endif