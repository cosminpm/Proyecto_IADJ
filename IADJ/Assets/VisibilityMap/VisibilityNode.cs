using Global;
using Grid;
using UnityEditor;
using UnityEngine;


    public class VisibilityNode
    {
        private int _teamInfluence;
        private float _valueOfVisibility;
            
            
        public VisibilityNode()
        {
            _teamInfluence = GlobalAttributes.CELDA_INFLUENCIA_NADIE;
            _valueOfVisibility = 0;
        }
        
        public VisibilityNode(int teamInfluence, int valueOfVisibility)
        {
            _teamInfluence = teamInfluence;
            _valueOfVisibility = valueOfVisibility;
        }
        
        public float GetValueVisibility()
        {
            return _valueOfVisibility;
        }
        
        public void DecreaseVisibility(float decreaseValue)
        {
            if (_valueOfVisibility - decreaseValue < 0)
                _valueOfVisibility = 0;
            else
            {
                _valueOfVisibility = _valueOfVisibility - decreaseValue;
            }
        }

        public void SetNewInfluence(int newValueOfInfluence)
        {
            _valueOfVisibility = newValueOfInfluence;
        }
        public void DrawVisibilityNodeRed(int i, int j)
        {
            Color c;
            if (_valueOfVisibility > GlobalAttributes.BAREM_HAS_VISIBILITY)
                c = Color.red;
            else
                c = Color.white;
            
            Vector3 originalPos =
                GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i, j].GetCenter();
            Vector3 whereToDraw = new Vector3(originalPos.x + GlobalAttributes.POS_DRAW_VISIBILITY_MAP_RED.x, GlobalAttributes.POS_DRAW_VISIBILITY_MAP_RED.y, originalPos.z + GlobalAttributes.POS_DRAW_VISIBILITY_MAP_RED.z);
            GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i,j].DrawCellColored(whereToDraw,c);
        }
        
        public void DrawVisibilityNodeBlue(int i, int j)
        {
            Color c;
            if (_valueOfVisibility > GlobalAttributes.BAREM_HAS_VISIBILITY)
                c = Color.blue;
            else
                c = Color.white;
            
            Vector3 originalPos =
                GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i, j].GetCenter();
            Vector3 whereToDraw = new Vector3(originalPos.x + GlobalAttributes.POS_DRAW_VISIBILITY_MAP_BLUE.x, GlobalAttributes.POS_DRAW_VISIBILITY_MAP_BLUE.y, originalPos.z + GlobalAttributes.POS_DRAW_VISIBILITY_MAP_BLUE.z);
            GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i,j].DrawCellColored(whereToDraw,c);
        }
        
    }
