using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public InventoryManager inventory;
    public CharController_Motor control;
    
    void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        control = GetComponent<CharController_Motor>();

    }

}
