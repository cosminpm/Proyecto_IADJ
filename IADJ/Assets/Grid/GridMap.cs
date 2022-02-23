
using UnityEditor;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Grid
{
    public class GridMap : MonoBehaviour
    {
        public int xSize, zSize;
        public Cell[,] gridMap;
        private bool _initialized;

        void Start()
        {
            CreateGrid();
            GridMapToString();
            _initialized = true;
        }

        void Update()
        {
        }


        private void CreateGrid()
        {
            gridMap = new Cell[xSize, zSize];
            float[] sizeOfMap = GetSizeOfPlane();
            float[] sizeOfCell = GetSizeOfCell();

            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    float x = i * sizeOfCell[0] - sizeOfMap[0] / 2 + sizeOfCell[0] / 2;
                    float z = j * sizeOfCell[1] - sizeOfMap[1] / 2 + sizeOfCell[1] / 2;
                    Vector3 center = new Vector3(x, 0, z);
                    gridMap[i, j] = new Cell(sizeOfCell[0], sizeOfCell[1], center);
                }
            }
        }

        private float[] GetSizeOfPlane()
        {
            float x = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size.x;
            float z = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size.z;
            return new[] {x, z};
        }


        public float[] GetSizeOfCell()
        {
            float x = GetSizeOfPlane()[0] / xSize;
            float z = GetSizeOfPlane()[1] / zSize;
            return new[] {x, z};
        }

        private void GridMapToString()
        {
            Debug.Log("Start of Grid");
            for (int i = 0; i < xSize; i++)
            {
                Debug.Log("ROW " + i);
                for (int j = 0; j < zSize; j++)
                {
                    Debug.Log(gridMap[i, j].GetCenter());
                }
            }

            Debug.Log("Finish of Map");
        }

        private void OnDrawGizmos()
        {
            if (_initialized)
                DrawGridMap();
        }

        private void DrawGridMap()
        {
            Handles.color = Color.black;
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    Handles.DrawWireCube(gridMap[i, j].GetCenter(),
                        new Vector3(gridMap[i, j].GetSizeX(), 0, gridMap[i, j].GetSizeZ()));
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(gridMap[i, j].GetCenter(), 5);
                }
            }
        }
    }
}