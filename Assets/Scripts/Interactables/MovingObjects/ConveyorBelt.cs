using System.Collections.Generic;
using System.Linq;
using UnityEditor.TextCore.Text;
using UnityEngine;

namespace Interactables.MovingObjects
{
    public class ConveyorBelt : RespondsToSwitch
    {
        public enum ConveyorDirection
        {
            Forwards,
            Reverse
        }
        public enum SwitchResponse
        {
            ReverseDirection,
            TurnOff,
        }
        public GameObject conveyorBeltSegmentPrefab;
        public int numberOfSegments = 5;
        public float maxSpeed = 10f;
        public float acceleration = 3f;
        public float zeroSpeedTolerance = .01f;
        public ConveyorDirection defaultDirection = ConveyorDirection.Forwards;
        public SwitchResponse switchResponse = SwitchResponse.ReverseDirection;
        public bool switchState = true;
        public bool drawPreview = true;

        private readonly List<GameObject> _conveyorBeltSegments = new List<GameObject>();
        private Vector3 _meshSize;
        private float _currentSpeed = 0f;

        private void Awake()
        {
            
            Mesh segmentMesh = conveyorBeltSegmentPrefab.GetComponent<MeshFilter>().mesh;
            if (segmentMesh != null)
            {
                _meshSize = segmentMesh.bounds.size;
                _meshSize.x *= conveyorBeltSegmentPrefab.transform.localScale.x;
                _meshSize.y *= conveyorBeltSegmentPrefab.transform.localScale.y;
                _meshSize.z *= conveyorBeltSegmentPrefab.transform.localScale.z;
            }
            else
            {
                Debug.LogError("No Mesh Found on ConveyorBeltSegment");
                enabled = false;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            for (int i = numberOfSegments-1; i >= 0; i--)
            {
                GameObject segment = Instantiate(conveyorBeltSegmentPrefab, transform);
                segment.transform.position += Vector3.forward*_meshSize.z*i;
                _conveyorBeltSegments.Add(segment);
            }
        }

        private void Update()
        {
            //increment speed based on state
            if (switchResponse == SwitchResponse.TurnOff && !switchState)
            {
                if (Mathf.Abs(_currentSpeed) < zeroSpeedTolerance)
                {
                    _currentSpeed = 0;
                }else if (_currentSpeed > 0)
                {
                    _currentSpeed -= acceleration * Time.deltaTime;
                }
                else
                {
                    _currentSpeed += acceleration * Time.deltaTime;
                }
            }else if (defaultDirection == ConveyorDirection.Forwards && !switchState
                      || defaultDirection == ConveyorDirection.Reverse && switchState)
            {
                _currentSpeed += acceleration * Time.deltaTime;
            }
            else
            {
                _currentSpeed -= acceleration * Time.deltaTime;
            }
            
            /*
            switch (runningState)
            {
                case ConveyorRunningState.Forwards:
                    _currentSpeed += acceleration * Time.deltaTime;
                    break;
                case ConveyorRunningState.Reverse:
                    _currentSpeed -= acceleration * Time.deltaTime;
                    break;
                case ConveyorRunningState.Off:
                    if (_currentSpeed < zeroSpeedTolerance)
                    {
                        _currentSpeed = 0;
                    }else if (_currentSpeed > 0)
                    {
                        _currentSpeed -= acceleration * Time.deltaTime;
                    }
                    else
                    {
                        _currentSpeed += acceleration * Time.deltaTime;
                    }
                    break;
                default:
                    Debug.LogError("Unsupported ConveyorState Hit");
                    break;
            }
            */
            
            //clamp speed
            _currentSpeed = Mathf.Clamp(_currentSpeed, -maxSpeed, maxSpeed);
            
            //move each segment and wrap to other side if too far
            foreach (var segment in _conveyorBeltSegments)
            {
                segment.transform.localPosition += Vector3.forward * (_currentSpeed * Time.deltaTime);
                if (segment.transform.localPosition.z < 0)
                {
                    segment.transform.localPosition += new Vector3(0,0,_meshSize.z * numberOfSegments);
                }else if (segment.transform.localPosition.z > _meshSize.z * numberOfSegments)
                {
                    segment.transform.localPosition -= new Vector3(0,0,_meshSize.z * numberOfSegments);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (drawPreview)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
            
                //Draw ConveyorBelt Spawn Location
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.zero, _meshSize);
            
                //Draw ConveyorBelt Move Locations
                Gizmos.color = Color.blue;
                for (int i = 1; i < numberOfSegments; i++)
                {
                    Gizmos.DrawCube(Vector3.forward * _meshSize.z * i, _meshSize);
                }

                //Draw Last Location Before back to start
                Gizmos.color = Color.red;
                Gizmos.DrawCube(Vector3.forward * _meshSize.z * numberOfSegments, _meshSize);
            }
        }

        private void OnValidate()
        {
            Mesh segmentMesh = conveyorBeltSegmentPrefab.GetComponent<MeshFilter>().sharedMesh;
            if (segmentMesh != null)
            {
                _meshSize = segmentMesh.bounds.size;
                _meshSize.x *= conveyorBeltSegmentPrefab.transform.localScale.x;
                _meshSize.y *= conveyorBeltSegmentPrefab.transform.localScale.y;
                _meshSize.z *= conveyorBeltSegmentPrefab.transform.localScale.z;
            }
            else
            {
                Debug.LogError("No Mesh Found on ConveyorBeltSegment");
                enabled = false;
            }
        }

        public override void SwitchOn()
        {
            switchState = true;
        }

        public override void SwitchOff()
        {
            switchState = false;
        }

        public override void ToggleSwitch()
        {
            switchState = !switchState;
        }
    }
}
