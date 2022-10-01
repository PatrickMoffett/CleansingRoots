using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
            
            Mesh segmentMesh = conveyorBeltSegmentPrefab.GetComponent<MeshFilter>().sharedMesh;
            if (segmentMesh != null)
            {
                _meshSize = segmentMesh.bounds.size;
                var localScale = conveyorBeltSegmentPrefab.transform.localScale;
                _meshSize.x *= localScale.x;
                _meshSize.y *= localScale.y;
                _meshSize.z *= localScale.z;
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
                segment.transform.localPosition += Vector3.forward*_meshSize.z * i;
                _conveyorBeltSegments.Add(segment);
            }
        }

        private void Update()
        {
            //increment speed based on state
            if (switchResponse == SwitchResponse.TurnOff && !switchState) //if switch is off and conveyorbelt should turn off
            {
                if (Mathf.Abs(_currentSpeed) < zeroSpeedTolerance) // if speed is almost zero, set to zero
                {
                    _currentSpeed = 0;
                }else if (_currentSpeed > 0) //else apply acceleration in opposite direction of motion
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
                _currentSpeed += acceleration * Time.deltaTime; // apply forward speed
            }
            else
            {
                _currentSpeed -= acceleration * Time.deltaTime; // apply reverse speed
            }        
            
            //clamp speed to max or negative max
            _currentSpeed = Mathf.Clamp(_currentSpeed, -maxSpeed, maxSpeed);
            
            //for each segment of conveyor belt
            foreach (var segment in _conveyorBeltSegments)
            {
                //apply speed, taking into account local scale
                segment.transform.localPosition += Vector3.forward * (_currentSpeed/transform.localScale.z * Time.deltaTime);
                
                //wrap segment if it has gone too far either direction
                if (segment.transform.localPosition.z < 0)
                {
                    segment.transform.localPosition += new Vector3(0,0,_meshSize.z * numberOfSegments);
                }else if (segment.transform.localPosition.z > _meshSize.z * numberOfSegments)
                {
                    segment.transform.localPosition -= new Vector3(0,0,_meshSize.z * numberOfSegments);
                }
            }
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (drawPreview && !EditorApplication.isPlaying)
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
                var localScale = conveyorBeltSegmentPrefab.transform.localScale;
                _meshSize.x *= localScale.x;
                _meshSize.y *= localScale.y;
                _meshSize.z *= localScale.z;
            }
            else
            {
                Debug.LogError("No Mesh Found on ConveyorBeltSegment");
                enabled = false;
            }
        }
#endif
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
