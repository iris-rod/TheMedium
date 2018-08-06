using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragAndDropHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler {

  private Vector3 initialPos, screenPoint, offset;
  private bool begin = true;

  public void OnDrag(PointerEventData eventData)
  {
    if (!GetComponent<Slot>().IsFree())
    {
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

  public void OnEndDrag(PointerEventData eventData)
  {
    if (!GetComponent<Slot>().IsFree())
    {
      transform.position = initialPos;
      begin = true;
    }
  }

  public void OnDrop(PointerEventData eventData)
  {
    RectTransform inventory = transform.parent as RectTransform;
    if (!RectTransformUtility.RectangleContainsScreenPoint(inventory, Input.mousePosition))
    {
      transform.parent.parent.GetComponent<InventoryManager>().RemoveItem(GetComponent<Slot>().GetItemName());
    }
    transform.position = initialPos;



  }
}
