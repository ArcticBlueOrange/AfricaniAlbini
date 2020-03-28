using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    //public enum EnemyStateEnum { idle, patrol, search, chase }; //old
    public enum EnemyStateEnum { idle, pathPatrol, idleAlert, pathSearch, pathInspect, chasePlayer, attack }; //temporary
    public EnemyStateEnum activeState; //points to the current state of the function
    [SerializeField] bool stateChanged;

    ///                                                           Detail of States
    // 1 Normal     passa ad Alert (3) se uno dei sensi (vista-udito) viene attivato
    //// | 1 | Idle
    //// | 2 | Path to WayPoint (walking)
    
    // 2 Alert      passa a Chase  (6) se il player rimane nel campo visivo troppo a lungo (1 secondo)
    //              passa a Normal (1) allo scadere di un timer di ricerca
    //// | 3 | Idle-Alert
    //// | 4 | Path to WayPoint (walking, scouting)
    //// | 5 | Path to Player Pos (walkking, searching)
    
    // 3 Chase      passa a Alert  (4) se il giocatore rimane fuori dal campo visivo troppo a lungo
    //// | 6 | Path to Player Pos (running, chase)
    //// | 7 | Attack Player
    
    /// others maybe in the future...
    // Given few states the distinction in macro-states can be absent (only theoretical), with more it may be necessary to have states AND sub-states.
    // For now try with 7 distinct states and their transictions.

    EnemyData data;
    void Start()
    {
        data = GetComponent<EnemyData>();
    }

    void Update()
    {
        //if (activeState != null)
        //{
        //    StartCoroutine(activeState);
        //}
        if (stateChanged)
        {
            switch (activeState)
            {
                case EnemyStateEnum.idle:
                    StartCoroutine(data.Idle());
                    break;
                case EnemyStateEnum.pathPatrol:
                    StartCoroutine(data.PathPatrol());
                    break;
                case EnemyStateEnum.idleAlert:
                    StartCoroutine(data.IdleAlert());
                    break;
                case EnemyStateEnum.pathSearch:
                    StartCoroutine(data.PathSearch());
                    break;
                case EnemyStateEnum.pathInspect:
                    StartCoroutine(data.PathInspect());
                    break;
                case EnemyStateEnum.chasePlayer:
                    StartCoroutine(data.ChasePlayer());
                    break;
                case EnemyStateEnum.attack:
                    StartCoroutine(data.Attack());
                    break;
            }
            stateChanged = false;
        }
    }

    public void setState(EnemyStateEnum state)
    {
        stateChanged = true;
        activeState = state;
    }



}
