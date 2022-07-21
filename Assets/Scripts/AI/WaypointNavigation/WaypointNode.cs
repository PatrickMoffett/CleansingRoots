using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AI.WaypointNavigation
{
    public class WaypointNode : BaseNavigationNode
    {
        private static int wpCount = 0;

        public float nodeConnectionRadius = 40.0f;
        public LayerMask nodeConnectionLayerMask = -1;
        public bool showDebug = false;
        public TMP_Text displayNumberText;
        
        [NonSerialized]public List<WaypointNode> connectedNodes = new List<WaypointNode>();
        [NonSerialized]public bool nodesToPlayerSet = false;

        private void Awake()
        {
            gameObject.name = "wp" + wpCount;
            wpCount++;
        }

        private void OnEnable()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, nodeConnectionRadius, LayerMask.GetMask("Waypoint"));

            RaycastHit rhInfo;
            for (int i = 0; i < colliders.Length; i++)
            {
                
                if(Physics.Raycast(transform.position,colliders[i].transform.position -transform.position,out rhInfo,nodeConnectionRadius,nodeConnectionLayerMask))
                {
                    if (showDebug)
                    {
                        Debug.Log(gameObject.name + " Hit: " + rhInfo.collider.gameObject.name);
                    }
                    if (rhInfo.collider.gameObject.CompareTag("WaypointNode"))
                    {
                        connectedNodes.Add(rhInfo.collider.gameObject.GetComponent<WaypointNode>());
                    }
                }
            }
        }
        
        private void Start()
        {
            ServiceLocator.Instance.Get<WaypointNavigationSystem>().RegisterWaypoint(this);
        }

        private void Update()
        {
            if (showDebug)
            {
                List<BaseNavigationNode> path = ServiceLocator.Instance.Get<WaypointNavigationSystem>().GetPath(this);
                string pathString = "";
                for (int i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(path[i-1].transform.position,path[i].transform.position,Color.green);
                }
            }
        }

        public void SetScore(int scoreToSet)
        {
            if (nodesToPlayerSet) return;
            nodesToPlayer = scoreToSet;
            nodesToPlayerSet = true;
            displayNumberText.text = "" + nodesToPlayer;
        }

        public void Reset()
        {
            nodesToPlayerSet = false;
            nodesToPlayer = Int32.MaxValue;
            displayNumberText.text = "?";
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            
            for (int i = 0; i < connectedNodes.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connectedNodes[i].gameObject.transform.position);
            }
        }
    }
}
