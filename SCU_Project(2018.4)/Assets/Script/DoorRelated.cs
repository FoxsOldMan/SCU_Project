using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRelated : MonoBehaviour, Interactable
{
    public string keyName;
    public DialogSystem dialog;
    public GameMode gameMode;
    public GameObject Player;
    public int doorPuzzleNumber = 1;

    private Collider doorCollider;

    void Start()
    {
        doorCollider = GetComponent<Collider>();
    }

    public void Interaction()
    {
        foreach(ItemData itemdate in Player.GetComponent<Backpack>().itemDatas)
        {
            if(itemdate.item.Name == keyName)
            {
                StartCoroutine(OpenDoor());
                return;
            }
        }

        string msg = "缺失" + keyName + "无法打开门锁";
        dialog.AddMassage(msg);
        dialog.ShowAllMassages();
    }

    private IEnumerator OpenDoor()
    {
        doorCollider.enabled = false;

        float offset = 20;
        while(offset > 0)
        {
            transform.position += Vector3.down * Time.deltaTime*2;
            offset -= Time.deltaTime*2;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("门锁打开");
        gameMode.puzzlesCompleteness[doorPuzzleNumber] = true;
        yield break;
    }
}
