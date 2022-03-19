namespace Pathfinding
{
    public class Node
    {
        private Cell _cell;
        private int _hCost;
        private int _gCost;
        private int _fCost;
        private Node _previousNode;

        public Node(Cell cell, int hCost, int gCost, Node previousNode)
        {
            _cell = new Cell(cell);
            _hCost = hCost;
            _gCost = gCost;
            _previousNode = previousNode;
        }

        public Node(Node n)
        {
            _cell = new Cell(n.GetCell());
            _hCost = n.GetHCost();
            _gCost = n.GetGCost();
            _fCost = n.GetFCost();
            _previousNode = n.GetPreviousNode();
        }

        public int CalculateFCost()
        {
            _fCost = _gCost + _hCost;
            return _fCost;
        }

        // Methods GET and SET
        public Cell GetCell()
        {
            return _cell;
        }

        public int GetGCost()
        {
            return _gCost;
        }

        public int GetHCost()
        {
            return _hCost;
        }

        public int GetFCost()
        {
            return _fCost;
        }

        public Node GetPreviousNode()
        {
            return _previousNode;
        }

        public void SetCell(Cell cell)
        {
            _cell = cell;
        }

        public void SetHCost(int hCost)
        {
            _hCost = hCost;
        }

        public void SetGCost(int gCost)
        {
            _gCost = gCost;
        }

        public void SetFCost(int fCost)
        {
            _fCost = fCost;
        }

        public void SetPreviousNode(Node node)
        {
            _previousNode = node;
        }

        public override string ToString()
        {
            return _cell.ToString();
        }
    }
}