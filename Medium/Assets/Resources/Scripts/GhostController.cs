using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour {

  public string Item;
  
  private Sprite inventoryIcon;
  private Sprite itemSprite;
  private bool give,canGive;
  private ItemManager ItM;
  private Animator anim;

	// Use this for initialization
	void Start () {
    anim = GetComponent<Animator>();
    ItM = GameObject.Find("Manager").GetComponent<ItemManager>();
    give = false;
    canGive = true;
    ItM.CreateItem(Item, transform.parent.gameObject);
  }
	
  void Update()
  {   
  }

  void OnCollisionEnter2D(Collision2D col)
  {
    if (col.transform.CompareTag("Player") && give)
    {
      if (canGive)
      {
        anim.SetTrigger("GiveItem");
        ItM.GiveItem(Item,transform.position + new Vector3(0, 60, 0));
        give = false;
        canGive = false;
      }
    }
  }
  
  public void GiveItem()
  {
    give = true;
  }
}
