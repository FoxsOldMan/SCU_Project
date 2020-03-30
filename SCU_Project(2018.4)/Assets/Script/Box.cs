using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, Interactable
{
    public GameObject Player;
    public int Capacity = 9;
    private Backpack playerBackpack;
    public GameObject pack;
    private GameObject packInstance;
    

    private bool isOpen = false;
    private Animator box;

    public ItemData[] itemDatas;
    ItemContainer[] Slots;
    Sprite[] itemimages;

    // Start is called before the first frame update
    void Start()
    {
        playerBackpack = Player.GetComponent<Backpack>();
        box = GetComponent<Animator>();
        pack = (GameObject)Resources.Load("Prefab/Pack/OtherPack");
        itemimages = Resources.LoadAll<Sprite>("ItemImages/Items");

        itemDatas = new ItemData[Capacity];
        Slots = new ItemContainer[Capacity];


    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen && packInstance != null)
        {
            ShowItemDatas();
        }
    }

    public void Interaction()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            box.SetBool("IsOpen", true);
        }
        else
        {
            Close();
            box.SetBool("IsOpen", false);
        }
    }

    public void Open()
    {
        isOpen = true;
        if (packInstance != null)
        {
            Destroy(packInstance);
        }
        packInstance = Instantiate(pack, GameObject.Find("PackUI").transform);
        for (int i = 0; i < Capacity; i++)
        {
            Slots[i] = packInstance.transform.GetChild(2).GetChild(i).GetChild(0).GetComponent<ItemContainer>();
        }

        playerBackpack.Open();
    }

    public void Close()
    {
        isOpen = false;
        if (packInstance != null)
        {
            Destroy(packInstance);
        }
        playerBackpack.Close();
    }

    private void ShowItemDatas()
    {
        for (int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[i] != null && itemDatas[i].quantity <= 0)
            {
                itemDatas[i] = null;
            }

            Slots[i].SetItemDatas(itemDatas);
        }
    }
}
