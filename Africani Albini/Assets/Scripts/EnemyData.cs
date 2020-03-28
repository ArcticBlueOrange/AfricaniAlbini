using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAIN "AI" CLASS OF THE ENEMY, and referencer for the other ENEMY scripts
public class EnemyData : MonoBehaviour
{
    private GameObject player;
    private EnemyMove move;
    private EnemySight sight;
    private EnemyFSM fsm;
    public GameObject patrolWayPoints;
    //[SerializeField] Transform[] wayPoints;

    [SerializeField] float waitTime = 2;
    //[SerializeField] int currentTarget = 0; //waypoint target
    private Transform TargetPosition { get; }
    //public EnemyFSM.EnemyStateEnum enemyState;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        move = GetComponent<EnemyMove>();
        sight = GetComponent<EnemySight>();
        fsm = GetComponent<EnemyFSM>();

        fsm.setState(EnemyFSM.EnemyStateEnum.pathPatrol);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F2)) { enemyState = EnemyFSM.EnemyStateEnum.idle; print("State idle"); } // idle
        //if (Input.GetKeyDown(KeyCode.F3)) { enemyState = EnemyFSM.EnemyStateEnum.patrol; print("State patrol"); } // patrol
        //if (Input.GetKeyDown(KeyCode.F4)) { enemyState = EnemyFSM.EnemyStateEnum.chase; print("State follow"); } // inseguimento
        //if (Input.GetKeyDown(KeyCode.F5)) { enemyState = EnemyFSM.EnemyStateEnum.search; print("State search"); } // ricerca
        if (Input.GetKeyDown(KeyCode.F6)) { scoutWayPoints(player.transform.position, 10, 8); } // prepare for search 
    }
    void scoutWayPoints(Vector3 coords, float circleSize, int wayPoints)
    { // cerchio di ricerca per il giocatore
        // define a circle at the V3 coordinates, of size cS
        //// means define an empty GO
        GameObject circle = new GameObject("scouting circle");
        circle.transform.position = coords;
        // define wP points aroundthe circle
        // define angles on which to put the points
        //float radAngle = 2 * ((float) System.Math.PI) / wayPoints;
        float eulAngle = 360 / wayPoints;
        float curAngle = 0f;
        // start creating n points
        for (int i = 1; i <= wayPoints; i++)
        {
            GameObject wp = new GameObject("waypoint " + i); // ilovememucho
            Vector3 offset = Vector3.forward * circleSize;
            //Quaternion rotation = Quaternion.Euler(0, curAngle, 0);
            //Quaternion rotation = Quaternion.Euler(0, curAngle, 0) * Quaternion.AngleAxis(curAngle, Vector3.up);
            Quaternion rotation = Quaternion.AngleAxis(curAngle, Vector3.up);
            wp.transform.position = circle.transform.position + (rotation * offset); //NON HO LA MINIMA IDEA DI COME FUNZIONI QUESTO PORCODDIO
            // make child
            wp.transform.parent = circle.transform;
            curAngle += eulAngle;
            //wp.transform.position = circle.transform.position
        }
        // for each point, if it's in a valid navmesh, activate it
        ///// TODO MANCANTE (E IMPORTANTE)
        // Remove old navmesh ( or store it separately)
        ///// TODO MANCANTE (E IMPORTANTE)
        // at the end, make this the new navmesh
        patrolWayPoints = circle;
        move.updateWayPoints();
    }

    //TODODEFINE STATES AS IENUMERATORS (SEE ENEMYMOVE)
    //example from enemymove: (NOTE: ALL THE IENUMS FROM OTHER SCRIPTS MUST BE REMOVED AND PUT HERE, OR IN A AD HOC SCRIPT)
    public IEnumerator Idle()
    {
        print("Entering Idle state");
        move.idle(); //manage stop and animation frim there
        yield return new WaitForSeconds(waitTime);
        print("Exiting idle state for patrol state");
        fsm.setState(EnemyFSM.EnemyStateEnum.pathPatrol);
    }
    public IEnumerator PathPatrol()
    {
        // calculate target waypoint (next patrol WP)
        move.currentTarget = (move.currentTarget + 1) % move.wayPoints.Length;
        print("Current target is " + move.currentTarget);
        Vector3 target = move.wayPoints[move.currentTarget].position;
        //launch script from "move" that will only make the character go there
        // this also sets animation and destination
        move.moveToTarget(target);
        //prepare loop of distance checking
        Vector3 horizontalOthr = new Vector3(target.x, 0, target.z);
        Vector3 horizontalSelf = new Vector3(transform.position.x, 0, transform.position.z);
        float dist = Vector3.Distance(horizontalOthr, horizontalSelf);
        while ( dist >= 4 )
        {// while desitnation is not reached, and no sensory information was gotten: continue. Else decide the new state
            horizontalOthr = new Vector3(target.x, 0, target.z);
            horizontalSelf = new Vector3(transform.position.x, 0, transform.position.z);
            // if something else happens, break loop and change state
            dist = Vector3.Distance(horizontalOthr, horizontalSelf);
            // print("New distance from "+ move.currentTarget + " ("+ target + ") and enemy ("+transform.position+") is " + dist);
            yield return null;
        }
        if (Vector3.Distance(horizontalOthr, horizontalSelf) < 4)
        {
            fsm.setState(EnemyFSM.EnemyStateEnum.idle);
        }
        print("Exiting patrol state for idle state");
        yield return null;
        // throw new NotImplementedException();
    }

    public IEnumerator IdleAlert()
    {
        // stop moving (just in case)
        // turn towards player (or player sensory info)
        // if player still there after (big time) seconds -> state to ChasePlayer
        // otherwise                                      -> state to PathInspect
        throw new NotImplementedException();
    }

    public IEnumerator PathSearch() //cerca anello intorno al giocatore (TODO la funzione per definire l'anello esiste, ma può generare waypoint non validi)
    {
        // sarch around player (like PathPatrol, but with no idle)
        // after getting to a WP, immediately to the next
        // if after some time the player is not found, back to Idle
        throw new NotImplementedException();
    }

    public IEnumerator PathInspect() //cerca diritto verso il giocatore
    {
        // va (camminando) verso l'ultima posizione registrata del giocatore (il codice è presente in EnemySight, al momento solo per la vista (plan per l'udito è nella mia testa e basta)
        // se il giocatore non viene individuato, va a PathSearch (cerca area intorno)
        // se il giocatore viene individuato, va a ChasePlayer
        throw new NotImplementedException();
    }
    public IEnumerator ChasePlayer() //corre verso il giocatore
    {
        // va (correndo) verso il giocatore
        // se lo raggiunge,         va a PathSearch
        // se lo perde di vista,    va a PathSearch
        throw new NotImplementedException();
    }

    public IEnumerator Attack() //attacca il giocatore
    {
        // si ferma e attacca (non abbiamo l'animazione quindi ci arrangiamo)
        // se il player è lontano ritorna su ChasePlayer
        throw new NotImplementedException();
    }



}
