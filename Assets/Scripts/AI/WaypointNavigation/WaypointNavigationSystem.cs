using System;
using System.Collections;
using System.Collections.Generic;
using AI.BehaviorTree;
using UnityEngine;

namespace AI.WaypointNavigation
{
    public class WaypointNavigationSystem : IService
    {
        private List<WaypointNode> _nodes = new List<WaypointNode>();
        private LayerMask nodeConnectionLayerMask = 513;
        private PlayerWaypointNode _playerNode;
        private float updateRate = .2f;
        private Coroutine updateCoroutine;

        public WaypointNavigationSystem()
        {
            updateCoroutine = ServiceLocator.Instance.Get<MonoBehaviorService>().StartCoroutine(UpdateNav());
        }

        ~WaypointNavigationSystem()
        {
            if (updateCoroutine != null)
            {
                ServiceLocator.Instance.Get<MonoBehaviorService>().StopCoroutine(updateCoroutine);
            }
        }
        public void RegisterWaypoint(WaypointNode nodeToRegister)
        {
            _nodes.Add(nodeToRegister);
        }
        public void UnregisterWaypoint(WaypointNode nodeToUnregister)
        {
            _nodes.Remove(nodeToUnregister);
        }
        
        public void RegisterPlayerNode(PlayerWaypointNode playerWaypointNode)
        {
            _playerNode = playerWaypointNode;
        }

        private void ClearAllNodes()
        {
            foreach (var node in _nodes)
            {
                node.Reset();
            }
        }

        private IEnumerator UpdateNav()
        {
            while (true)
            {
                if (_playerNode != null && _nodes.Count > 0)
                {
                    ClearAllNodes();
                    _playerNode.ScoreNodes();
                }
                yield return new WaitForSeconds(updateRate);
            }
        }

        public List<BaseNavigationNode> GetPath(Vector3 startPosition)
        {
            Collider[] colliders = Physics.OverlapSphere(startPosition, 40f, LayerMask.GetMask("Waypoint"));
            RaycastHit rhInfo;
            BaseNavigationNode closestNode = null;
            int closestNodeDistance = Int32.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (Physics.Raycast(startPosition, colliders[i].transform.position - startPosition, out rhInfo, 40f,
                        nodeConnectionLayerMask))
                {
                    if (rhInfo.collider.gameObject.CompareTag("WaypointNode") || rhInfo.collider.gameObject.CompareTag("Player"))
                    {
                        var currentNode = rhInfo.collider.gameObject.GetComponent<BaseNavigationNode>();
                        if (currentNode.nodesToPlayer < closestNodeDistance)
                        {
                            closestNodeDistance = currentNode.nodesToPlayer;
                            closestNode = currentNode;
                        }
                    }
                }
            }

            return closestNode == null ? null : GetPath(closestNode);
        }
        public List<BaseNavigationNode> GetPath(BaseNavigationNode startNode)
        {
            if (startNode.nodesToPlayer == 0)
            {
                return new List<BaseNavigationNode> { startNode };
            }

            if (startNode.nodesToPlayer == 1)
            {
                return new List<BaseNavigationNode> { startNode, _playerNode };
            }
            
            if (startNode.nodesToPlayer == Int32.MaxValue)
            {
                Debug.LogWarning(startNode.name + " Node has no path connected to player");
                return null;
            }
            
            List<BaseNavigationNode> path = new List<BaseNavigationNode> { startNode };
            WaypointNode currentNode = (WaypointNode)startNode;
            while (currentNode.nodesToPlayer != 1)
            {
                for (int i = 0; i < currentNode.connectedNodes.Count; i++)
                {
                    if (currentNode.connectedNodes[i].nodesToPlayer < currentNode.nodesToPlayer)
                    {
                        currentNode = currentNode.connectedNodes[i];
                        path.Add(currentNode);
                        break;
                    }
                }
            }
            path.Add(_playerNode);
            return path;
        }
    }
}