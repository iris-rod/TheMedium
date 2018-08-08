using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

  //Used to switch layers when fading
  private bool hide;
  private bool moving;
  private bool canPickup;
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
	}
	
	// Update is called once per frame
	void Update () {
    if (Input.GetMouseButtonDown(0))
    {
      SetEndPosition();    
      moving = true;
    }
    if (moving)
      Move();
  }



  private void SetEndPosition()
  {
    var aux = Input.mousePosition;
    aux.z = 10.0f;
    endPoint = Camera.main.ScreenToWorldPoint(aux);
    //Move left
    if (endPoint.x < transform.position.x)
    {
      animator.SetBool("LookingLeft", true);
      animator.SetBool("LookingRight", false);
    }
    //Move right
    else
    {
      animator.SetBool("LookingLeft", false);
      animator.SetBool("LookingRight", true);
    }
  }

  public void Move()
  {
    animator.SetBool("Move", true);
    float step = Speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, endPoint, step);
    if (transform.position == endPoint)
    {
      moving = false;
      animator.SetBool("Move", false);
    }

  }

  void OnCollisionStay2D(Collision2D col)
  {
    if (col.transform.CompareTag("Button"))
    {
      SM.OpenPanel();
    }
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
    else if (col.transform.CompareTag("Interactable"))
    {
      if (col.transform.GetComponent<Interactable>().CanInteract())
      {
        col.transform.GetComponent<Interactable>().StartInteraction();
      }
    }
    moving = false;
    transform.position = Vector2.MoveTowards(transform.position, col.transform.position, -1 * 10 * Time.deltaTime);
    animator.SetBool("Move", false);
  }

  void Pickup()
  {
    InvManager.AddItem(itemToPick.transform.GetComponent<Pickable>());
    ItManager.RemoveItem(itemToPick.gameObject);
    canPickup = true;
  }

}
