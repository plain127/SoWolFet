using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using LLMRRObject;
using ChatClientStorageObject;

public partial class LLMManager
{
    //채팅 데이터 클라이언트 저장
    private Stack<ChatData> loadMsgStack = new Stack<ChatData>(); //시스템 시작 시 Json파싱 역직렬화 저장 
    private Stack<ChatData> currentMsgStack = new Stack<ChatData>(); //시스템 시작 후 UI에 입력한 채팅 데이터 저장
    int count = 0;
    string jsonPath;

    void SaveChatData(int id, string msg)
    {
        ChatDataList chatDataList = new ChatDataList();

        if (File.Exists(jsonPath))
        {
            FileInfo fileInfo = new FileInfo(jsonPath);

            //기존 데이터 용량이 1MB를 넘을때, 새로운 파일 생성 => 로딩 속도 정확히 체킹 후 최적화 속도 추산필요
            if (fileInfo.Length > 1024 * 1024)
            {
                count += 1;
                //jsonPath = Path.Combine(Application.dataPath, $"Resources/chatData{count}.json");
                jsonPath = Path.Combine(Application.persistentDataPath, $"ChatContent/chatData{count}.json");
            }
            else
            {
                string json = File.ReadAllText(jsonPath);
                chatDataList = JsonUtility.FromJson<ChatDataList>(json);
            }
        }

        chatDataList.chatList.Add(new ChatData { id = id, message = msg, date = DateTime.Now.ToString("dddd. dd MMMM yyyy"), time = DateTime.Now.ToString("HH:mm") });
        string updatedJson = JsonUtility.ToJson(chatDataList, true);

        File.WriteAllText(jsonPath, updatedJson);
        Debug.Log("Data save to" + jsonPath);
    }

    void LoadChatData()
    {
        jsonPath = Path.Combine(Application.persistentDataPath, $"ChatContent/chatData{count}.json");
        //jsonPath = Path.Combine(Application.dataPath, $"Resources/chatData{count}.json");
        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);//저장할때, 분할 저장하므로 전부 읽기 모드로 해도 상관 없음
            ChatDataList loadMsgList = new ChatDataList();
            loadMsgList = JsonUtility.FromJson<ChatDataList>(json);
            loadMsgStack = new Stack<ChatData>(loadMsgList.chatList);

            //용량이 일정 이상이면 오래된 내용부터 삭제 => 추후 고려사항
        }
        else
        {
            Debug.Log("Let's start to send a message :)");
        }
    }

    void LoadChatDataTop()
    {
        if (loadMsgStack.Count > 0)
        {
            LoadChatInterfaceTop(loadMsgStack.Pop());
        }
        else
        {
            if (count > 0)
            {
                count -= 1;
                LoadChatData();
                LoadChatDataTop();
            }
            Debug.Log("No more messages");
        }
    }

    void LoadChatDataDown()
    {
        if (currentMsgStack.Count > 0)
        {
            LoadChatInterfaceDown(currentMsgStack.Pop());
        }
        else
        {

            Debug.Log("Recently message :)");
        }
    }
}
