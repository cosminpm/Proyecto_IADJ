using Global;
using Grid;
using UnityEditor;
using UnityEngine;

namespace InfluenceMap
{
    public class InfluenceNode
    {
        private bool _isUnitOnNode;
        private int _teamInfluence;
        private float _valueOfInfluence;
            
            
        public InfluenceNode()
        {
            _isUnitOnNode = false;
            _teamInfluence = GlobalAttributes.CELDA_INFLUENCIA_NADIE;
            _valueOfInfluence = 0;
        }
        
        public InfluenceNode(bool isUnitOnNode, int teamInfluence, float valueOfInfluence)
        {
            _isUnitOnNode = isUnitOnNode;
            _teamInfluence = teamInfluence;
            _valueOfInfluence = valueOfInfluence;
        }

        
        
        
        public void SetNewInfluence(int newTeamInfluence, float newValueOfInfluence)
        {
            if (newTeamInfluence == _teamInfluence)
            {
                if (_valueOfInfluence + newValueOfInfluence >= GlobalAttributes.MAXIMUM_VALUE_INFLUENCE)
                    _valueOfInfluence = GlobalAttributes.MAXIMUM_VALUE_INFLUENCE;
                else
                    _valueOfInfluence += newValueOfInfluence;
            }

            else
            {
                if (newValueOfInfluence > _valueOfInfluence && (newValueOfInfluence - _valueOfInfluence) >
                    GlobalAttributes.MINIMUM_VALUE_INFLUENCE)
                {
                    _valueOfInfluence = newValueOfInfluence - _valueOfInfluence;
                    _teamInfluence = newTeamInfluence;
                }
                else if(_valueOfInfluence > newValueOfInfluence  && (_valueOfInfluence - newValueOfInfluence) >
                    GlobalAttributes.MINIMUM_VALUE_INFLUENCE)

                {
                    _valueOfInfluence = _valueOfInfluence - newValueOfInfluence;
                }

                else
                {
                    _valueOfInfluence = 0f;
                    _teamInfluence = GlobalAttributes.CELDA_INFLUENCIA_NADIE;
                }
            }
        }

        public void SetUnitOnNode(bool isUnitOnNode)
        {
            _isUnitOnNode = isUnitOnNode;
        }
        
        public void SetTeamInfluence(int teamInfluence)
        {
            _teamInfluence = teamInfluence;
        }

        public void SetValueOfInfluence(float valueOfInfluence)
        {
            _valueOfInfluence = valueOfInfluence;
        }

        public float GetValueInfluence()
        {
            return _valueOfInfluence;
        }


        public void DrawInfluenceNode(int i, int j)
        {
            GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i,j].DrawCell();

            Color c;

            if (_teamInfluence == GlobalAttributes.CELDA_INFLUENCIA_ROJO)
            {
                c = Color.Lerp(Color.white, Color.red, _valueOfInfluence / GlobalAttributes.MAXIMUM_VALUE_INFLUENCE);
            } 
            else if (_teamInfluence == GlobalAttributes.CELDA_INFLUENCIA_AZUL)
            {
                c = Color.Lerp(Color.white, Color.blue, _valueOfInfluence / GlobalAttributes.MAXIMUM_VALUE_INFLUENCE);
            }
            else
            {
                c = Color.gray;
            }

            GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i,j].DrawCellColored(c);
            
        }
        
    }
}