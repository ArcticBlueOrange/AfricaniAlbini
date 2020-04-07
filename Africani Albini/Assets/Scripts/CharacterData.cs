using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public InventoryManager inventory;
    public CharController_Motor control;
    public CharController_Look look;


    public bool playingMinigame = false; //TODO expand this and inglobare the whole minigame stuff in the player


    void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        control = GetComponent<CharController_Motor>();
        look = GetComponent<CharController_Look>();
    }

}
