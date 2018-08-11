using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {
  // An interactable can be of 'destroy' or 'alter'
  public string TypeOfInteraction;
  // The item needed to be able to interact with the item
  public string ItemNeeded;
  // The reward that comes when the interaction is finished
  public string Reward;

  private bool canInteract, startInteraction;
  private bool hasReward;
  private bool hasItemNeeded;
  private int numberOfClicks;
  private ItemManager ItManager;
  private InventoryManager InvManager;

	// Use this for initialization
	void Start () {
    InvManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    ItManager = GameObject.Find("Manager").GetComponent<ItemManager>();
    canInteract = false;
    startInteraction = false;
    hasItemNeeded = false;
    hasReward = true;
    numberOfClicks = 0;
	}
	
  
  void Update()
  {
    if (startInteraction)
    {
      UpdateImage();
      startInteraction = false;
      canInteract = false;
    }
  }

  private Sprite GetSpriteByName(Sprite[] sprites, string name)
  {
    for (int i = 0; i < sprites.Length; i++)
    {
      if (sprites[i].name == name)
        return sprites[i];
    }
    return null;
  }

  
  private void UpdateImage()
  {
    string name = GetComponent<Image>().sprite.name;
    string[] split = name.Split('_');
    // If the interactable is the 'destroy' type, then with each click, the sprite of item is update, 
    // to simulate the destruction with the clicks. When there are no more images, the reward is given
    if (TypeOfInteraction.ToLower() == "destroy")
    {
      Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/" + split[0].ToLower());
      string newName = split[0] + "_" + numberOfClicks;
      Sprite newImg = GetSpriteByName(allSprites, newName);
      int nextClick = numberOfClicks + 1;
      Sprite hasNextImg = GetSpriteByName(allSprites, split[0] + "_" + nextClick);
      SoundManager.PlaySound("chest");
      if (newImg != null)
        GetComponent<Image>().sprite = newImg;
      if (hasNextImg == null && hasReward)
        SetReward();
    }
    // If the interactable is the 'alter' type, it only needs one click and it generates the reward
    else if(TypeOfInteraction.ToLower() == "alter")
    {
      if (hasReward)
        SetReward();
      if(name != "standing_lamp")
        Destroy(transform.gameObject);
    }
  }

  // Gives the reward from the interaction with the item
  private void SetReward()
  {
    // If the reward isnt to unlock something, it means it is a new item
    if (!Reward.Contains("unlock"))
    {
      ItManager.CreateItem(Reward, null);
      ItManager.GiveItem(Reward, transform.position + new Vector3(0, -60, 0));
      InvManager.RemoveItem(ItemNeeded);
      if (Reward.Contains("hat")) SoundManager.PlaySound("ribbon");
      else if (Reward.Contains("flower")) SoundManager.PlaySound("cut");
    }
    else Unlock();
    hasReward = false;
  }

  
  private void Unlock()
  {
    // If the reward is to unlock a door, it starts the animation to open the door, and the animation to remove the lock
    if (Reward.Contains("door"))
    {
      GameObject door = transform.parent.gameObject;
      door.GetComponent<Button>().interactable = true;
      door.GetComponent<Animator>().SetBool("open", true);
      transform.position += new Vector3(0, -10, 0);
      transform.GetComponent<BoxCollider2D>().isTrigger = true;
      InvManager.RemoveItem("key");
      SoundManager.PlaySound("open");
    }
    // If the reward is to unlock a light, it starts the animation of fade in of the black screen
    else if (Reward.Contains("light"))
    {
      GameObject.Find("DarkRoom").GetComponent<Animator>().SetBool("Fade", true);
      InvManager.RemoveItem("lamp");
      SoundManager.PlaySound("bulb");
    }
  }

  // Check if the player has the item needed to interact with the item
  private void CheckIfHasNeededItem()
  {
    hasItemNeeded = InvManager.CheckForItem(ItemNeeded);
    if (ItemNeeded == "") hasItemNeeded = true;
  }

  // Used in the event listener onClick - sets the item as an interactable , if the player has 
  // the necessary item to interact with it
  public void Clicked()
  {
    CheckIfHasNeededItem();
    if (!canInteract && hasItemNeeded)
    {
      canInteract = true;
      numberOfClicks++;
    }
  }

  public bool CanInteract()
  {
    return canInteract;
  }

  public void StartInteraction()
  {
    startInteraction = true;
  }

}
