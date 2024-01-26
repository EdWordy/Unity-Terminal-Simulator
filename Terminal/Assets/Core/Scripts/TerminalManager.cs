using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.PackageManager.UI;
using Image = UnityEngine.UI.Image;
using static System.Net.Mime.MediaTypeNames;

namespace Terminal.Core
{
    public class TerminalManager : MonoBehaviour
    {
        /// <summary>
        /// The manager class for the Terminal system, responsible for marshalling the UI and coordinating parts.
        /// </summary>

        // vars
        // ----

        [Header("Prefabs")]
        [SerializeField] private GameObject DirectoryLine;
        [SerializeField] private GameObject ResponseLine;
        [SerializeField] private GameObject EmptyLine;
        [SerializeField] private GameObject EmptyWindow;

        [Header("Refs")]
        [SerializeField] private TMP_InputField TerminalInput;
        [SerializeField] private GameObject InputLine;
        [SerializeField] private ScrollRect ScrollRect;
        [SerializeField] private GameObject MessageList;

        [SerializeField] private Interpreter InterpreterModule;

        // methods
        // ----

        private void Awake()
        {
            // move the user input line to the end
            InputLine.transform.SetAsLastSibling();

            // clear the input field
            ClearInputField();

            // select the input field
            StartCoroutine(WaitForInputActivation());
        }

        // GUI
        // ----

        private void OnGUI()
        {
            if (TerminalInput.isFocused && TerminalInput.text != "" & Input.GetKey(KeyCode.Return))
            {
                // Store what the user types
                string userInput = TerminalInput.text;

                // instatiate the game object with a directory prefix
                AddDirectoryLine(userInput);

                // parse the input into a response list
                List<string> responseList = InterpreterModule.Interpret(userInput);

                // instatiate the response list
                foreach (string response in responseList) 
                {
                    AddDirectoryLine(response);
                }

                ScrollToBottom(responseList.Count);

                // move the user input line to the end
                InputLine.transform.SetAsLastSibling();

                // clear the input field
                ClearInputField();

                // select the input field
                StartCoroutine(WaitForInputActivation());
            }
        }

        // input processing 
        // ----

        private void ClearInputField()
        {
            TerminalInput.text = string.Empty;
        }
        public void AddDirectoryLine(string userInput)
        {
            // resizing the container such that the scroll rect behaves correctly
            Vector2 messageListSize = MessageList.GetComponent<RectTransform>().sizeDelta;
            MessageList.GetComponent<RectTransform>().sizeDelta = new Vector2(messageListSize.x, messageListSize.y + 35.0f);

            // instaniate the directory line
            GameObject message = Instantiate(DirectoryLine, MessageList.transform);

            // set the child index
            message.transform.SetSiblingIndex(MessageList.transform.childCount - 1);

            // set the text
            message.GetComponentsInChildren<TMP_Text>()[1].text = userInput;
        }

        public void AddNewLine(string userInput)
        {
            // resizing the container such that the scroll rect behaves correctly
            Vector2 messageListSize = MessageList.GetComponent<RectTransform>().sizeDelta;
            MessageList.GetComponent<RectTransform>().sizeDelta = new Vector2(messageListSize.x, messageListSize.y + 35.0f);

            // instaniate the directory line
            GameObject message = Instantiate(EmptyLine, MessageList.transform);

            // set the child index
            message.transform.SetSiblingIndex(MessageList.transform.childCount - 1);

            // set the text
            message.GetComponentsInChildren<TMP_Text>()[0].text = userInput;
        }

        // helper methods 
        // ----

        public IEnumerator WaitForInputActivation()
        {
            yield return 0;
            TerminalInput.ActivateInputField();
        }

        void ScrollToBottom(int lines)
        {
            if (lines > 4)
            {
                ScrollRect.velocity = new Vector2(0, 450);
            }
            else
            {
                ScrollRect.verticalNormalizedPosition = 0;
            }
        }
    }
}