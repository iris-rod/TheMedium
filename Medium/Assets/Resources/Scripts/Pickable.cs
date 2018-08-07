using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour {

  public string Name;
  public int Quantity;
  public Sprite InventoryIcon;

  private bool canBePicked = false;

  public void Setup(string name, int value, Sprite icon)
  {
    Name = name;
    Quantity = value;
    InventoryIcon = icon;
  }

  public void Clicked()
  {
    canBePicked = true;
  }

  public bool CanBePicked()
  {
    return canBePicked;
  }

}
