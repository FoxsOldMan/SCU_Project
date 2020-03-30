using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCStateAndMovement : MonoBehaviour, Interactable, GotHitable
{
    public DialogSystem dialog;
    public GameObject Player;
    private float distance;

    public Text massages;
    private string[] msgtext;

    public float AttentionLine = 10f;
    public bool isFriendly = true;
    
    private EnemyStateAndMovement enemyComponent;

    private List<ItemData> itemDatas;

    private void Start()
    {
        msgtext = massages.text.Split('\n');
        enemyComponent = GetComponent<EnemyStateAndMovement>();
        enemyComponent.enabled = false;

        itemDatas = new List<ItemData>();
        itemDatas.Add(new ItemData(ItemDatabase.GetItemByID(1), ItemDatabase.GetImageByID(0), 2));
        itemDatas.Add(new ItemData(ItemDatabase.GetItemByID(2), ItemDatabase.GetImageByID(1), 2));
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if(distance <= AttentionLine)
        {
            RotateTowardTarget(Player.transform.position);
        }

        if (!isFriendly)
        {
            enemyComponent.enabled = true;
            this.enabled = false;
        }

    }

    public void Interaction()
    {
        //Debug.Log("NPC Interaction");
        dialog.AddMassage(msgtext);
        dialog.ShowAllMassages();

        for (int i = 0; i < itemDatas.Count; i++)
        {
            if (Player.GetComponent<Backpack>().AddItem(itemDatas[i]))
                itemDatas.RemoveAt(i--);
        }

    }

    public void RotateTowardTarget(Vector3 target)
    {
        Vector3 difference = target - transform.position;

        transform.forward = difference;
    }

    public bool GotHit()
    {
        isFriendly = false;
        return true;
    }
}
