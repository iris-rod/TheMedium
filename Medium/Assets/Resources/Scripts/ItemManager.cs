using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

  private GameObject currentScene;
  Dictionary<string, GameObject> itens;

  private GameObject itemGO;
  private InventoryManager InvManager;

	// Use this for initialization
	void Start () {
    itens = new Dictionary<string, GameObject>();
    InvManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  public void CreateItem(string name)
  {
    Sprite icon = null;
    Sprite image = null;
    if (name != "" && name != null)
    {
      icon = Resources.Load("Sprites/" + name , typeof(Sprite)) as Sprite;
      image = Resources.Load("Sprites/" + name, typeof(Sprite)) as Sprite;
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
      InvManager.CanAdd(itemGO.GetComponent<Pickable>());
      itemGO.GetComponent<Pickable>().Clicked();
    });

    itemGO.tag = "Pickable";
    SetItemInScene(itemGO);

    itemGO.SetActive(false);
    itens.Add(name, itemGO);
  }

  public void GiveItem(string name, Vector3 position)
  {
    GameObject item = itens[name];
    item.SetActive(true);
    item.transform.position =  position;
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

  private void SetItemInScene(GameObject item)
  {
    item.transform.parent = currentScene.transform;
  }

}
