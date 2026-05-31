using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
    public class ChatController : MonoBehaviour
    {
        public TMP_InputField ChatInputField;

        public TMP_Text ChatDisplayOutput;

        public Scrollbar ChatScrollbar;

        private void OnEnable()
        {
            ChatInputField.onSubmit.AddListener(AddToChatOutput);
        }

        private void OnDisable()
        {
            ChatInputField.onSubmit.RemoveListener(AddToChatOutput);
        }


        private void AddToChatOutput(string newText)
        {
            ChatInputField.text = string.Empty;

            var timeNow = DateTime.Now;

            var formattedInput = "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") +
                                 ":" + timeNow.Second.ToString("d2") + "</color>] " + newText;

            if (ChatDisplayOutput != null)
            {
                if (ChatDisplayOutput.text == string.Empty)
                    ChatDisplayOutput.text = formattedInput;
                else
                    ChatDisplayOutput.text += "\n" + formattedInput;
            }

            ChatInputField.ActivateInputField();

            ChatScrollbar.value = 0;
        }
    }
}