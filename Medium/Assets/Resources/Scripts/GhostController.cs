using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour {

  public string Item;
  public bool Male;
  public string Receives;

  private Sprite inventoryIcon;
  private Sprite itemSprite;
  private bool interact,canInteract,canReceive;
  private ItemManager ItM;
  private InventoryManager InvManager;
  private CharactersManager CharManager;
  private Animator anim;
  private bool show, endShow, playerEndedInteraction;

  // Use this for initialization
  void Start()
  {
    anim = GetComponent<Animator>();
    ItM = GameObject.Find("Manager").GetComponent<ItemManager>();
    InvManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    CharManager = GameObject.Find("Manager").GetComponent<CharactersManager>();
    interact = false;
    show = false;
    endShow = false;
    canInteract = true;
    canReceive = true;
    playerEndedInteraction = false;
    ItM.CreateItem(Item, transform.parent.gameObject);
    if (Male)
    {
      anim.SetBool("LookingRight", true);
      anim.SetBool("LookingLeft", false);
    }
  }

  void Update()
  {
    if(show && playerEndedInteraction)
    {
      anim.SetBool("Show", true);
      endShow = true;
      show = false;
      playerEndedInteraction = false;
      Invoke("ResetShow", 1.2f);
    }
    else if(interact && playerEndedInteraction)
    {
      if (canInteract)
      {
        anim.SetTrigger("GiveItem");
        ItM.GiveItem(Item, transform.position + new Vector3(0, 60, 0));
        interact = false;
        canInteract = false;
      }
      else if (canReceive)
      {
        string name = Receives + "_after";
        if (InvManager.CheckForItem(name))
        {
          ItM.CreateItem(name, null);
          anim.SetTrigger("GiveItem");
          ItM.GiveItem(name, transform.position + new Vector3(0, 60, 0));
          interact = false;
          canReceive = false;
          anim.SetBool("Receive", true);
          Invoke("ReceiveItem", 1f);
        }

      }
    }
    playerEndedInteraction = false;
  }

  private void ReceiveItem()
  {
    ItM.RemoveItemByName(Receives + "_after");
    Invoke("SetCharacter",1);
  }

  void SetCharacter()
  {
    CharManager.SetCharacterFinalScene(transform.gameObject);
  }

  private void ResetShow()
  {
    anim.SetBool("Show",false);
  }
  
  public void Clicked()
  {
    //Interact with the NPC only he has already shown the initial animation
    if (!show && endShow)
      interact = true;
    //Show the initial animation
    else if (!endShow)
      show = true;
    //If he can receive the item the player needs to give him, the animation is played
    else if (canReceive)
    {
      interact = true;
    }
  }

  public void PlayerEndedInteractions(bool value)
  {
    playerEndedInteraction = value;
  }

  public void SetCanReceive(bool value)
  {
    canReceive = value;
  }

}
