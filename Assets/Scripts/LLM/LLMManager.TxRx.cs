using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public partial class LLMManager
{
    //Send 처리구간
    void SendChat()
    {
        apiKey = apiManager.GetApiKey(0); //APIkey가 늘어나면 반복문처리 필요
        if (apiKey != null)
        {
            sendInput = chatInput.text;
            SaveChatData(1, sendInput);
            LoadChatData();
            LoadChatInterface();

            if (sendInput.Trim() != "")
            {
                StartCoroutine(SendGptRequest());
                chatInput.text = "";
            }
        }
    }

    //Receive 처리구간
    void ReceiveChat(string msg)
    {
        SaveChatData(0, msg);
        LoadChatData();
        LoadChatInterface();
    }
}
