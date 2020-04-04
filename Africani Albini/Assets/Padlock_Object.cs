using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padlock_Object : InventoryObject
{
    public int[] state;
    public GameObject[] gears;
    public GameObject pivot;
    public int[] correctCombination;
    public float rotationSpeed = 10;
    public bool[] triggers;
    void Start()
    {
        pickable = false;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(playerObject.GetComponent<Collider>(), GetComponent<Collider>());
        rb = GetComponent<Rigidbody>();
        state = new int[3] { 0, 0, 0 };
        correctCombination = new int[3] { 6, 6, 6 };
        gears = new GameObject[3];
        gears[0] = transform.Find("Mesh").transform.Find("Tambour 0").gameObject;
        gears[1] = transform.Find("Mesh").transform.Find("Tambour 1").gameObject;
        gears[2] = transform.Find("Mesh").transform.Find("Tambour 2").gameObject;
        pivot = transform.Find("Mesh").transform.Find("Pivot").gameObject;
        triggers = new bool[3] { false, false, false };
        //StartCoroutine(rotateGear(0));
    }

    void Update()
    {
        if (triggers[0])
        {
            triggers[0] = false;
            StartCoroutine(rotateGear(0));
        }
        if (triggers[1])
        {
            triggers[1] = false;
            StartCoroutine(rotateGear(1));
        }
        if (triggers[2])
        {
            triggers[2] = false;
            StartCoroutine(rotateGear(2));
        }
    }

    IEnumerator rotateGear(int gear)
    {
        if (gear < 0 || gear > 2) throw new MissingComponentException("wrong gear");
        state[gear] = (state[gear] + 1) % 10;
        // rotate gear
        //float objectiveRotation = (gears[gear].transform.localRotation.eulerAngles.y - 36) % 360;
        //float objectiveRotation = (gears[gear].transform.localRotation.eulerAngles.y + 360 - 36);// % 360;
        float objectiveRotation = 36,//= (gears[gear].transform.localRotation.eulerAngles.y + 360 - 36);// % 360;
              rot = 0;
        while (rot < objectiveRotation)
        {
            gears[gear].transform.RotateAround(pivot.transform.position, Vector3.down, rotationSpeed * Time.deltaTime);
            rot += rotationSpeed * Time.deltaTime;
            yield return null;
        }
        //print(state[gear]);
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

    public override void useObject()
    {
        // point raycast from camera of player
        Ray ray = playerObject.GetComponent<CharacterData>().control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        int layermask = (1 << 11);//~
        // if (Physics.Raycast(ray, out RaycastHit hit, layermask)) //not working uh?
        RaycastHit[] hits = Physics.RaycastAll(ray, 10f);//, layermask);
        foreach(RaycastHit hit in hits)
        {// check if ray hits any of the three gears
            // tried changing layer where gears are set, didn't work
            print("Hit " + hit.collider.transform.name);
            // looks inefficient. comunque non funziona
            for (int i = 0; i < gears.Length; i++)
            {
                GameObject gear = gears[i];
                if (hit.collider.gameObject == gear.gameObject)
                {
                    //print("Hit " + i);
                    StartCoroutine(rotateGear(i));
                }
            }
            // if so, rotate it
            // otherwise, do'na sega
            /// ^ questo già lo fa
        }

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
