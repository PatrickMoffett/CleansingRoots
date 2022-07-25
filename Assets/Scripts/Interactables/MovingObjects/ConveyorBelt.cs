using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public GameObject conveyorBeltSegmentPrefab;
    public int numberOfSegments = 5;
    public float speed = 10f;

    private Queue<GameObject> conveyorBeltSegments = new Queue<GameObject>();
    private Vector3 _meshSize;
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
            conveyorBeltSegments.Enqueue(segment);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        foreach (var segment in conveyorBeltSegments)
        {
            segment.transform.position += Vector3.forward * (speed * Time.deltaTime);
        }

        if (conveyorBeltSegments.Peek().transform.localPosition.z > _meshSize.z * numberOfSegments)
        {
            GameObject segment = conveyorBeltSegments.Dequeue();
            segment.transform.position = transform.position;
            conveyorBeltSegments.Enqueue(segment);
        }
    }

    private void OnDrawGizmos()
    {
        //Draw ConveyorBelt Spawn Location
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position,_meshSize);
        
        //Draw ConveyorBelt Move Locations
        Gizmos.color = Color.blue;
        for (int i = 1; i < numberOfSegments; i++)
        {
            Gizmos.DrawCube(transform.position+Vector3.forward*_meshSize.z*i,_meshSize);
        }
        
        //Draw Last Location Before back to start
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position+Vector3.forward*_meshSize.z*numberOfSegments,_meshSize);
    }

    private void OnValidate()
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
}
