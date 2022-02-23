using System;
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
    
}