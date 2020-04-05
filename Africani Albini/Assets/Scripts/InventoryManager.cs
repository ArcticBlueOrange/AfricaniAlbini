using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //TODO move graphic code to Canvas UI Object
    public float size = 100,
                 maxHealth = 100,
                 playerHealth = 100,
                 maxEnergy = 100,
                 playerEnergy = 100;

    public Canvas InventoryMenu;
    public bool InventoryActive = true;
    public List<GameObject> objects;
    public List<GameObject> uiObjects;
    public float menuObjectsOffset = -25;
    public GameObject objectButton;
    public GameObject selectionArea;
    [SerializeField] private int selection = -1;
    CharacterData data;
    //public int Selection { get => selection; set => selection = value % objects.Count; }
    void Start()
    {
        //TODO MOVE COMMANDS RELATED TO INVENTORY MENU TO A SEPARATE SCRIPT
        data = GetComponent<CharacterData>();
        InventoryMenu.transform.Find("_GUI").gameObject.SetActive(true);
        InventoryMenu.transform.Find("_INVENTORY").gameObject.SetActive(false);
        objects = new List<GameObject>();
        uiObjects = new List<GameObject>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryActive == false)
            {
                InventoryMenu.transform.Find("_GUI").gameObject.SetActive(false);
                InventoryMenu.transform.Find("_INVENTORY").gameObject.SetActive(true);
                refreshUI();
                InventoryActive = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0.3f;
            }
            else if (InventoryActive == true)
            {
                InventoryMenu.transform.Find("_GUI").gameObject.SetActive(true);
                InventoryMenu.transform.Find("_INVENTORY").gameObject.SetActive(false);
                InventoryActive = false;
                //Cursor.visible = false; //fastidioso al momento
                //Cursor.lockState = CursorLockMode.Locked; //fastidioso al momento
                Time.timeScale = 1f;
            }
        }

        if (!InventoryActive && objects.Count > 0)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                unSelectObject();
                //selection = ;
                selectObject( (selection + 1) % objects.Count ) ;
                //print("Selected " + selection + objects[selection].GetComponent<InventoryObject>().objectName);
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                unSelectObject();
                //print("( " + selection + " - 1 ) % " + objects.Count);
                // quanto fa (0 - 1) % 2 ????? nada
                //selection = selection == 0 ? objects.Count - 1 : (selection - 1) ;
                selectObject( selection == 0 ? objects.Count - 1 : (selection - 1) );
                //print("Selected " + selection + objects[selection].GetComponent<InventoryObject>().objectName);
            }
            if (Input.GetKeyDown(KeyCode.Mouse1)) //drop
            {
                dropSelectedObject();

            }

            if (Input.GetKeyDown(KeyCode.Q)) //unequip everything
            {
                unSelectObject();
                selection = -1;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0)) // use
            {
                if (selection >= 0)
                {
                    GameObject obj = objects[selection];
                    InventoryObject inv = obj.GetComponent<InventoryObject>();
                    //print("Using " + inv.objectName);
                    inv.useObject();
                }
            }
        }
    }

    public void dropSelectedObject()
    {
        print("Dropping obj");
        GameObject obj = objects[selection];
        InventoryObject inv = obj.GetComponent<InventoryObject>();
        inv.dropObject();
        objects.RemoveAt(selection);
        obj.transform.position = transform.position + transform.forward * 2;
        selectObject(selection >= objects.Count ? objects.Count - 1 : selection);
        //selection = selection >= objects.Count ? objects.Count - 1 : selection;
        //unSelectObject();
        //obj.SetActive(true);
        //obj = objects[selection];
    }

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InventoryObject>() ) //per ora seleziona tutti gli oggetti nelle vicinanze
        {
            //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other);
            GameObject obj = other.gameObject;
            InventoryObject inv = obj.GetComponent<InventoryObject>();
            //print("Pickable object:" + other.GetComponent<InventoryObject>().objectName);
            RaycastHit hit;
            Ray ray = data.control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))//
            {
                //print("Pressing button E:" + other.GetComponent<InventoryObject>().objectName);

                if (hit.collider.gameObject == obj)
                {
                    //print("Mouse hit " + hit.collider.objectName + " vs " + obj.objectName);
                    if (Input.GetKeyDown(KeyCode.E) && inv.pickable)
                    { // 5 graffe per determinare se posso raccogliere un oggetto??? FuckOff
                        //print("Taking");
                        objects.Add(obj);
                        obj.SetActive(false);
                        //GameObject.Destroy(obj.gameObject);
                        unSelectObject();
                        selectObject(objects.Count - 1);
                        print("Picked " + selection + inv.objectName);
                        if (InventoryActive) refreshUI();
                    }
                    if (Input.GetKeyDown(KeyCode.Mouse0) && selection <0 && !inv.pickable)
                    {
                        //print("using " + inv.objectName);
                        inv.useObject();
                    }
                }
            }
        }
    }
    /*
     *     void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InventoryObject>() ) //per ora seleziona tutti gli oggetti nelle vicinanze
        {
            //Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other);
            GameObject obj = other.gameObject;
            InventoryObject inv = obj.GetComponent<InventoryObject>();
            //print("Pickable object:" + other.GetComponent<InventoryObject>().objectName);
            if (Input.GetKeyDown(KeyCode.E) && inv.pickable)
            {
                //print("Pressing button E:" + other.GetComponent<InventoryObject>().objectName);
                RaycastHit hit;
                Ray ray = data.control.cam.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //print("Mouse hit " + hit.collider.objectName + " vs " + obj.objectName);
                    if (hit.collider.gameObject == obj )
                    { // 5 graffe per determinare se posso raccogliere un oggetto??? FuckOff
                        //print("Taking");
                        objects.Add(obj);
                        obj.SetActive(false);
                        //GameObject.Destroy(obj.gameObject);
                        unSelectObject();
                        selectObject(objects.Count - 1);
                        print("Picked " + selection + inv.objectName);
                        if (InventoryActive) refreshUI();
                    }
                }
            }
        }
    } /**/

    public void selectObject(int i)
    {
        //unSelectObject();
        print("Requesting " + i);
        //need to unequip previous item
        selection = Mathf.Min(i, objects.Count-1);
        if (selection >= 0)
        {
            InventoryObject inv = objects[selection].GetComponent<InventoryObject>();
            print("Selected " + selection + inv.objectName);
            inv.equipObject();
            if (!InventoryActive)
            {
                //transform.Find("Object").GetComponent<MeshFilter>().mesh  = objects[selection].GetComponent<MeshFilter>().mesh;
            }
        }
        if (InventoryActive) refreshUI();
    }
    public void unSelectObject()
    {
        print("Unequipping " + selection);
        if (selection >= 0 && objects.Count > 0)
        {
            InventoryObject inv = objects[selection].GetComponent<InventoryObject>();
            print("Unselected " + selection + inv.objectName);
            inv.unEquipObject();
        } else { print("Empty bag"); }
        //if (InventoryActive) refreshUI();
    }


    void refreshUI()
    {
        //Canvas button = InventoryMenu.transform.Find("_INVENTORY").transform.Find("ObjectButton").;
        float yMov = 0;//button.transform.position.y;
        int count = 0;
        if (uiObjects.Count > 0) //try
        {
            foreach (GameObject uiobj in uiObjects)
            {
                GameObject.Destroy(uiobj);
            }
            uiObjects.Clear();
        } //catch  { print("Nothing to remove"); }
        //list of objects
        foreach(GameObject obj in objects)
        {
            GameObject uiobj = UnityEngine.Object.Instantiate(objectButton);
            uiobj.transform.SetParent(objectButton.transform.parent);
            
            uiobj.transform.position = objectButton.transform.position;
            uiobj.transform.Translate(new Vector3(0, yMov, 0));

            uiobj.gameObject.SetActive(true);
            //uiobj.transform.Find("Icon").GetComponent<UnityEngine.UI.Image>() = obj.GetComponent<InventoryObject>().icon;
            uiobj.transform.Find("Clickable").GetComponent<TMPro.TextMeshProUGUI>().text = "- " + count + obj.GetComponent<InventoryObject>().objectName;
            uiobj.transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = obj.GetComponent<InventoryObject>().icon;
            uiobj.transform.Find("Clickable").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate {
                unSelectObject();
                selectObject(uiObjects.IndexOf(uiobj));}) ;
            //print("Button " + count + " assigned to function delegate { selectObject(" + count + ")}");
            uiobj.SetActive(true);
            uiObjects.Add(uiobj);
            yMov += menuObjectsOffset;
            count += 1;
        }
        //refresh other elements in UI
        //selection
        if (selection >= 0)
        {
            GameObject obj = objects[selection];
            InventoryObject inv = obj.GetComponent<InventoryObject>();
            selectionArea.SetActive(true);
            selectionArea.transform.Find("SelectionName").GetComponent<TMPro.TextMeshProUGUI>().text = inv.objectName;
            selectionArea.transform.Find("SelectionDescription").GetComponent<TMPro.TextMeshProUGUI>().text = inv.description;
            selectionArea.transform.Find("SelectionIcon").GetComponent<UnityEngine.UI.Image>().sprite = inv.icon;
            //selectionArea.transform.Find("Canvas").GetComponent<Canvas>.
        } else selectionArea.SetActive(false);
        //health (and energy)
        InventoryMenu.transform.Find("_INVENTORY").Find("_HealthAmount").GetComponent<TMPro.TextMeshProUGUI>().text = playerHealth + "/" + maxHealth;
        InventoryMenu.transform.Find("_INVENTORY").Find("_EnergyAmount").GetComponent<TMPro.TextMeshProUGUI>().text = playerEnergy + "/" + maxEnergy;
        //todo
    }


    

}
