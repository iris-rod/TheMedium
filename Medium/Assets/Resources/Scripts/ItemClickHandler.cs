using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemClickHandler : MonoBehaviour, IPointerClickHandler
{
  private bool selected;
  private InventoryManager InvManager;
  private GameObject outline;

  void Start()
  {
    InvManager = transform.parent.parent.parent.GetComponent<InventoryManager>();
    outline = transform.parent.gameObject;
  }

  // Check if the player clicked on the inventory - selects the item in the inventory to be used
  public void OnPointerClick(PointerEventData eventData)
  {
    if (!GetComponent<ItemDragAndDropHandler>().IsDragging())
    {
      bool hasItem = !GetComponent<Slot>().IsFree();
      if (!selected)
      {
        if (hasItem && !InvManager.HasItemAlreadySelected())
        {
          outline.transform.GetComponent<Outline>().effectColor = Color.yellow;
          selected = !selected;
        }
      }
      else
      {
        selected = !selected;
        outline.transform.GetComponent<Outline>().effectColor = Color.gray;
      }
    }else
    {
      GetComponent<ItemDragAndDropHandler>().OnEndDrag(eventData);
      GetComponent<ItemDragAndDropHandler>().OnDrop(eventData);
      outline.transform.GetComponent<Outline>().effectColor = Color.gray;
    }
  }

  public bool IsSelected()
  {
    return selected;
  }

  public void SetSelected(bool value)
  {
    selected = value;
    if(!value)
      outline.transform.GetComponent<Outline>().effectColor = Color.gray;
  }

}
