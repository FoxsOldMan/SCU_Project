using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler/*, IDropHandler*/
{
    public ItemData[] itemDatas;

    private int indexofslot;
    private Transform myParent;

    private void Start()
    {
        SetItemDatas(null);
        indexofslot = transform.parent.GetSiblingIndex();
        myParent = transform.parent;
    }
    public void SetItemDatas(ItemData[] itemDatas)
    {
        this.itemDatas = itemDatas;

        if (itemDatas!=null && itemDatas[indexofslot]!=null)
        {
            gameObject.name = itemDatas[indexofslot].item.Name;
            Image image = gameObject.GetComponent<Image>();
            image.sprite = itemDatas[indexofslot].sprite;
            image.color = new Color(255, 255, 255, 255);
            Text text = transform.GetChild(0).GetComponent<Text>();
            text.text = itemDatas[indexofslot].quantity.ToString();
            text.enabled = true;
        }
        else
        {
            gameObject.name = "empty"+indexofslot;
            Image image = gameObject.GetComponent<Image>();
            image.color = new Color(255, 255, 255, 0);
            Text text = transform.GetChild(0).GetComponent<Text>();
            text.enabled = false;
        }
    }

    public bool SwapOrMergeItemData(ItemContainer other)
    {
        if(itemDatas[indexofslot] != null && other.itemDatas != null)
        {
            if (other.name.Equals(gameObject.name))
            {
                other.itemDatas[other.indexofslot].quantity += itemDatas[indexofslot].quantity;
                itemDatas[indexofslot] = null;
                //Debug.Log("物品重复，合并");
                return true;
            }
            ItemData temp = itemDatas[indexofslot];
            itemDatas[indexofslot] = other.itemDatas[other.indexofslot];
            other.itemDatas[other.indexofslot] = temp;
            //Debug.Log("物品成功交换位置");
            return true;

        }
        Debug.Log("ItemContainer未获取ItemDatas");
        return false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemDatas[indexofslot] != null)
        {
            Transform root = myParent;
            while(root.parent != null)
            {
                root = root.parent;
            }
            this.transform.SetParent(myParent.parent.parent.parent);
            this.transform.position = eventData.position;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemDatas[indexofslot] != null)
        {
            this.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        this.transform.SetParent(myParent);
        this.transform.position = myParent.position;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        Transform root = transform;
        GraphicRaycaster gr = null;
        while (root.parent != null)
        {
            gr = root.GetComponent<GraphicRaycaster>();
            if (gr != null)
            {
                break;
            }
            root = root.parent;
        }
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        //if (results.Count != 0)
        //{
        //    foreach(RaycastResult rs in results)
        //    {
        //        Debug.Log(rs.gameObject.name);
        //    }
        //}
        //else
        //{
        //    Debug.Log("没有物体");

        //}
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if(results.Count != 0)
        {
            ItemContainer itemContainer = results[0].gameObject.GetComponent<ItemContainer>();
            if (itemContainer != null)
                this.SwapOrMergeItemData(itemContainer);
        }


    }

}
