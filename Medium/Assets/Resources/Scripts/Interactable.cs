using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

  public string TypeOfInteraction;
  public string ItemNeeded;
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
    if (TypeOfInteraction.ToLower() == "destroy")
    {
      Sprite[] allSprites = Resources.LoadAll<Sprite>("Sprites/" + split[0].ToLower());
      string newName = split[0] + "_" + numberOfClicks;
      Sprite newImg = GetSpriteByName(allSprites, newName);
      int nextClick = numberOfClicks + 1;
      Sprite hasNextImg = GetSpriteByName(allSprites, split[0] + "_" + nextClick);
      if (newImg != null)
        GetComponent<Image>().sprite = newImg;
      if (hasNextImg == null && hasReward)
        SetReward();
    }
    else if(TypeOfInteraction.ToLower() == "alter")
    {
      if (hasReward)
        SetReward();
      if(name != "standing_lamp")
        Destroy(transform.gameObject);
    }
  }

  private void SetReward()
  {
    if (!Reward.Contains("unlock"))
    {
      ItManager.CreateItem(Reward, null);
      ItManager.GiveItem(Reward, transform.position + new Vector3(0, -60, 0));
      InvManager.RemoveItem(ItemNeeded);
    }
    else Unlock();
    hasReward = false;
  }

  private void Unlock()
  {
    if (Reward.Contains("door"))
    {
      GameObject door = transform.parent.gameObject;
      door.GetComponent<Button>().interactable = true;
      door.GetComponent<Animator>().SetBool("open", true);
      transform.position += new Vector3(0, -10, 0);
      transform.GetComponent<BoxCollider2D>().isTrigger = true;
      InvManager.RemoveItem("key");
    }else if (Reward.Contains("light"))
    {
      GameObject.Find("DarkRoom").GetComponent<Animator>().SetBool("Fade", true);
      InvManager.RemoveItem("lamp");
    }
  }

  private void CheckIfHasNeededItem()
  {
    hasItemNeeded = InvManager.CheckForItem(ItemNeeded);
    if (ItemNeeded == "") hasItemNeeded = true;
  }

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
