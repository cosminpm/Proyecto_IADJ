using System;

namespace Global
{
    public static class GlobalAttributes
    {
        public static String NAME_GRID_CONTROLLER = "GridController";
        
        public static String TAG_EQUIPO_AZUL = "EquipoAzul";
        public static String TAG_EQUIPO_ROJO = "EquipoRojo";

        public static int CELDA_INFLUENCIA_NADIE = 0;
        public static int CELDA_INFLUENCIA_AZUL = 1;
        public static int CELDA_INFLUENCIA_ROJO = 2;

        public static float MINIMUM_VALUE_INFLUENCE = 0.1f;
        public static float MAXIMUM_VALUE_INFLUENCE = 10f;
    }
}