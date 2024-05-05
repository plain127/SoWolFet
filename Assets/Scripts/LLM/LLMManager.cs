using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using UnityEngine.Networking;
using LLMRRObject;
using ChatClientStorageObject;
public partial class LLMManager : MonoBehaviour
{
    //API Key 연결
    public APIManager apiManager;
    private string apiKey;

    //OpenAI URL
    private string url = "https://api.openai.com/v1/chat/completions";

    //OpenAI와 HTTP통신
    IEnumerator SendGptRequest()
    {
        GptRequest gptRequest = new GptRequest();
        gptRequest.AddMessage("user", sendInput);

        string json = JsonUtility.ToJson(gptRequest, true);
        Debug.Log(apiKey);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", $"Bearer {apiKey}");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log($"Response : {www.downloadHandler.text}");
                //Json파일 파싱
                GptResponse gptResponse = JsonUtility.FromJson<GptResponse>(www.downloadHandler.text);
                if (gptResponse != null && gptResponse.choices != null)
                {
                    Debug.Log(gptResponse.choices[0].message.content);
                    ReceiveChat(gptResponse.choices[0].message.content);
                }
            }
        }
    }
}
