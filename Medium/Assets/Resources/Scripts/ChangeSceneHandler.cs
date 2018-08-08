﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSceneHandler : MonoBehaviour {

  private GameObject player;
  private string buttonPressed;
  private GameObject buttonToScene3;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
    buttonToScene3 = GameObject.Find("ButtonUp").gameObject;
    buttonToScene3.GetComponent<Button>().interactable = false;
  }

  //Place the player at the right side when he moves between scenes
	public void SetScene(GameObject button)
  {
    buttonPressed = button.name.Substring(6);
  }


  public void ChangeScene()
  {
    if (buttonPressed != "" && buttonPressed != null)
    {
      switch (buttonPressed.ToLower())
      {
        case "left":
          player.transform.position = new Vector3(285, -15, 3);
          break;
        case "right":
          player.transform.position = new Vector3(-265, -15, 3);
          break;
        case "up":
          player.transform.position = new Vector3(4, -70, 3);
          break;
        case "down":
          player.transform.position = new Vector3(4, 60, 3);
          break;
      }
    }
  }
}
