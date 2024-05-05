using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class ApiKeys
{
    public List<string> openAiApiKeys;
}

public class APIManager : MonoBehaviour
{
    private List<string> openAiApiKeys;

    private void LoadApiKeys()
    {

        TextAsset filePath = Resources.Load<TextAsset>("apiKey");

        if (filePath != null)
        {
            ApiKeys apiKeyContainer = JsonUtility.FromJson<ApiKeys>(filePath.text);

            //OpenAI API Key 받기
            openAiApiKeys = apiKeyContainer.openAiApiKeys;

        }
    }

    public string GetApiKey(int index)
    {
        LoadApiKeys();

        if (index >= 0 && index < openAiApiKeys.Count)
        {
            return openAiApiKeys[index];
        }
        return null;
    }
}
