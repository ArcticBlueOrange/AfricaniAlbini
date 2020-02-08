using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCharacter : MonoBehaviour
{
    public InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InventoryObject>() && Input.GetKeyDown(KeyCode.I))
        {
            InventoryObject obj = other.GetComponent<InventoryObject>();
            print("Picked " + obj.name);
        }
    }

}
