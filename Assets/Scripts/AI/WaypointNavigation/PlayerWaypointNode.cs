using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.WaypointNavigation
{
    public class PlayerWaypointNode : BaseNavigationNode
    {
        public float nodeConnectionRadius = 40.0f;
        public LayerMask nodeConnectionLayerMask = -1;

        private void Awake()
        {
            nodesToPlayer = 0;
        }

        private void Start()
        {
            ServiceLocator.Instance.Get<WaypointNavigationSystem>().RegisterPlayerNode(this);
        }

        public void ScoreNodes()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, nodeConnectionRadius, LayerMask.GetMask("Waypoint"));

            RaycastHit rhInfo;
            List<WaypointNode> hitNodes = new List<WaypointNode>();
            
            for (int i = 0; i < colliders.Length; i++)
            {
                if(Physics.Raycast(transform.position,colliders[i].transform.position -transform.position,out rhInfo,nodeConnectionRadius,nodeConnectionLayerMask))
                {
                    if (rhInfo.collider.gameObject.CompareTag("WaypointNode"))
                    {
                        hitNodes.Add(rhInfo.collider.gameObject.GetComponent<WaypointNode>());
                    }
                }
            }
            
            Queue<WaypointNode> nodes = new Queue<WaypointNode>();
            //Score and Enqueue Initial Nodes;
            for (int i = 0; i < hitNodes.Count; i++)
            {
                hitNodes[i].SetScore(1);
                nodes.Enqueue(hitNodes[i]);
            }
            
            while (nodes.Count > 0)
            {
                WaypointNode currentNode = nodes.Dequeue();
                foreach (var childNode in currentNode.connectedNodes)
                {
                    if (childNode.nodesToPlayerSet) continue;
                    childNode.SetScore(currentNode.nodesToPlayer+1);
                    nodes.Enqueue(childNode);
                }
            }
        }
    }
}