using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class ItemDatabase: MonoBehaviour
{
    static Dictionary<int, Item> itemDict;
    static Dictionary<string, int> nameDict;

    static JsonData itemData;

    static Sprite[] itemImages;

    static ItemDatabase()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/Script/ItemRelated/items.json"));
        itemDict = new Dictionary<int, Item>();
        nameDict = new Dictionary<string, int>();

        itemImages = Resources.LoadAll<Sprite>("ItemImages/Items");

        constructItemDatabase();
    }

    private static void constructItemDatabase()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            Item item = new Item();
            item.ID = (int)itemData[i]["id"];
            item.Name = itemData[i]["name"].ToString();

            switch ((int)itemData[i]["type"])
            {
                case 0:
                    item.Type = ItemType.ToEnemy;
                    break;
                case 1:
                    item.Type = ItemType.ToSelf;
                    break;
                default:
                    item.Type = ItemType.Spetial;
                    break;
            }

            item.Value = (int)itemData[i]["value"];
            item.Image = itemData[i]["image"].ToString();
            item.Desc = itemData[i]["description"].ToString();

            itemDict.Add(item.ID, item);
            nameDict.Add(item.Name, item.ID);
            //Debug.LogFormat("ID:{0} Name:{1}", item.ID, item.Name);
        }
    }

    public static Item GetItemByID(int id)
    {
        if (itemDict.ContainsKey(id))
        {
            return itemDict[id];
        }
        else
            return null;
    }

    public static int GetIDByName(string name)
    {
        if (nameDict.ContainsKey(name)){
            return nameDict[name];
        }
        else
            return -1;
    }

    public static Sprite GetImageByID(int id)
    {
        if(itemImages != null)
        {
            if(0 <= id && id < itemImages.Length)
            {
                return itemImages[id];
            }
        }
        return null;
    }
}

public enum ItemType
{
    ToEnemy,
    ToSelf,
    Spetial,
}

public class Item
{
    private int id;
    private string name;
    private ItemType type;
    private int value; 
    private string image;
    private string desc;

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public ItemType Type { get => type; set => type = value; }
    public int Value { get => value; set => this.value = value; }
    public string Image { get => image; set => image = value; }
    public string Desc { get => desc; set => desc = value; }


    public Item()
    {
        ID = -1;
    }

    public Item(int id, string name, ItemType type, int value, string image, string desc)
    {
        ID = id;
        Name = name;
        Type = type;
        Value = value;
        Image = image;
        Desc = desc;
    }

    public void Use(PlayerStateAndMovement Player)
    {
        switch (Name)
        {
            case "bloodvial":
                if (Player.HP + value > Player.MaxHP)
                    Player.HP = Player.MaxHP;
                else
                    Player.HP += value;
                break;
            case "tranquilizer":
                if (Player.SAM - value < 0)
                    Player.SAM = 0;
                else
                    Player.SAM -= value;
                break;
            default:
                Debug.Log(Desc);
                break;
        }
    }
}

public class ItemData
{
    public Item item;
    public Sprite sprite;
    //public int indexOfSlot;
    public int quantity;

    public ItemData()
    {

    }

    public ItemData(Item item, Sprite sprite, int quantity)
    {
        this.item = item;
        this.sprite = sprite;
        this.quantity = quantity;
    }
}
