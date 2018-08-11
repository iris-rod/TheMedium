using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragAndDropHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler {

  private Vector3 initialPos, screenPoint, offset;
  private bool begin = true;
  private bool dragging = false;
  private ItemManager ItManager;

  void Start()
  {
    ItManager = GameObject.Find("Manager").GetComponent<ItemManager>();
  }

  // When dragging the slot, it follows the mouse position
  public void OnDrag(PointerEventData eventData)
  {
    if (!GetComponent<Slot>().IsFree())
    {
      dragging = true;
      if (begin)
      {
        initialPos = transform.position;
        begin = false;
      }
      var v3 = Input.mousePosition;
      v3.z = 10.0f;
      v3 = Camera.main.ScreenToWorldPoint(v3);
      transform.position = v3;
    }
  }

  // When he stops dragging, the slot returns to its original position
  public void OnEndDrag(PointerEventData eventData)
  {
    if (!GetComponent<Slot>().IsFree())
    {
      transform.position = initialPos;
      begin = true;
      dragging = false;
    }
  }

  // When the slot is dropped, the item is created in the scene, the slot returns to its original position, and the 
  // item is removed from the inventory
  public void OnDrop(PointerEventData eventData)
  {
    string name = "";
    RectTransform inventory = transform.parent as RectTransform;
    if (!RectTransformUtility.RectangleContainsScreenPoint(inventory, Input.mousePosition))
    {
      name = GetComponent<Slot>().GetItemName();
      transform.parent.parent.parent.GetComponent<InventoryManager>().RemoveItem(name);
    }
    transform.position = initialPos;

    ItManager.CreateItem(name, null);
    var v3 = Input.mousePosition;
    v3.z = 10.0f;
    v3 = Camera.main.ScreenToWorldPoint(v3);
    ItManager.GiveItem(name,v3);

  }

  public bool IsDragging()
  {
    return dragging;
  }

}
