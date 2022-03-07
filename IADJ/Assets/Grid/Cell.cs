using UnityEditor;
using UnityEngine;

public class Cell
{
    private float _sizeX, _sizeZ;
    private Vector3 _center;

    public Cell(float sizeX, float sizeZ, Vector3 center)
    {
        _sizeX = sizeX;
        _sizeZ = sizeZ;
        _center = center;
    }

    public float GetSizeX()
    {
        return _sizeX;
    }

    public float GetSizeZ()
    {
        return _sizeZ;
    }

    public Vector3 GetCenter()
    {
        return _center;
    }

    public bool CheckIfCellClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.CompareTag("Terrain") && CheckInsideBox(hit.point))
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public bool CheckInsideBox(Vector3 point)
    {
        float amplitudeX = _sizeX / 2;
        float amplitudeZ = _sizeZ / 2;

        if (point.x > (_center.x - amplitudeX) &&
            point.x < (_center.x + amplitudeX) &&
            point.z > (_center.z - amplitudeZ) &&
            point.z < (_center.z + amplitudeZ))
            return true;
        else
            return false;
    }

    public void DrawCell()
    {
        Handles.DrawWireCube(_center, new Vector3(_sizeX, 0,_sizeZ));
    }

    public void DrawCenter()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_center, 2);
    }

    public void DrawCellColored()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_center, new Vector3(_sizeX, 0,_sizeZ));
    }
    public void CreateBoxCollider(GameObject parent)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = _center;
        cube.transform.parent = parent.transform;
        cube.name = "CubeCollider";

        float sizeX = cube.GetComponent<Renderer>().bounds.size.x;
        float sizeZ = cube.GetComponent<Renderer>().bounds.size.z;

        var localScale = cube.transform.localScale;

        float rescaleX = localScale.x;
        rescaleX = _sizeX * rescaleX / sizeX;

        float rescaleZ = localScale.z;
        rescaleZ = _sizeZ * rescaleZ / sizeZ;

        localScale = new Vector3(rescaleX, 10, rescaleZ);

        cube.AddComponent<BoxCollider>();
        cube.GetComponent<Renderer>().enabled = false;
        cube.transform.localScale = localScale;
    }
}