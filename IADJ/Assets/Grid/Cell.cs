using System;
using Grid;
using Pathfinding;
using UnityEditor;
using UnityEngine;

public class Cell
{
    private float _sizeX, _sizeZ;
    private Vector3 _center;
    private GameObject _collider;
    private bool _allowedCell;
    private int _coorX, _coorZ;

    public Cell(float sizeX, float sizeZ, Vector3 center, int coorX, int coorZ)
    {
        _sizeX = sizeX;
        _sizeZ = sizeZ;
        _center = center;
        _coorX = coorX;
        _coorZ = coorZ;
    }
    
    public Cell(Cell c)
    {
        _sizeX = c._sizeX;
        _sizeZ = c._sizeZ;
        _center = c._center;
        _coorX = c._coorX;
        _coorZ = c._coorZ;
    }
    

    public float GetSizeX()
    {
        return _sizeX;
    }

    public float GetSizeZ()
    {
        return _sizeZ;
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

    public bool CheckIfCellClicked(bool input)
    {
        if (input)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
            
           if (CheckIfHitsInsideBox(hits))
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

    private bool CheckIfHitsInsideBox(RaycastHit[] hits)
    {
        foreach (var h in hits)
        {
            float amplitudeX = _sizeX / 2;
            float amplitudeZ = _sizeZ / 2;
            Vector3 point = h.point;
            if (h.transform.CompareTag(GameObject.Find("Controlador").GetComponent<GridMap>().tagFloor) &&
                point.x > (_center.x - amplitudeX) &&
                point.x < (_center.x + amplitudeX) &&
                point.z > (_center.z - amplitudeZ) &&
                point.z < (_center.z + amplitudeZ)
            )
                return true;
        }
        return false;
    }

    public bool CheckIfVector3InsideBox(Vector3 entrante)
    {
        float amplitudeX = _sizeX / 2;
        float amplitudeZ = _sizeZ / 2;
        Vector3 point = entrante;
        if (
            point.x > (_center.x - amplitudeX) &&
            point.x < (_center.x + amplitudeX) &&
            point.z > (_center.z - amplitudeZ) &&
            point.z < (_center.z + amplitudeZ)
        )
            return true;
        return false;
    }
    
    
    public void DrawCell()
    {
        Handles.DrawWireCube(_center, new Vector3(_sizeX, 0,_sizeZ));
    }

    public void DrawCenter()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_center, 0.5f);
    }

    public void DrawCellColored(Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawCube(_center, new Vector3(_sizeX, 0,_sizeZ));
    }

    public void DrawCellNumber(int sizeOfTextGrid)
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.fontSize = sizeOfTextGrid;
        Vector3 pos = new Vector3(_center.x - _sizeX/2, _center.y, _center.z - _sizeZ/2);
        Handles.Label(pos,_coorX + ", " + _coorZ,style);
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

    public override String ToString()
    {
        return  _coorX + ", " + _coorZ;
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

    public bool Equals(Cell n)
    {
        return GetCoorX() == n.GetCoorX() && GetCoorZ() == n.GetCoorZ();
    }
}