using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;

// Manejador de combate
public static class CombatHandler
{

    // Definimos los tipos de unidad
    public enum TypeUnits{
        Soldier = 0,
        Archer = 1,
        Tank = 2,
        Healer = 3
    }
    
     // Diccionaro con los costes de daño a cada tipo de unidad
    private static Dictionary<TypeUnits, Dictionary<TypeUnits,float>> damageTable;

    // Creamos la "tabla de tipos"
    static CombatHandler(){
        CreateTableDamage();
    }

   // Calcula el daño infligido del atacante al defensor. 
   public static float CalculateDamage (NPC attacker, NPC defender){

        float miss = Random.Range(0f, 1f);
        float critic = Random.Range(0f, 1f);

        // Comprobamos si el atacante ha acertado el golpe. 
        if (miss <= attacker.Unit.AttackAccuracy){

            // Si ha acertado, calculamos el daño causado.
            Dictionary<TypeUnits, float> dmgDictionary = damageTable[(TypeUnits)((int)attacker.Unit.TypeUnit)];
            float damageIndex = dmgDictionary[(TypeUnits)((int)defender.Unit.TypeUnit)];

            // Comprobamos si el golpe es crítico. En ese caso, se hace un 50%
            // más de daño.
            if (critic <= attacker.Unit.CriticRate)
            {
                return attacker.Unit.AttackPoints * damageIndex * 1.5f;
            }

            return attacker.Unit.AttackPoints * damageIndex;
        }
        // Si ha fallado, no quita vida. 
        return 0f;
    }

    // Creamos la tabla de tipos (TODO_Opcional: se puede hacer que se lea desde un fichero easy)
    private static void CreateTableDamage(){

        Dictionary<TypeUnits, float> dmgSoldier = new Dictionary<TypeUnits, float>();
        dmgSoldier.Add(TypeUnits.Soldier, 1f);
        dmgSoldier.Add(TypeUnits.Archer, 1.75f);
        dmgSoldier.Add(TypeUnits.Tank, 0.5f);
        dmgSoldier.Add(TypeUnits.Healer, 1f);

        Dictionary<TypeUnits, float> dmgArcher = new Dictionary<TypeUnits, float>();
        dmgArcher.Add(TypeUnits.Soldier, 1.5f);
        dmgArcher.Add(TypeUnits.Archer, 1f);
        dmgArcher.Add(TypeUnits.Tank, 1f);
        dmgArcher.Add(TypeUnits.Healer, 1.5f);

        Dictionary<TypeUnits, float> dmgTank = new Dictionary<TypeUnits, float>();
        dmgTank.Add(TypeUnits.Soldier, 1.5f);
        dmgTank.Add(TypeUnits.Archer, 1.75f);
        dmgTank.Add(TypeUnits.Tank, 1f);
        dmgTank.Add(TypeUnits.Healer, 1.5f);


        damageTable = new Dictionary<TypeUnits, Dictionary<TypeUnits,float>>();
        damageTable.Add(TypeUnits.Soldier, dmgSoldier);
        damageTable.Add(TypeUnits.Archer, dmgArcher);
        damageTable.Add(TypeUnits.Tank, dmgTank);

    }

    private static void MostrarTabla(){

        Debug.Log(damageTable[TypeUnits.Soldier]);
    }
}

   