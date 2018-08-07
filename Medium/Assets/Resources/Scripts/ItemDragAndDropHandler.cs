using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragAndDropHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler {

  private Vector3 initialPos, screenPoint, offset;
  private bool begin = true;
  private ItemManager ItManager;

  void Start()
  {
    ItManager = GameObject.Find("Manager").GetComponent<ItemManager>();
  }

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
    string name = "";
    RectTransform inventory = transform.parent as RectTransform;
    if (!RectTransformUtility.RectangleContainsScreenPoint(inventory, Input.mousePosition))
    {
      name = GetComponent<Slot>().GetItemName();
      transform.parent.parent.GetComponent<InventoryManager>().RemoveItem(name);
    }
    transform.position = initialPos;

    ItManager.CreateItem(name);
    var v3 = Input.mousePosition;
    v3.z = 10.0f;
    v3 = Camera.main.ScreenToWorldPoint(v3);
    ItManager.GiveItem(name,v3);

  }
}
