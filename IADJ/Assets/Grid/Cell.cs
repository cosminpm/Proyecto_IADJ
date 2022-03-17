using Grid;
using UnityEditor;
using UnityEngine;

public class Cell
{
    private float _sizeX, _sizeZ;
    private Vector3 _center;
    private GameObject _collider;
    private bool _allowedCell;
    private int _coorX, _coorZ, _cost;

    public Cell(float sizeX, float sizeZ, Vector3 center, int coorX, int coorZ)
    {
        _sizeX = sizeX;
        _sizeZ = sizeZ;
        _center = center;
        _coorX = coorX;
        _coorZ = coorZ;
        _cost = int.MaxValue;
    }

    public float GetSizeX()
    {
        return _sizeX;
    }

    public float GetSizeZ()
    {
        return _sizeZ;
    }

    public int GetCost()
    {
        return _cost;
    }

    public void SetCost(int cost)
    {
        _cost = cost;
    }

    public bool GetIsAllowedCell()
    {
        return _allowedCell;
    }

    public int GetCoorX()
    {
        return _coorX;
    }
    
    public int GetCoorZ()
    {
        return _coorZ;
    }
    
    public Vector3 GetCenter()
    {
        return _center;
    }

    public bool CheckIfCellClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
           
           
           if (CheckVector3InsideBox(hits))
            {
                return true;
            }
            else
            {
                return false;  
            }
               
        }
        else
            return false;
    }

    private bool CheckVector3InsideBox(RaycastHit[] hits)
    {
        foreach (var h in hits)
        {
            float amplitudeX = _sizeX / 2;
            float amplitudeZ = _sizeZ / 2;
            Vector3 point = h.point;
            if (h.transform.CompareTag("Terrain") &&
                point.x > (_center.x - amplitudeX) &&
                point.x < (_center.x + amplitudeX) &&
                point.z > (_center.z - amplitudeZ) &&
                point.z < (_center.z + amplitudeZ)
            )
                return true;
        }

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

    public void DrawCellColored(Color color)
    {
        Gizmos.color = color;
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
        _collider = cube;
    }

    public bool CheckCollision()
    {
        Collider[] colls = Physics.OverlapBox(_center, new Vector3(_sizeX, 1, _sizeZ) / 2);
        foreach (var c in colls)
        {
            if (c.transform.CompareTag("Untagged"))
            {
                _allowedCell = false;
                return true;
            }
               
        }
        _allowedCell = true;
        return false;
    }

}