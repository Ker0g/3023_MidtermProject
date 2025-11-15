using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Holds reference and count of items, manages their visibility in the Inventory panel
public class ItemSlot : MonoBehaviour
{
    public Item item = null;

    public int tileSize = 32;

    public int adjustX;
    public int adjustY;

    [SerializeField]
    private int count = 0;
    public int Count
    {
        get { return count; }
        set
        {
            count = value;
            UpdateGraphic();
        }
    }

    [SerializeField]
    Image itemIcon;

    [SerializeField]
    TextMeshProUGUI itemCountText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateGraphic();
    }

    //Change Icon and count
    public void UpdateGraphic()
    {
        if (count < 1 || item == null)
        {
            count = 0;
            item = null;
            RectTransform rt = itemIcon.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(32, 32);
            itemIcon.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
        else
        {
            //change icon size and position based on item grid space
            RectTransform rt = itemIcon.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(item.width * tileSize, item.height * tileSize);

            if (item.width > 1)
            {
                adjustX = tileSize * item.width / 4;
                
            }
            if (item.height > 1) 
            {
                adjustY = tileSize * item.height / 4;
            
            }
            rt.position = new Vector2(rt.position.x + adjustX, rt.position.y - adjustY);

            //set sprite to the one from the item
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = count.ToString();
        }
    }

    private bool CanUseItem()
    {
        return (item != null && count > 0);
    }


}
