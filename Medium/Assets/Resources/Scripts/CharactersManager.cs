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

  // Show the player and the ghosts
  // Place the ghosts on the layer above the scene, and change the alpha of the player to 1
  public void FadeIn()
  {
    if (player != null)
    {
      GetComponent<ChangeSceneHandler>().ChangeScene();

      Color c = player.GetComponent<SpriteRenderer>().color;
      c.a = 1;
      player.GetComponent<SpriteRenderer>().color = c;

      for (int i = 0; i < npcs.Length; i++)
        npcs[i].GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
  }

  // Hide the player and the ghosts
  // Place the ghosts on a layer bellow the scenes and change the alpha of the player to 0
  public void FadeOut()
  {
    if (player != null)
    {
      Color c = player.GetComponent<SpriteRenderer>().color;
      c.a = 0;
      player.GetComponent<SpriteRenderer>().color = c;
      for (int i = 0; i < npcs.Length; i++)
        npcs[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
    }
  }

  public void SetCharacterFinalScene(GameObject charGO)
  {
    GetComponent<ScreenManager>().FadeOut();
    StartCoroutine(FinalSceneSetUp(charGO, 1.5f));
  }

  //Create Fadeout effect in the current scene, then setup the ghost final position, and then perform fadein
  IEnumerator FinalSceneSetUp(GameObject ghost, float delay)
{
  yield return new WaitForSeconds(delay);
    ghost.transform.parent = GameObject.Find("MainScene").transform;
    if (ghost.GetComponent<GhostController>().Receives.ToLower() == "flower")
      ghost.transform.localPosition = new Vector3(30, -10, 3);
    else if (ghost.GetComponent<GhostController>().Receives.ToLower() == "hat")
      ghost.transform.localPosition = new Vector3(-30, -10, 3);
    GetComponent<ScreenManager>().FadeIn();
  }
}
