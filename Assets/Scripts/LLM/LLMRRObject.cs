using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PromptManager;

namespace LLMRRObject
{
    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class GptRequest
    {
        public string model = "gpt-3.5-turbo";
        public List<Message> messages;
        public GptRequest()
        {
            PromptLoad promptLoad = new PromptLoad();
            string prompt = promptLoad.GetPrompt();
            messages = new List<Message>
            {
                new Message { role = "system", content = prompt}//content내용 prompt 엔지니어링 파일로 따로 분리 연결
            };
        }
        public void AddMessage(string role, string content)
        {
            Message newMessage = new Message { role = role, content = content };
            messages.Add(newMessage);
        }
    }

    [Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }


    [Serializable]
    public class Choice
    {
        public int index;
        public Message message;
        public object logprobs;
        public string finish_reason;
    }

    [Serializable]
    public class GptResponse
    {
        public string id;
        public string object_type;
        public int created;
        public string model;
        public string system_fingerprint;
        public Choice[] choices;
        public Usage usage;

    }
}
