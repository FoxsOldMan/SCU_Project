using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public PlayerAndCameraController controller;

    public GameObject sceneUI;
    private Transform sceneStart;
    private Transform sceneWin;
    private Transform sceneLose;

    [HideInInspector]
    public bool[] puzzlesCompleteness = new bool[2];

    public float startUIAppearTime = 2f;
    public float StartUIStayTime = 1f;
    public float startUIFadeTime = 2f;
    [Range(0, 255)]
    public float startUITransparency = 100f;
    [Range(0, 255)]
    public float startTextTransparency = 255f;

    private void Start()
    {
        sceneStart = sceneUI.transform.GetChild(0);
        sceneStart.gameObject.SetActive(true);
        sceneWin = sceneUI.transform.GetChild(1);
        sceneWin.gameObject.SetActive(false);
        sceneLose = sceneUI.transform.GetChild(2);
        sceneLose.gameObject.SetActive(false);

        StartCoroutine(StartUIAppear());
        StartCoroutine(CheckLose());
        StartCoroutine(CheckWin());
        
    }


    private IEnumerator StartUIAppear()
    {
        controller.isActive = false;
        while (sceneStart.GetComponent<Image>().color.a < startUITransparency/255f)
        {
            Color temp = sceneStart.GetComponent<Image>().color;
            temp.a += startUITransparency / 255f / startUIAppearTime * Time.deltaTime;

            sceneStart.GetComponent<Image>().color = temp;

            temp = sceneStart.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            temp.a += startTextTransparency / 255f / startUIAppearTime * Time.deltaTime;
            sceneStart.GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(StartUIStayTime);
        StartCoroutine(StartUIFade());

        yield break;
    }

    private IEnumerator StartUIFade()
    {
        while(sceneStart.GetComponent<Image>().color.a > 0)
        {
            //Debug.Log("Fade");
            Color temp = sceneStart.GetComponent<Image>().color;
            temp.a -= startUITransparency / 255f / startUIFadeTime * Time.deltaTime;
            sceneStart.GetComponent<Image>().color = temp;

            temp = sceneStart.GetChild(0).GetComponent<TextMeshProUGUI>().color;
            temp.a -= startTextTransparency / 255f / startUIFadeTime * Time.deltaTime;
            sceneStart.GetChild(0).GetComponent<TextMeshProUGUI>().color = temp;

            yield return new WaitForEndOfFrame();
        }

        sceneStart.gameObject.SetActive(false);
        controller.isActive = true;
        yield break;
    }

    private IEnumerator CheckWin()
    {
        yield return new WaitUntil(Win);
        Debug.Log("Win!");
        sceneWin.gameObject.SetActive(true);
    }

    private bool Win()
    {
        foreach(bool completeness in puzzlesCompleteness)
        {
            if (!completeness)
                return false;
        }

        return true;
    }

    private IEnumerator CheckLose()
    {
        yield return new WaitUntil(Lose);
        Debug.Log("Lose!");
        sceneLose.gameObject.SetActive(true);
    }

    private bool Lose()
    {
        if (!player.GetComponent<PlayerStateAndMovement>().isAlive)
            return true;

        return false;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReTryCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
