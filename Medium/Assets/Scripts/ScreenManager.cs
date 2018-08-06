﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{

  //Screen to open automatically at the start of the Scene
  public Animator initiallyOpen;

  //Black image used to create fade in and out
  public Animator fadeAnim;

  private Animator receivedAnim;

  //Currently Open Screen
  private Animator currentOpen;

  //Hash of the parameter we use to control the transitions.
  private int OpenParameterId;

  //The GameObject Selected before we opened the current Screen.
  //Used when closing a Screen, so we can go back to the button that opened it.
  private GameObject PreviouslySelected;

  //Animator State and Transition names we need to check against.
  const string OpenTransitionName = "Open";
  const string ClosedStateName = "Closed";

  public void OnEnable()
  {
    FadeIn();
    //We cache the Hash to the "Open" Parameter, so we can feed to Animator.SetBool.
    OpenParameterId = Animator.StringToHash(OpenTransitionName);

    //If set, open the initial Screen now.
    if (initiallyOpen == null)
      return;
    OpenPanel(initiallyOpen);
  }

  //Closes the currently open panel and opens the provided one.
  //It also takes care of handling the navigation, setting the new Selected element.
  public void OpenPanel(Animator anim)
  {
    if (currentOpen == anim)
      return;

    FadeOut();
    //Activate the new Screen hierarchy so we can animate it.
    anim.gameObject.SetActive(true);
    //Save the currently selected button that was used to open this Screen. (CloseCurrent will modify it)
    if (EventSystem.current != null)
    {
      var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
      PreviouslySelected = newPreviouslySelected;
    }
    //Move the Screen to front.
    anim.transform.SetAsLastSibling();

    receivedAnim = anim;
    
    //If it is the begining of the game, open the initial scene
    if(currentOpen == null)
    {
      currentOpen = anim;
      //Start the open animation
      currentOpen.SetBool(OpenParameterId, true);
      Invoke("FadeIn", 2.5f);
    }
    //If it isnt the begining of the game, wait for a while for the fade out to be complete before closing and opening the scenes
    else
      Invoke("MoveScenes",1f);

    //Set an element in the new screen as the new Selected one.
    GameObject go = FindFirstEnabledSelectable(anim.gameObject);
    SetSelected(go);
  }

  private void MoveScenes()
  {
    //Invoke("CloseCurrent", .5f);
    CloseCurrent();
    //Set the new Screen as then open one.
    currentOpen = receivedAnim;
    //Start the open animation
    currentOpen.SetBool(OpenParameterId, true);
    Invoke("FadeIn", 2.5f);
  }

  //Finds the first Selectable element in the providade hierarchy.
  static GameObject FindFirstEnabledSelectable(GameObject gameObject)
  {
    GameObject go = null;
    var selectables = gameObject.GetComponentsInChildren<Selectable>(true);
    foreach (var selectable in selectables)
    {
      if (selectable.IsActive() && selectable.IsInteractable())
      {
        go = selectable.gameObject;
        break;
      }
    }
    return go;
  }

  //Closes the currently open Screen
  //It also takes care of navigation.
  //Reverting selection to the Selectable used before opening the current screen.
  public void CloseCurrent()
  {
    if (currentOpen == null)
      return;

    //Start the close animation.
    currentOpen.SetBool(OpenParameterId, false);

    //Reverting selection to the Selectable used before opening the current screen.
    if(PreviouslySelected != null)
      SetSelected(PreviouslySelected);
    //Start Coroutine to disable the hierarchy when closing animation finishes.
    StartCoroutine(DisablePanelDeleyed(currentOpen));
    //No screen open.
    currentOpen = null;
  }

  //Coroutine that will detect when the Closing animation is finished and it will deactivate the
  //hierarchy.
  IEnumerator DisablePanelDeleyed(Animator anim)
  {
    bool closedStateReached = false;
    bool wantToClose = true;
    while (!closedStateReached && wantToClose)
    {
      if (!anim.IsInTransition(0))
        closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(ClosedStateName);

      wantToClose = !anim.GetBool(OpenParameterId);

      yield return new WaitForEndOfFrame();
    }

    if (wantToClose)
      anim.gameObject.SetActive(false);
  }

  //Make the provided GameObject selected
  //When using the mouse/touch we actually want to set it as the previously selected and 
  //set nothing as selected for now.
  private void SetSelected(GameObject go)
  {
    //Select the GameObject.
    if(EventSystem.current != null)
    EventSystem.current.SetSelectedGameObject(go);

  }

  private void FadeIn()
  {
    fadeAnim.SetBool("Fade", true);

  }

  private void FadeOut()
  {
    fadeAnim.SetBool("Fade", false);

  }

}