using Global;
using Grid;
using UnityEditor;
using UnityEngine;

namespace InfluenceMap
{
    public class InfluenceNode
    {
        private int _teamInfluence;
        private float _valueOfInfluence;
            
            
        public InfluenceNode()
        {
            _teamInfluence = GlobalAttributes.CELDA_INFLUENCIA_NADIE;
            _valueOfInfluence = 0;
        }
        
        public InfluenceNode(int teamInfluence, float valueOfInfluence)
        {
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

        
        public void SetTeamInfluence(int teamInfluence)
        {
            _teamInfluence = teamInfluence;
        }

        public void SetValueOfInfluence(float valueOfInfluence)
        {
            _valueOfInfluence = valueOfInfluence;
            if (_valueOfInfluence < GlobalAttributes.MINIMUM_VALUE_INFLUENCE)
            {
                _teamInfluence = GlobalAttributes.CELDA_INFLUENCIA_NADIE;
            }
            
        }

        public float GetValueInfluence()
        {
            return _valueOfInfluence;
        }


        public void DrawInfluenceNode(int i, int j)
        {
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
                c = Color.white;
            }


            Vector3 originalPos =
                GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i, j].GetCenter();
            Vector3 whereToDraw = new Vector3(originalPos.x + GlobalAttributes.POS_DRAW_INFLUENCE_MAP.x, GlobalAttributes.POS_DRAW_INFLUENCE_MAP.y, originalPos.z + GlobalAttributes.POS_DRAW_INFLUENCE_MAP.z);
            GameObject.Find(GlobalAttributes.NAME_GRID_CONTROLLER).GetComponent<GridMap>().GetCellMap()[i,j].DrawCellColored(whereToDraw,c);
            
        }
        
    }
}