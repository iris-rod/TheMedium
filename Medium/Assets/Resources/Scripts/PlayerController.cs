using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  //Used to switch layers when fading
  private bool hide;
  private bool moving;
  private bool canPickup, canInteract;
  private Vector3 endPoint;
  private Animator animator;
  private ScreenManager SM;
  private InventoryManager InvManager;
  private ItemManager ItManager;
  private GameObject itemToPick;

  //Moving speed
  public float Speed;


	// Use this for initialization
	void Start () {
    SM = GameObject.Find("Manager").GetComponent<ScreenManager>();
    InvManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    ItManager = GameObject.Find("Manager").GetComponent<ItemManager>();
    animator = transform.GetComponent<Animator>();

    canPickup = true;
    canInteract = true;
	}
	
	
	void Update () {
    // The player only moves to the position clicked if it wasn't on the inventory
    if (Input.GetMouseButtonDown(0) && !InvManager.ClickedOnInventory())
    {
      SetEndPosition();    
      moving = true;
    }
    if (moving)
      Move();
  }


  // Sets the final position of the player, based on the position where he clicked.
  private void SetEndPosition()
  {
    SoundManager.PlaySound("walking_default");
    var aux = Input.mousePosition;
    aux.z = 10.0f;
    endPoint = Camera.main.ScreenToWorldPoint(aux);
    //Set animation to be looking left
    if (endPoint.x < transform.position.x)
    {
      animator.SetBool("LookingLeft", true);
      animator.SetBool("LookingRight", false);
    }
    //Set animation to be looking right
    else
    {
      animator.SetBool("LookingLeft", false);
      animator.SetBool("LookingRight", true);
    }
  }

  //Sets the animation to move and moves the character depending on its speed
  public void Move()
  {
    animator.SetBool("Move", true);
    float step = Speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, endPoint, step);
    if (transform.position == endPoint)
    {
      moving = false;
      animator.SetBool("Move", false);
      SoundManager.StopAudio();
    }

  }

  // Checks for collisions in the scene
  void OnCollisionStay2D(Collision2D col)
  {
    //If it collides with a 'Button', it means it's changing rooms
    if (col.transform.CompareTag("Button"))
    {
      SM.OpenPanel();
    }
    // If it collides with an pickable item, he picks it (if he already clicked on it)
    else if (col.transform.CompareTag("Pickable"))
    {
      if (canPickup && col.transform.GetComponent<Pickable>().CanBePicked())
      {
        itemToPick = col.gameObject;
        animator.SetTrigger("Item");
        Invoke("Pickup", 1f);
        canPickup = false;
      }
    }
    // If it collides with an interactable item, he can only interact if he already clicked on it
    else if (col.transform.CompareTag("Interactable"))
    {
      if (col.transform.GetComponent<Interactable>().CanInteract())
      {
        col.transform.GetComponent<Interactable>().StartInteraction();
      }
    }
    // By default, the player is pushed back when he collides with anything that has a collider, and stops moving
    moving = false;
    transform.position = Vector2.MoveTowards(transform.position, col.transform.position, -1 * 10 * Time.deltaTime);
    animator.SetBool("Move", false);
    if (col.transform.CompareTag("NPC") && canInteract)
    {
      animator.SetTrigger("Interact");
      canInteract = false;
      // A timer is set to indicate the end of the player animation of the interaction with a ghost
      StartCoroutine(EndInteraction(col.gameObject, 1.5f));
    }
  }

  // Whenever he collides with something, the audio clip of walking should be stopped
  void OnCollisionEnter2D(Collision2D col)
  {
    SoundManager.StopAudio();
  }

  // Function called used in the timer to indicate the end of the interaction with a ghost
  // (this way the animation ghosts only starts when the animation of the player ends)
  IEnumerator EndInteraction(GameObject ghost, float delay)
  {
    yield return new WaitForSeconds(delay);
    ghost.GetComponent<GhostController>().PlayerEndedInteractions(true);
    canInteract = true;
  }

  // Picks up an item, and it is added to the inventory and removed from the scene
  void Pickup()
  {
    SoundManager.PlaySound("pick");
    InvManager.AddItem(itemToPick.transform.GetComponent<Pickable>());
    ItManager.RemoveItem(itemToPick.gameObject);
    canPickup = true;
  }

}
