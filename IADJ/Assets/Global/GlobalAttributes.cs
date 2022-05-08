using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Global
{
    public static class GlobalAttributes
    {
        public static String NAME_GRID_CONTROLLER = "GridController";
        public static String NAME_AGENTE_INVISIBLE = "AgenteInvisible";
        public static String NAME_MINICAMERA = "MinimapCamera";
        public static String NAME_BOLA_AMARILLA = "BolaAmarilla";
        public static String NAME_MAP_PARENT = "Mapa";
        
        public static String TAG_EQUIPO_AZUL = "EquipoAzul";
        public static String TAG_EQUIPO_ROJO = "EquipoRojo";

        public static String TAG_BASE_ROJA = "BaseRoja";
        public static String TAG_BASE_AZUL = "BaseAzul";
        public static String TAG_CAMINO = "Camino";
        public static String TAG_RIO = "Rio";
        public static String TAG_PRADERA = "Pradera";
        public static String TAG_BOSQUE = "Bosque";
        public static String TAG_ACANTILADO = "Acantilado";
        
        
        public static int CELDA_INFLUENCIA_NADIE = 0;
        public static int CELDA_INFLUENCIA_AZUL = 1;
        public static int CELDA_INFLUENCIA_ROJO = 2;
        
        public static float MINIMUM_VALUE_INFLUENCE = 0.1f;
        public static float MAXIMUM_VALUE_INFLUENCE = 20f;
        public static float CONSTANT_DECRASING_VALUE = 0.5f;

        public static Vector3 POS_DRAW_INFLUENCE_MAP = new Vector3(0,-1,-25);
        
        public enum Team {
            Red = 0,
            Blue = 1    
        }
        

        public static bool CheckIfItIsFloor(String tagFloor)
        {
            if (tagFloor == TAG_BASE_ROJA ||
                tagFloor == TAG_BASE_AZUL ||
                tagFloor == TAG_CAMINO ||
                tagFloor == TAG_CAMINO ||
                tagFloor == TAG_RIO ||
                tagFloor == TAG_PRADERA ||
                tagFloor == TAG_BOSQUE ||
                tagFloor == TAG_ACANTILADO)
            {
                return true; 
            }
            return false;
        }
        
    }
}