using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

// En principio cada tipo de personaje podrá definir su propio GUI...

public class GUIManagerGlobal : MonoBehaviour
{
    public UIBar barraRoja;
    public UIBar barraAzul;
    private GameHandler gameManager;


    void Awake(){
        gameManager = GetComponent<GameHandler>();
    }

    void Update(){
        barraRoja.UpdateBar(gameManager.waypointManager.blueEnemyBase.winningPercentage);
        barraAzul.UpdateBar(gameManager.waypointManager.redEnemyBase.winningPercentage);
    }

    public void Initialize(){
        barraRoja.SetMaxValue(100);
        barraAzul.SetMaxValue(100);
    }

}

   