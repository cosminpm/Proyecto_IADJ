using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;



public class WallAvoidance : Seek
{
   
    [SerializeField] public float avoidDistance = 8f; 
    [SerializeField] public float lookAhead = 5f;
    [SerializeField] public float lookAheadCentral = 8f;
    [SerializeField] public int numBigotes = 3;

    [Tooltip("Debug")] [SerializeField]  
    protected bool _drawGizmos;
    [SerializeField] private float _anchuraLinea = 1.5f;
    [SerializeField] private float anguloBigotes = 25f;

    [SerializeField] public float AnguloLimiteBigotes = 180f;

    // lista de bigotes
    private List<Vector3> listaBigotes;

    private AgentInvisible auxTarget;

    public void Start(){

        GameObject go = new GameObject("auxWallInvisible");
        auxTarget = go.AddComponent<AgentInvisible>();
        auxTarget.GetComponent<AgentInvisible>().DrawGizmos = true;
        auxTarget.ArrivalRadius = 1f;
        auxTarget.InteriorRadius = 1f;
        listaBigotes = new List<Vector3>();

        group = 0;
    }

    public override Steering GetSteering (Agent agent){
        
        listaBigotes.Clear();
        Steering steer = new Steering();

        // Si no se especifican bigotes..
        if ( numBigotes < 0)
        {
            return steer;
        }

        // Inicializamos los vectores de cada bigote
        InicializarBigotes(agent);


        // Detectamos la colision

        steer = DetectarColision(agent);
        return steer;
    }

    private void InicializarBigotes(Agent agent){

        Vector3 bigote;
        int aux = -1;
        int index = 0;

        if ( numBigotes == 1){
            bigote = agent.Velocity.normalized;
            listaBigotes.Add(bigote);

        } else {
            // Si es impar tiene el bigotecentral
            if ( numBigotes % 2 != 0) {
                bigote = agent.Velocity.normalized;
                listaBigotes.Add(bigote);

                index++;
            }
            int contador = 0;
            for ( int i = index ; i < numBigotes ; i++){
                bigote = Quaternion.AngleAxis(-anguloBigotes*aux, Vector3.up) * agent.Velocity.normalized;
                listaBigotes.Add(bigote);
                contador++;
                aux *= -1;

                if ( contador == 2){
                    aux *= 2;
                    contador = 0;
                }

                if ( Mathf.Abs(anguloBigotes)*Mathf.Abs(aux) >= AnguloLimiteBigotes)
                {
                    numBigotes = listaBigotes.Count;
                    break;
                }
                    
                
            }
        }
    }

    private Steering DetectarColision(Agent agent)
    {
        RaycastHit hit;
        Vector3 positionFinal = Vector3.zero;

        // Siempre daremos preferencia al bigote central.
        if (Physics.Raycast(agent.Position, listaBigotes[0], out hit, lookAheadCentral+lookAhead)) 
        {
            positionFinal =  hit.point + hit.normal * avoidDistance;
            positionFinal.y = 0;

            auxTarget.Position = positionFinal;
            Target = auxTarget;
            return base.GetSteering(agent);

        } else {

            for (int i = 1  ; i < numBigotes ; i++)
            {
                if (Physics.Raycast(agent.Position, listaBigotes[i], out hit, lookAhead))
                {
                    positionFinal = hit.point + hit.normal * avoidDistance;
                    positionFinal.y = 0;

                    auxTarget.Position = positionFinal;
                    Target = auxTarget;
                    return base.GetSteering(agent);
                }
            }
        }
        return new Steering();
    }

    private void GizmosBigotes(int numeroBigotes)
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;
        Vector3 bigote;

        int contador = 0;
        float aux = -1.0f;
        Handles.color = Color.blue;

        for( int i = 0 ; i < numeroBigotes; i++){
            // Si ya hemos pintado dos bigotes, aumentamos el angulo por 2
            if ( contador == 2)
            {
                aux *= 2;
                contador = 0;
            }

            bigote = Quaternion.Euler(0,anguloBigotes*aux,0) * forward;
            Handles.DrawLine(position, position + bigote * lookAhead,_anchuraLinea);
            aux = aux * -1.0f;
            contador++;
        }
    }

    public void OnDrawGizmos()
    {
            if (_drawGizmos) {
                Vector3 position = transform.position;
                Vector3 forward = transform.forward;
                if (numBigotes == 1){
                    // Bigote Central
                    Handles.color = Color.red;
                    Handles.DrawLine(position, position + forward * lookAheadCentral,_anchuraLinea);
                } else {
                    // Si es impar tiene el bigotecentral
                    if ( numBigotes % 2 != 0){
                        Handles.color = Color.red;
                        Handles.DrawLine(position, position + forward * lookAheadCentral,_anchuraLinea);
                        GizmosBigotes(numBigotes-1);
                    } else {
                        GizmosBigotes(numBigotes);
                    }

                }

            }
    }
}
