using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Grid
{
    
    public class InputToMap:MonoBehaviour
    {
        public String carpetaTexto = "Assets/Grid/Escenarios/";
        public String fichero = "";
        private String path;
        public List<GameObject> listTerrenos;

        private void Start()
        {
            path = carpetaTexto + fichero;
            CreateMap(path);
        }
        
        private void CreateMap(String path)
        {
            StreamReader reader = new StreamReader(path);
            int row = 0;
            GameObject parent = new GameObject();
            parent.name = "Mapa";
            
            while(!reader.EndOfStream)
            {
                string rowString = reader.ReadLine();
                CreateRow(rowString, row, parent);
                row += 1;
            }
        }

        // Create a row
        private void CreateRow(String rowString, int row, GameObject parent)
        {
            int column = 0;
            foreach (var i in rowString)
            {
                column += 1;
                CreateFloor(i, column, row, parent);
            }
        }
        // Create one instantiation of the floor
        private void CreateFloor(Char floorChar, int column, int row, GameObject parent)
        {
            int floorInt = (int) Char.GetNumericValue(floorChar);
            if (floorInt > listTerrenos.Count - 1)
            {
                throw new Exception("Error: el numero pasado: " + floorInt +" en el fichero de texto: " + path +
                                    " es mayor que los terreno permitido.");
            }
            GameObject actualFloor = Instantiate(listTerrenos[floorInt], parent.transform, true);
            actualFloor.transform.position = new Vector3(column * 5,0,row * -5);
            actualFloor.name = actualFloor.name +"_"+ row +"_"+ column;
        }
    }
}