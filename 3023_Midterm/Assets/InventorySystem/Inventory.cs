using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.EventSystems;
using static UnityEngine.UI.Image;

public class Inventory : MonoBehaviour
{
    List<ItemSlot> itemSlots = new List<ItemSlot>();

    
    ItemSlot currentSlot;
    ItemSlot newSlot;

    Item heldItem;
    int heldCount;

    public int rows = 8;
    public int columns = 11;

    float tileWidth = 32;
    float tileHeight = 32;

    [SerializeField]
    GameObject inventoryPanel;

    RectTransform rectTransform;

    ItemSlot[,] slot2DArray;
    
    int index = 0;
    void Start()
    {

        rectTransform = inventoryPanel.GetComponent<RectTransform>();

        // Read all ItemSlot children
        itemSlots = new List<ItemSlot>(
            inventoryPanel.transform.GetComponentsInChildren<ItemSlot>()
        );

        // create a 2D array for the item slots based on the dimensions of the inventory
        slot2DArray = new ItemSlot[rows, columns];

        for (int j = 0; j < columns; j++)
        {
            for (int i = 0; i < rows; i++)
            {
                slot2DArray[i, j] = itemSlots[index];
                index++;
            }
        }

    }



        Vector2 gridPosition = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    // turn mouse position into grid position
    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
      

        gridPosition.x = mousePosition.x - rectTransform.position.x ;
        gridPosition.y = rectTransform.position.y - mousePosition.y ;

        tileGridPosition.x = (int)(gridPosition.x / tileWidth);
        tileGridPosition.y = (int)(gridPosition.y / tileHeight);

        return tileGridPosition;
    }

    void Update()
    {
        // self explanitory I should hope
        if (Input.GetMouseButtonDown(0))
        {

            GrabItem();
        }

        if (Input.GetMouseButtonUp(0))
        {
            PlaceItem();

        }
    }


    void PlaceItem()
    {
        
        newSlot = slot2DArray[GetGridPosition(Input.mousePosition).x, GetGridPosition(Input.mousePosition).y];
        Debug.Log(GetGridPosition(Input.mousePosition));

        if(newSlot.item == null)
        {
            if (FitTest(currentSlot.item, GetGridPosition(Input.mousePosition).x, GetGridPosition(Input.mousePosition).y))
            {

                newSlot.item = heldItem;
                newSlot.Count = heldCount;
                newSlot.UpdateGraphic();
                currentSlot.UpdateGraphic();
                currentSlot.item = null;
                currentSlot.Count = 0;
            }
        
           

        }
        else
        {
            Debug.Log("Slot full!");
            heldItem = null;
            heldCount = 0;
        }

        heldItem = null;
        heldCount = 0;
    }

    // pick up item if there is an item to pick up
    void GrabItem() 
    {
        currentSlot = slot2DArray[GetGridPosition(Input.mousePosition).x, GetGridPosition(Input.mousePosition).y];
        Debug.Log(GetGridPosition(Input.mousePosition));

        if (currentSlot.item != null && currentSlot.Count > 0)
        {
            heldItem = currentSlot.item;
            heldCount = currentSlot.Count;



            Debug.Log("Picked up " + heldItem.name);
        }
    }

    // check to see if the item has space to be placed using a nested for loop
    bool FitTest(Item item, int originX, int originY)
    {
        

        for(int i = 0; i < item.width; i++)
        {
            for (int j = 0; j < item.height; j++) 
            {
                int widthCheck = originX + i;

                int heightCheck = originY + j;

                if(widthCheck >= columns ||  heightCheck >= rows)
                {
                    Debug.Log("AAAAAA OUT OF BOUNDS WHAAAAAAAA");
                    return false;
                }

                if (slot2DArray[widthCheck, heightCheck].item != null)
                {
                    Debug.Log("ITEM IN THE WAY AAAAAAAA");
                    return false;
                }
                            
            }
        }

        return true;
    }

}
