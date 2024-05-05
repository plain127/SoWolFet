using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace PromptManager
{
    [Serializable]
    public class Prompt
    {
        public List<string> prompt;
    }

    public class PromptLoad
    {
        private string prompt = "";

        private void LoadPrompt()
        {
            TextAsset promptPath = Resources.Load<TextAsset>("prompt");

            if (promptPath != null)
            {
                Prompt promptContainer = JsonUtility.FromJson<Prompt>(promptPath.text);

                foreach (string p in promptContainer.prompt)
                {
                    prompt += p;
                }
            }
        }

        public string GetPrompt()
        {
            LoadPrompt();
            return prompt;
        }
    }
}
