using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Data/Artifact")]
public class Artifact : ScriptableObject
{
    public string ID = Guid.NewGuid().ToString();
    public string FriendlyName;
    public string Description;
    public int InventoryIndex;
    public Sprite Icon;
    public GameObject gameObject;
}