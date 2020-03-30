using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    private Queue<string> dialogCache = new Queue<string>();
    private Canvas dialogUI;
    private Text dialog;

    // Start is called before the first frame update
    void Start()
    {
        dialogUI = GetComponent<Canvas>();
        dialogUI.enabled = false;
        dialog = transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    public void AddMassage(string input)
    {
        dialogCache.Enqueue(input);
    }

    public void AddMassage(IEnumerable<string> list)
    {
        foreach(string massage in list)
        {
            dialogCache.Enqueue(massage);
        }
    }

    public void ShowAllMassages()
    {
        StartCoroutine(ShowDialog());
    }

    private IEnumerator ShowDialog()
    {
        dialogUI.enabled = true;
        Time.timeScale = 0;

        while (dialogCache.Count > 0)
        {
            //Debug.Log(dialogCache.Count);
            dialog.text = dialogCache.Dequeue();

            yield return new WaitUntil(CheckKeyDown);

            yield return new WaitForEndOfFrame();

        }
        Debug.Log("对话完毕");

        dialogUI.enabled = false;
        Time.timeScale = 1;
        yield break;
    }

    private bool CheckKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.N))
            return true;
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            dialogCache.Clear();
            return true;
        }
        else
            return false;
    }

}
