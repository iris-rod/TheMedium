using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemManager : MonoBehaviour {

  private GameObject currentScene;
  Dictionary<string, GameObject> itens;

  private GameObject itemGO;
  private InventoryManager InvManager;

  private Sprite[] sprites;

  // Use this for initialization
  void Start () {
    itens = new Dictionary<string, GameObject>();
    InvManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    sprites = Resources.LoadAll<Sprite>("Sprites/items");

    GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
    GameObject[] pickables = GameObject.FindGameObjectsWithTag("Pickable");

    AddItems(interactables);
    AddItems(pickables);
  }

  public Sprite GetSpriteByName(string name)
  {
    for (int i = 0; i < sprites.Length; i++)
    {
      if (sprites[i].name == name)
        return sprites[i];
    }
    return null;
  }

  public void CreateItem(string name, GameObject scene)
  {
    Sprite icon = null;
    Sprite image = null;

    if (name != "" && name != null)
    {
      icon = GetSpriteByName(name); //Resources.Load("Sprites/" + name , typeof(Sprite)) as Sprite;
      image = GetSpriteByName(name); //Resources.Load("Sprites/" + name, typeof(Sprite)) as Sprite;
    }

    itemGO = new GameObject();
    itemGO.AddComponent<Button>();

    itemGO.AddComponent<Image>();
    itemGO.GetComponent<Image>().sprite = image;
    itemGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 60);
    itemGO.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 60);

    itemGO.AddComponent<CircleCollider2D>();
    itemGO.GetComponent<CircleCollider2D>().radius = 20;

    itemGO.AddComponent<Pickable>();
    itemGO.GetComponent<Pickable>().Setup(name, 1, icon);
    itemGO.GetComponent<Button>().onClick.AddListener(() => {
      
      itens[name].GetComponent<Pickable>().Clicked();
      InvManager.CanAdd(itens[name].GetComponent<Pickable>());
    });

    itemGO.tag = "Pickable";
    if (scene == null)
      SetItemInCurrentScene(itemGO);
    else SetItemInSpecificScene(itemGO,scene);

    itemGO.SetActive(false);
    itens.Add(name, itemGO);
  }

  public void GiveItem(string name, Vector3 position)
  {
    GameObject item = itens[name];
    item.SetActive(true);
    item.transform.position = position;
  }

  public void SetCurrentScene(GameObject go)
  {
    currentScene = go;
  }

  public void RemoveItem(GameObject pickable)
  {
    itens.Remove(pickable.GetComponent<Pickable>().Name);
    Destroy(pickable);
  }


  private void SetItemInCurrentScene(GameObject item)
  {
    item.transform.parent = currentScene.transform;
  }

  private void SetItemInSpecificScene(GameObject item, GameObject scene)
  {
    item.transform.parent = scene.transform;
  }

  private void AddItems(GameObject[] newEntries)
  {
    for(int i = 0; i< newEntries.Length; i++)
    {
      itens.Add(newEntries[i].name.ToLower(),newEntries[i] );
    }
  }


}
