using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public bool isActive;
    public GameObject bridge0;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        bridge0 = GameObject.FindGameObjectWithTag("Bridge0");
        print(bridge0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        print("collision");
        if (other.transform.tag == "Player")
        {
            print("Player");
            isActive = !isActive;
            bridge0.GetComponent<Animator>().SetBool("Active", isActive);
        }
    }
}
