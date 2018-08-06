using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

  //Used to switch layers when fading
  private bool hide;
  private bool moving;
  private Vector3 endPoint;
  private Animator animator;

  //Moving speed
  public float Speed;


	// Use this for initialization
	void Start () {
    animator = transform.GetComponent<Animator>();
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
  }

  public void Move()
  {
    //Move left
    if (endPoint.x < transform.position.x)
    {
      animator.SetBool("MoveLeft", true);
      animator.SetBool("MoveRight", false);
    }
    //Move right
    else
    {
      animator.SetBool("MoveLeft", false);
      animator.SetBool("MoveRight", true);
    }
    float step = Speed * Time.deltaTime;
    transform.position = Vector3.MoveTowards(transform.position, endPoint, step);
    if (transform.position == endPoint)
    {
      moving = false;
      animator.SetBool("MoveLeft", false);
      animator.SetBool("MoveRight", false);
    }

  }

}
