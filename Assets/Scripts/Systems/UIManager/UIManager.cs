using System.Collections.Generic;
using Constants;
using UnityEngine;

public class UIManager : IService
{
    public enum UILayer
    {
        UI,
        LOADING,
        ERROR,
    }

    private static readonly string ROOT_PREFAB = UIPrefabs.RootUI;
    private Transform[] _layerTransforms;
    private GameObject _rootObject;
    private Dictionary<string, UIWidget> _uiWidgets = new Dictionary<string, UIWidget>();

    public UIManager()
    {
        // Instantiate Root UI Canvas
        _rootObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>(ROOT_PREFAB));
        Object.DontDestroyOnLoad(_rootObject);
        _layerTransforms = new Transform[3];

        for (int i = 0; i < _layerTransforms.Length; i++)
        {
            _layerTransforms[i] = _rootObject.transform.GetChild(i);
        }
    }

    public UIWidget LoadUI(string uiPrefab, UILayer uICategory = UILayer.UI)
    {
        GameObject go = Object.Instantiate<GameObject>(Resources.Load<GameObject>(uiPrefab), _layerTransforms[(int)uICategory]);
        string guid = System.Guid.NewGuid().ToString();
        UIWidget uiWidget = new UIWidget(guid, go);
        _uiWidgets[guid] = uiWidget;
        return uiWidget;
    }

    public void RemoveUIByGuid(string guid)
    {
        if (_uiWidgets.ContainsKey(guid))
        {
            Object.Destroy(_uiWidgets[guid].UIObject);
            _uiWidgets.Remove(guid);
        }
    }
}
