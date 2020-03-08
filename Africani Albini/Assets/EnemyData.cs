using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//MAIN "AI" CLASS OF THE ENEMY, and referencer for the other ENEMY scripts
public class EnemyData : MonoBehaviour
{
    private GameObject player;
    private EnemyMove move;
    private EnemySight sight;
    public GameObject patrolWayPoints;
    public enum EnemyStateEnum { idle, patrol, search, chase}; //queste cose dovrebbero esser definite altrove dico bene?
    public EnemyStateEnum enemyState;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        move = GetComponent<EnemyMove>();
        sight = GetComponent<EnemySight>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2)) { enemyState = EnemyStateEnum.idle; print("State idle"); } // idle
        if (Input.GetKeyDown(KeyCode.F3)) { enemyState = EnemyStateEnum.patrol; print("State patrol"); } // patrol
        if (Input.GetKeyDown(KeyCode.F4)) { enemyState = EnemyStateEnum.chase; print("State follow"); } // inseguimento
        if (Input.GetKeyDown(KeyCode.F5)) { enemyState = EnemyStateEnum.search; print("State search"); } // ricerca
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

}
