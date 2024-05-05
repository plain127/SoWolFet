using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ChatClientStorageObject
{
    [Serializable]
    public class ChatData
    {
        public int id; //pet : 0, user : 1
        public string message;
        public string date;
        public string time;
    }

    [Serializable]
    public class ChatDataList
    {
        public List<ChatData> chatList = new List<ChatData>();
    }
}