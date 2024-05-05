using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class BtnManager : MonoBehaviour
{
    public List<Button> btnList;
    public GameObject chatWithPet;
    bool startChat = false;

    void Start()
    {
        if (btnList.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
        for (int i = 0; i < btnList.Count; i++)
        {
            string name = btnList[i].name;
            switch (name)
            {
                case "MainBtn":
                    btnList[i].onClick.AddListener(GoToMain);
                    break;
                case "ARBackBtn":
                    btnList[i].onClick.AddListener(GoToARBack);
                    break;
                case "ARSelfBtn":
                    btnList[i].onClick.AddListener(GoToARSelf);
                    break;
                case "StrollBtn":
                    btnList[i].onClick.AddListener(GoToStroll);
                    break;
                case "QuitBtn":
                    btnList[i].onClick.AddListener(ApplicationEnd);
                    break;
                case "ChatStart":
                    btnList[i].onClick.AddListener(ChatStart);
                    break;
            }
        }


    }

    void GoToMain()
    {
        SceneManager.LoadScene("Main");
    }

    void ChatStart()
    {
        if (startChat == false)
        {
            chatWithPet.SetActive(true);
            startChat = true;
        }
        else
        {
            chatWithPet.SetActive(false);
            startChat = false;
        }
    }
    void GoToARBack()
    {
        SceneManager.LoadScene("ARBack");
    }

    void GoToARSelf()
    {
        SceneManager.LoadScene("ARSelf");
    }

    void GoToStroll()
    {
        SceneManager.LoadScene("Stroll");
    }

    void ApplicationEnd()
    {
        //어플종료시 선택된 캐릭터 초기화 서버, DB 구축 후 수정 예정
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
