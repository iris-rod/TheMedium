using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour {

  private GameObject player;
  private GameObject[] npcs;

	// Use this for initialization
	void Awake () {
    player = GameObject.FindGameObjectWithTag("Player");
    npcs = GameObject.FindGameObjectsWithTag("NPC");
	}

  public void FadeIn()
  {
    if (player != null)
    {
      player.GetComponent<SpriteRenderer>().sortingOrder = 1;
      for (int i = 0; i < npcs.Length; i++)
        npcs[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
  }
  public void FadeOut()
  {
    if (player != null)
    {
      player.GetComponent<SpriteRenderer>().sortingOrder = 0;
      for (int i = 0; i < npcs.Length; i++)
        npcs[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
  }

}
