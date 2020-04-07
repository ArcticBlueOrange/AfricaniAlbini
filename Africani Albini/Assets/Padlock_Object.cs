﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock_Object : InventoryObject
{ //TODO create a new class MiniGameObject and derive from there instead
    public int[] state;
    public GameObject[] gears; //gears in the GUI model
    public GameObject verr; //piece to open
    public GameObject pivot; //rotation of gears
    public int[] correctCombination;
    public float rotationSpeed = 500f;
    public bool[] triggers;
    public Transform minigameCanvas;
    CharacterData playerData;
    void Start()
    {
        pickable = false;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerData = playerObject.GetComponent<CharacterData>();
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
        rb = GetComponent<Rigidbody>();
        state = new int[3] { 0, 0, 0 };
        correctCombination = new int[3] { 6, 6, 6 };
        minigameCanvas = transform.Find("MiniGameCanvas");//GetComponent<Canvas>();
        triggers = new bool[3] { false, false, false};
        gears = new GameObject[3];
        gears[0] = minigameCanvas.Find("Minigame_Padlock").transform.Find("Tambour 0").gameObject;
        gears[1] = minigameCanvas.Find("Minigame_Padlock").transform.Find("Tambour 1").gameObject;
        gears[2] = minigameCanvas.Find("Minigame_Padlock").transform.Find("Tambour 2").gameObject;
        verr = minigameCanvas.Find("Minigame_Padlock").transform.Find("Verrou").gameObject;
        pivot = minigameCanvas.Find("Minigame_Padlock").transform.Find("Pivot").gameObject;
        minigameCanvas.gameObject.SetActive(false);
        triggers = new bool[3] { false, false, false }; //use for debugging purpose
        //StartCoroutine(rotateGear(0));
        
    }

    void Update()
    {
        
        //if (triggers[0] && playerData.playingMinigame)
        //{
        //    triggers[0] = false;
        //    StartCoroutine(rotateGear(0));
        //}
        //if (triggers[1] && playerData.playingMinigame)
        //{
        //    triggers[1] = false;
        //    StartCoroutine(rotateGear(1));
        //}
        //if (triggers[2] && playerData.playingMinigame)
        //{
        //    triggers[2] = false;
        //    StartCoroutine(rotateGear(2));
        //}

        if (playerData.playingMinigame)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Generate raycast from mouse to canvas
                Ray ray = playerData.control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                int layer = 5;
                if (Physics.Raycast(ray, out RaycastHit hit, layer))
                {
                    // check if hits a gear
                    //print(hit);
                    //print(hit.transform);
                    //print(hit.transform.gameObject);
                    //print(hit.transform.gameObject.name);
                    //print(hit.transform.gameObject.name == "Tambour 0");
                    //print(hit.transform.gameObject.name == "Tambour 1");
                    //print(hit.transform.gameObject.name == "Tambour 2");
                    //print(hit.transform.gameObject.name == "Verrou");
                    // print(hit);
                    if (hit.transform.gameObject.name == "Tambour 0") { StartCoroutine(rotateGear(0)); }
                    if (hit.transform.gameObject.name == "Tambour 1") { StartCoroutine(rotateGear(1)); }
                    if (hit.transform.gameObject.name == "Tambour 2") { StartCoroutine(rotateGear(2)); }
                    if (hit.transform.gameObject.name == "Verrou") { StartCoroutine(tryToOpen()); }
                    // if so rotate it
                }
            }
        }
    }

    IEnumerator rotateGear(int gear)
    {
        if (gear < 0 || gear > 2) throw new MissingComponentException("wrong gear");
        // rotate gear
        //float objectiveRotation = (gears[gear].transform.localRotation.eulerAngles.y - 36) % 360;
        //float objectiveRotation = (gears[gear].transform.localRotation.eulerAngles.y + 360 - 36);// % 360;
        float objectiveRotation = 36,//= (gears[gear].transform.localRotation.eulerAngles.y + 360 - 36);// % 360;
              rot = 0;
        while (rot < objectiveRotation)
        {
            gears[gear].transform.RotateAround(pivot.transform.position, pivot.transform.forward, rotationSpeed * Time.deltaTime);
            rot += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        state[gear] = (state[gear] + 1) % 10;
        updateUICode();
        //print(state[gear]);
    }

    IEnumerator tryToOpen()
    {
        // animation for trying to open
        float openspeed = .05f;
        float origY = verr.transform.localPosition.y;
        if (checkCombination())
        {
            print("Correct!");
            while (verr.transform.localPosition.y <= origY + .5)
            {//made animation 1: open...
                verr.transform.Translate(0, openspeed * 3 * Time.deltaTime, 0);
                yield return null;
            }
            //made animation 2: rotate round pivot...
            float objectiveRotation = 180,
                  rot = 0;
            while (rot < objectiveRotation)
            {
                verr.transform.RotateAround(pivot.transform.position, pivot.transform.forward, rotationSpeed * Time.deltaTime);
                rot += rotationSpeed * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            //destroy object
            Destroy(gameObject);
        } else
        {
            print("Fail!");
            while (verr.transform.localPosition.y <= origY+.2)
            {//failed animation
                verr.transform.Translate(0, openspeed * Time.deltaTime, 0);
                yield return null;
            } yield return new WaitForSeconds(0.2f);
            while (verr.transform.localPosition.y >= origY)
            {//back to normal
                verr.transform.Translate(0, -openspeed * Time.deltaTime, 0);
                yield return null;
            }
        }
        print(verr.transform.position);
        yield return null;
    }

    void changeState()
    {
        playerData.playingMinigame = !playerData.playingMinigame;
        CharController_Look look = playerObject.GetComponent<CharacterData>().look;
        if (playerData.playingMinigame)
        {
            minigameCanvas.gameObject.SetActive(true);
            look.LockMouse(true);
        } else
        {
            minigameCanvas.gameObject.SetActive(false);
            look.LockMouse(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && playerData.playingMinigame)
            changeState();
    }

    bool checkCombination()
    {
        for (int i = 0; i < state.Length; i++)
        {
            if (state[i] != correctCombination[i]) return false;
        }
        print("Combination correct");
        return true;
    }

    void updateUICode()
    {
        Transform UICODE = minigameCanvas.Find("UI_Code");
        TMPro.TextMeshProUGUI text = UICODE.GetComponent<TMPro.TextMeshProUGUI>();
        string newtext = string.Format("{0}:{1}:{2}", state[0], state[1], state[2]);
        //print(newtext);
        //print(UICODE);
        //print(text);
        text.text = newtext; 
    }

    public override void useObject()
    {
        changeState();
        //// point raycast from camera of player
        //Ray ray = playerObject.GetComponent<CharacterData>().control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        //int layermask = (1 << 11);//~
        //// if (Physics.Raycast(ray, out RaycastHit hit, layermask)) //not working uh?
        //RaycastHit[] hits = Physics.RaycastAll(ray, 10f);//, layermask);
        //foreach(RaycastHit hit in hits)
        //{// check if ray hits any of the three gears
        //    // tried changing layer where gears are set, didn't work
        //    print("Hit " + hit.collider.transform.name);
        //    // looks inefficient. comunque non funziona
        //    for (int i = 0; i < gears.Length; i++)
        //    {
        //        GameObject gear = gears[i];
        //        if (hit.collider.gameObject == gear.gameObject)
        //        {
        //            //print("Hit " + i);
        //            StartCoroutine(rotateGear(i));
        //        }
        //    }
        //    // if so, rotate it
        //    // otherwise, do'na sega
        //    /// ^ questo già lo fa
        //}

        // // perform animation
        // print("TODO Animazione Badile");
        // // chck if object "zolladiterra" in front" e vicìn
        // Ray ray = playerObject.GetComponent<CharacterData>().control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out RaycastHit hit))
        // {
        //     print(hit.transform.gameObject.name + ", " + hit.distance);
        //     if (hit.transform.gameObject.GetComponent<InventoryObject>() && hit.distance <= 10)
        //     {
        //         InventoryObject io = hit.transform.gameObject.GetComponent<InventoryObject>();
        //         if (io.objectName == "Zolla di Terra")
        //         {
        //             print("Scavando...");
        //             io.useObject();
        //             StartCoroutine(DestroyAfterEndZolla(hit.transform.gameObject));
        //         }
        //     }
        // }
        // //  if so, destroy both zolla and pala
        // //  otherwise, do 'na sega
    }




}
