using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    public List<InventorySlot> InventoryItems = new List<InventorySlot>();
    private VisualElement m_Root;
    private VisualElement m_SlotContainer;
    private void Start()
    {
        //Store the root from the UI Document component
        m_Root = GetComponent<UIDocument>().rootVisualElement;
        m_Root.style.visibility = Visibility.Hidden;
        //Search the root for the SlotContainer Visual Element
        m_SlotContainer = m_Root.Q<VisualElement>("SlotContainer");
        //Create InventorySlots and add them as children to the SlotContainer
        for (int i = 0; i < 24; i++)
        {
            InventorySlot item = new InventorySlot();
            InventoryItems.Add(item);
            m_SlotContainer.Add(item);
        }
    }

    public void ToggleVisibility()
    {
        m_Root.style.visibility = m_Root.style.visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
    }

    public bool AddCollectedItem(Artifact artifact)
    {
        InventoryItems[artifact.InventoryIndex].Icon.sprite = artifact.Icon;
        return true;
    }
}
