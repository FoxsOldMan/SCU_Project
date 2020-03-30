using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    //private GameObject[] weapons;
    //private GameObject[] offhands;
    private GameObject weapon;
    private GameObject torch;
    private GameObject torchinstance;

    private Vector3 offset = new Vector3(-2.5f, 5.5f, 3.5f);

    public ItemData[] handDatas;
    void Start()
    {
        weapon = (GameObject)Resources.Load("Prefab/Equipment/stone");
        torch = (GameObject)Resources.Load("Prefab/Equipment/torch");
        handDatas = GetComponent<Backpack>().handDatas;

    }

    private void Update()
    {
        //if(torchinstance != null)
        //{
        //    torchinstance.transform.position = transform.position + offset;
        //}
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(torchinstance != null)
            {
                torchinstance.GetComponent<Torch>().Switch();
            }
            else
            {
                if (handDatas[0] != null && handDatas[0].item.Name.Equals("torch"))
                {
                    if (handDatas[0].quantity > 0)
                    {
                        torchinstance = Instantiate(torch, transform.position + transform.rotation * offset, transform.rotation);
                        torchinstance.transform.SetParent(gameObject.transform);
                        if (torchinstance.GetComponent<Torch>().LightTorch())
                        {
                            handDatas[0].quantity--;

                        }
                    }
                    else
                        Debug.Log("没火把了");

                }
                else
                {
                    Debug.Log("副手没有火把，无法点亮");
                }

            }
        }
    }

    //public GameObject GetWeapon()
    //{
    //    return weapon;
    //}

    public bool UseItem()
    {
        //switch (handDatas[1].item.Type)
        //{

        //}
        if(weapon != null)
        {
            
            GameObject stone = Instantiate(weapon,null);
            stone.transform.position = transform.position + transform.forward * 3.0f + new Vector3(0,6.4f,0);
            stone.transform.forward = transform.forward;
            return true;
        }
        Debug.LogFormat("获取预制体失败");
        return false;
    }
}
