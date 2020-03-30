using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITest : MonoBehaviour
{
    public InputField inputField;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

    }

    public void SetRed(bool isRed)
    {
        if(isRed)
            text.color = Color.red;
        Debug.Log("red:"+isRed);
    }

    public void SetGreen(bool isGreen)
    {
        if(isGreen)
            text.color = Color.green;
        Debug.Log("green:"+isGreen);

    }

    public void SetBlue(bool isBlue)
    {
        if(isBlue)
            text.color = Color.blue;
        Debug.Log("blue:"+isBlue);
    }

    public void ValueChange()
    {
        Debug.Log("Change!!");
    }

    public void TestChange(string input)
    {
        Debug.Log(input);
    }

    public void InputFieldValueChange()
    {
        Debug.Log("ValueChange!");
    }

    public void InputFieldEndEdit()
    {
        Debug.Log("EditEnd");
    }
}
