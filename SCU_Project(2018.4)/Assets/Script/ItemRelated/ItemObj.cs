using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObj : MonoBehaviour
{
    public string itemName;
    private Vector3 basepos = Vector3.zero;
    private float height = 1.5f;
    private float period = 8;
    private float value = 0;

    private float rotatespeed = 30f;

    private MeshCollider[] meshColliders;
    // Start is called before the first frame update
    void Start()
    {
        basepos = transform.position;
        meshColliders = GetComponentsInChildren<MeshCollider>();

        for (int i = 0; i < meshColliders.Length; i++)
        {
            meshColliders[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        value += 2 * Mathf.PI / period * Time.deltaTime;
        while (value < 0)
            value += Mathf.PI;
        while (value > Mathf.PI)
            value -= Mathf.PI;

        transform.position = basepos + new Vector3(0, height * Mathf.Sin(value), 0);
        transform.Rotate(new Vector3(0, rotatespeed * Time.deltaTime , 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.LogFormat("{0}吃到了{1}物品", other.name, itemName);
            Backpack playerbackpack = other.GetComponent<Backpack>();
            if (playerbackpack.AddItem(itemName))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.LogFormat("{0}添加失败", itemName);
            }


        }
    }


}
