using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debug_Gizmos : MonoBehaviour
{
    //public Transform[] waypoints;
    //private void Start()
    //{
    //    waypoints = GetComponentsInChildren<Transform>();
    //}
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2);
    }
}
