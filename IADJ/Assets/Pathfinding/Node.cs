using System;

namespace Pathfinding
{
    public class Node
    {
        private Cell _cell;
        
        // For all algorithms
        private float _hCost;
        
        // For A Star
        private float _gCost;
        private float _fCost;
        private Node _previousNode;
        
        // For LRTA
        private float _tempCost;
        private float _maxCost;
        
        public Node(Cell cell, float hCost, float gCost, Node previousNode)
        {
            _cell = new Cell(cell);
            _hCost = hCost;
            _gCost = gCost;
            _previousNode = previousNode;
        }

        public Node(Node n)
        {
            _cell = n.GetCell();
            _hCost = n.GetHCost();
            _gCost = n.GetGCost();
            _fCost = n.GetFCost();

            _previousNode = n.GetPreviousNode();
            _tempCost = n.GetTempCost();
            _maxCost = n.GetMaxCost();
        }

        public float CalculateFCost()
        {
            return _gCost + _fCost;
        }

        // Methods GET and SET
        public Cell GetCell()
        {
            return _cell;
        }

        public float GetGCost()
        {
            return _gCost;
        }

        public float GetHCost()
        {
            return _hCost;
        }

        public float GetFCost()
        {
            return _fCost;
        }

        public Node GetPreviousNode()
        {
            return _previousNode;
        }

        public float GetTempCost()
        {
            return _tempCost;
        }

        public float GetMaxCost()
        {
            return _maxCost;
        }

        public void SetMaxCost(float maxCost)
        {
            _maxCost = maxCost;
        }

        public void SetTempCost(float tempCost)
        {
            _tempCost = tempCost;
        }
        
        
        public void SetCell(Cell cell)
        {
            _cell = cell;
        }

        public void SetHCost(float hCost)
        {
            _hCost = hCost;
        }

        public void SetGCost(float gCost)
        {
            _gCost = gCost;
        }

        public void SetFCost(float fCost)
        {
            _fCost = fCost;
        }

        public void SetPreviousNode(Node node)
        {
            _previousNode = node;
        }

        public override string ToString()
        {
            
            return (_cell.ToString() + " CosteG:" + GetGCost() + "\nCosteH:" +GetHCost());
        }


        public bool Equals(Node n)
        {
            return _cell.Equals(n.GetCell());
        }
    }
}