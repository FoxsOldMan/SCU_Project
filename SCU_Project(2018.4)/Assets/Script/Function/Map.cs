using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
        if(image != null)
        {
            image.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            image.enabled = !image.enabled;
        }
    }
}
