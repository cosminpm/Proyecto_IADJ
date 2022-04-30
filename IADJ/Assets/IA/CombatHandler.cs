using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

// Manejador de combate
public static class CombatHandler
{

    public enum TypeUnits{
        Soldier = 0,
        Archer = 1,
        Tank = 2,
    }

     // Diccionaro con los costes de daño a cada tipo de unidad
    private static Dictionary<TypeUnits, Dictionary<TypeUnits,float>> damageTable;


    static CombatHandler(){
        CreateTableDamage();
    }

   
   
   // Calcula el daño infligido del atacante al defensor
   public static float CalculateDamage (NPC attacker, NPC defender){

        Dictionary<TypeUnits, float> dmgDictionary =  damageTable[(TypeUnits)((int)attacker.Unit.TypeUnit)];

        float damageIndex = dmgDictionary[(TypeUnits)((int)defender.Unit.TypeUnit)];

        return attacker.Unit.AttackPoints*damageIndex;

   }

    // Creo la tabla de tipos, se puede hacer que se lea desde un fichero easy
    private static void CreateTableDamage(){

        Dictionary<TypeUnits, float> dmgSoldier = new Dictionary<TypeUnits, float>();
        dmgSoldier.Add(TypeUnits.Soldier, 1f);
        dmgSoldier.Add(TypeUnits.Archer, 1.75f);
        dmgSoldier.Add(TypeUnits.Tank, 0.5f);

        Dictionary<TypeUnits, float> dmgArcher = new Dictionary<TypeUnits, float>();
        dmgArcher.Add(TypeUnits.Soldier, 1.5f);
        dmgArcher.Add(TypeUnits.Archer, 1f);
        dmgArcher.Add(TypeUnits.Tank, 1f);


        Dictionary<TypeUnits, float> dmgTank = new Dictionary<TypeUnits, float>();
        dmgTank.Add(TypeUnits.Soldier, 1.5f);
        dmgTank.Add(TypeUnits.Archer, 1.75f);
        dmgTank.Add(TypeUnits.Tank, 1f);

        damageTable = new Dictionary<TypeUnits, Dictionary<TypeUnits,float>>();
        damageTable.Add(TypeUnits.Soldier, dmgSoldier);
        damageTable.Add(TypeUnits.Archer, dmgArcher);
        damageTable.Add(TypeUnits.Tank, dmgTank);

    }

    private static void MostrarTabla(){

        Debug.Log(damageTable[TypeUnits.Soldier]);
    }
 



}

   