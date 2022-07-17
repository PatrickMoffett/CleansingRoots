using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviorTree
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        private BaseNode _root = null;

        private Dictionary<string, object> _contextData = new Dictionary<string, object>();

        protected void Start()
        {
            _root = SetupTree();
            _root?.SetOwningTree(this);
        }

        protected abstract BaseNode SetupTree();

        private void Update()
        {
            _root.Run();
        }

        public void SetData(string key, object value)
        {
            _contextData[key] = value;
        }

        public bool HasData(string key)
        {
            return _contextData.ContainsKey(key);
        }
        public object GetData(string key)
        {
            return _contextData.ContainsKey(key) ? _contextData[key] : null;
        }

        public bool ClearData(string key)
        {
            return _contextData.ContainsKey(key) && _contextData.Remove(key);
        }
    }
}