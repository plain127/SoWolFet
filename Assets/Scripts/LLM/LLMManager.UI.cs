using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using LLMRRObject;
using ChatClientStorageObject;

public partial class LLMManager
{
    //UI
    public TMP_InputField chatInput;
    string sendInput;
    public Button sendBtn;

    public Scrollbar scrollbar; //스크롤할때마다 데이터 호출
    private bool isLoadingData = false;
    public RectTransform chatBoxParent;

    public GameObject petChatBox;
    public GameObject userChatBox;


    void Start()
    {
        sendBtn.onClick.AddListener(SendButtonClick);
        //추후 Start()말고 다른 활성화 메서드에 첨가
        LoadChatData(); //코루틴 사용
        LoadChatInterface(); //LoadChatData가 사용된 후 호출
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
    }

    void SendButtonClick() //버튼 작동 메서드
    {
        SendChat();
    }

    void OnScrollbarValueChange(float value)
    {
        //value 값 QA후 UX 반영 필요
        if (value >= 0.9f && !isLoadingData)
        {
            StartCoroutine(LoadDataWithDelay(true));
        }
        else if (value <= 0.1f && !isLoadingData)
        {
            StartCoroutine(LoadDataWithDelay(false));
        }
    }

    IEnumerator LoadDataWithDelay(bool loadTop)
    {
        isLoadingData = true;

        yield return new WaitForSeconds(0.5f);

        // 지정된 방향에 따라 데이터 로드
        if (loadTop)
        {
            LoadChatDataTop();
        }
        else
        {
            LoadChatDataDown();
        }

        isLoadingData = false;
    }

    void LoadChatInterface()
    {
        //채팅 UI화면 재부팅
        if (chatBoxParent.childCount > 0)
        {
            foreach (Transform child in chatBoxParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < 7; i++) //CPU에 최적화할 수 있는 i개수 적용
        {
            if (loadMsgStack.Count > 0)
            {
                ChatData chatData = loadMsgStack.Pop();
                if (chatData.id == 0)
                {
                    petChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
                    petChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
                    GameObject petChat = Instantiate(petChatBox, chatBoxParent);
                    petChat.transform.SetAsFirstSibling();
                }
                else if (chatData.id == 1)
                {
                    userChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
                    userChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
                    GameObject userChat = Instantiate(userChatBox, chatBoxParent);
                    userChat.transform.SetAsFirstSibling();
                }
            }
            else
            {
                if (count > 0)
                {
                    count -= 1;
                    LoadChatData();
                    LoadChatInterface(); //비동기되지 않도록 연결
                }
                break;
            }
        }
    }

    void LoadChatInterfaceTop(ChatData chatData)
    {
        if (chatData.id == 0)
        {
            petChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
            petChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
            Instantiate(petChatBox, chatBoxParent).transform.SetAsFirstSibling();
        }
        else if (chatData.id == 1)
        {
            userChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
            userChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
            Instantiate(userChatBox, chatBoxParent).transform.SetAsFirstSibling();
        }
        TopChatBoxUiToData();
    }
    void LoadChatInterfaceDown(ChatData chatData)
    {
        if (chatData.id == 0)
        {
            petChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
            petChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
            Instantiate(petChatBox, chatBoxParent).transform.SetAsLastSibling();
        }
        else if (chatData.id == 1)
        {
            userChatBox.GetComponentsInChildren<Text>()[0].text = chatData.message;
            userChatBox.GetComponentsInChildren<Text>()[1].text = chatData.time;
            Instantiate(userChatBox, chatBoxParent).transform.SetAsLastSibling();
        }
        DownChatBoxUiToData();
    }
    void TopChatBoxUiToData()
    {
        ChatData chatData = new ChatData();
        GameObject chatBox = chatBoxParent.GetChild(chatBoxParent.childCount - 1).gameObject;
        if (chatBox.name == "PetChatBox")
        {
            chatData.id = 0;
            chatData.message = chatBox.GetComponentsInChildren<Text>()[0].text;
            chatData.date = "";
            chatData.time = chatBox.GetComponentsInChildren<Text>()[1].text;
        }
        else if (chatBox.name == "UserChatBox")
        {
            chatData.id = 1;
            chatData.message = chatBox.GetComponentsInChildren<Text>()[0].text;
            chatData.date = "";
            chatData.time = chatBox.GetComponentsInChildren<Text>()[1].text;
        }
        currentMsgStack.Push(chatData);
        Destroy(chatBox);
    }

    void DownChatBoxUiToData()
    {
        ChatData chatData = new ChatData();
        GameObject chatBox = chatBoxParent.GetChild(0).gameObject;
        if (chatBox.name == "PetChatBox")
        {
            chatData.id = 0;
            chatData.message = chatBox.GetComponentsInChildren<Text>()[0].text;
            chatData.date = "";
            chatData.time = chatBox.GetComponentsInChildren<Text>()[1].text;
        }
        else if (chatBox.name == "UserChatBox")
        {
            chatData.id = 1;
            chatData.message = chatBox.GetComponentsInChildren<Text>()[0].text;
            chatData.date = "";
            chatData.time = chatBox.GetComponentsInChildren<Text>()[1].text;
        }
        loadMsgStack.Push(chatData);
        Destroy(chatBox);
    }
}
