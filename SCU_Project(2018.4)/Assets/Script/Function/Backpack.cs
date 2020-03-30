using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public ItemData[] itemDatas;
    public ItemData[] handDatas;

    private bool isOpen = false;

    Canvas backpackCanvas;
    ItemContainer lefthandslot;
    ItemContainer righthandslot;
    ItemContainer[] Slots;
    // Start is called before the first frame update
    void Start()
    {
        backpackCanvas = GameObject.Find("PackUI").GetComponent<Canvas>();      

        GameObject.Find("Title_Panel").transform.GetChild(0).GetChild(0).GetComponent<Text>().text = gameObject.name+"的背包";

        GameObject slotPanel = GameObject.Find("Slot_Panel");
        itemDatas = new ItemData[slotPanel.transform.childCount];
        Slots = new ItemContainer[slotPanel.transform.childCount];
        for(int i = 0; i < slotPanel.transform.childCount; i++)
        {
            Slots[i] = slotPanel.transform.GetChild(i).GetChild(0).GetComponent<ItemContainer>();
        }
        //Debug.Log("slot个数:" + slotPanel.transform.childCount);


        lefthandslot = GameObject.Find("Hand_Panel").transform.GetChild(0).GetChild(0).GetComponent<ItemContainer>();
        righthandslot = GameObject.Find("Hand_Panel").transform.GetChild(1).GetChild(0).GetComponent<ItemContainer>();
        handDatas = new ItemData[2];
        //Debug.Log("左手右手");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            AddItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            AddItem(3);

        if (Input.GetKeyDown(KeyCode.Backspace) && itemDatas[0] != null)
            itemDatas[0].quantity = 0;

        if (Input.GetKeyDown(KeyCode.I) && backpackCanvas != null)
        {
            Switch();
        }

        if (isOpen)
        {
            ShowItemDatas();

        }

    }

    public void Switch()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    public void Open()
    {
        isOpen = true;
        backpackCanvas.enabled = true;
        Time.timeScale = 0;
    }

    public void Close()
    {
        isOpen = false;
        backpackCanvas.enabled = false;
        Time.timeScale = 1;
    }

    private void ShowItemDatas()
    {
        if (handDatas[0] != null && handDatas[0].quantity <= 0)
            handDatas[0] = null;
        if (handDatas[1] != null && handDatas[1].quantity <= 0)
            handDatas[1] = null;
        lefthandslot.SetItemDatas(handDatas);
        righthandslot.SetItemDatas(handDatas);

        for (int i = 0; i< itemDatas.Length; i++)
        {
            if(itemDatas[i] != null && itemDatas[i].quantity <= 0)
            {
                itemDatas[i] = null;
            }
            
            Slots[i].SetItemDatas(itemDatas);
        }

    }

    public bool AddItem(int item_id)
    {
        foreach(ItemData itemdata in itemDatas)
        {
            if(itemdata != null && itemdata.item.ID == item_id)
            {
                itemdata.quantity++;
                //Debug.Log("此物品已经在背包中，数量加一");
                return true;
            }
        }

        Item item = ItemDatabase.GetItemByID(item_id);
        if(item != null)
        {
            for(int i = 0; i < itemDatas.Length; i++)
            {
                if(itemDatas[i] == null || itemDatas[i].quantity <= 0)
                {
                    itemDatas[i] = new ItemData(item, ItemDatabase.GetImageByID(Convert.ToInt32(item.Image)), 1);
                    //Debug.LogFormat("添加了{0}到背包中", item.Name);
                    return true;
                }
            }
            Debug.Log("背包已经满了");
            return false;

        }

        Debug.LogFormat("没有id为{0}的物品", item_id);
        return false;     

    }

    public bool AddItem(string item_name)
    {
        int id = ItemDatabase.GetIDByName(item_name);
        if (id > -1)
        {
            if (AddItem(id))
                return true;
        }

        return false;
    }

    public bool AddItem(ItemData inputItemData)
    {
        foreach (ItemData itemdata in itemDatas)
        {
            if (itemdata != null && itemdata.item.ID == inputItemData.item.ID)
            {
                itemdata.quantity += inputItemData.quantity;   
                return true;
            }
        }

        for (int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[i] == null || itemDatas[i].quantity <= 0)
            {
                itemDatas[i] = inputItemData;
                return true;
            }
        }

        Debug.Log("背包已经满了");
        return false;
    }

}

