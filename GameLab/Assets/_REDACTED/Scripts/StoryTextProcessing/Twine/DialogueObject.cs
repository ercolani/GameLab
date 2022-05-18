using System;
using System.Collections.Generic;
using UnityEngine;


public class DialogueObject : MonoBehaviour
{
    private const string kStart = "START";
    private const string kEnd = "END";

    public struct Response
    {
        public string displayText;
        public string destinationNode;

        public Response(string display, string destination)
        {
            displayText = display;
            destinationNode = destination;
        }
    }

    public class Node
    {
        public string title;
        public string text;
        public List<string> tags;
        public List<Response> responses;

        internal bool IsEndNode()
        {
            return tags.Contains(kEnd);
        }

    }

    public class Dialogue
    {
        private string title;
        private Dictionary<string, Node> nodes;
        private string titleOfStartNode;

        public List<string> FindAllStartNodes()
        {
            List<string> startNodes = new List<string>();
            foreach (KeyValuePair<string, Node> node in nodes)
            {
                if (node.Value.tags.Contains("START"))
                {
                    startNodes.Add(node.Key);
                }
            }
            return startNodes;
        }

        public Dialogue(TextAsset twineText)
        {
            nodes = new Dictionary<string, Node>();
            ParseTwineText(twineText.text);
        }

        public Node GetNode(string nodeTitle)
        {
            Debug.Log(nodeTitle);
            return nodes[nodeTitle];
        }

        public Node GetStartNode()
        {
            UnityEngine.Assertions.Assert.IsNotNull(titleOfStartNode);
            return nodes[titleOfStartNode];
        }

        public void ParseTwineText(string twineText)
        {
            string[] nodeData = twineText.Split(new string[] { "::" }, StringSplitOptions.None);
            bool passedHeader = false;
            //const int kIndexOfContentStart = 4;
            for (int i = 0; i < nodeData.Length; i++)
            {
                //the first node comes after the UserStylesheet node
                if (!passedHeader)
                {
                    if (nodeData[i].StartsWith(" UserStylesheet"))
                    {
                        passedHeader = true;
                    }
                    continue;
                }

                //Note: tags are optional
                //Normal Format: "NodeTitle [Tags, comma, separated] \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]
                // No-Tag Format: "NodeTitle \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]"
                string currentLineText = nodeData[i];

                //remove position data
                int posBegin = currentLineText.IndexOf("{\"position");
                if (posBegin != -1)
                {
                    int posEnd = currentLineText.IndexOf("}", posBegin);
                    currentLineText = currentLineText.Substring(0, posBegin) + currentLineText.Substring(posEnd + 1); //title + message
                }

                bool tagsPresent = currentLineText.IndexOf("[") < currentLineText.IndexOf("\r\n");
                int endOfFirstLine = currentLineText.IndexOf("\r\n");

                int startOfResponses = -1;
                int startOfResponseDestinations = currentLineText.IndexOf("[[");
                bool lastNode = startOfResponseDestinations == -1;
                if (lastNode)
                {
                    startOfResponses = currentLineText.Length;
                }
                else
                {
                    //last new line before "[["
                    startOfResponses = currentLineText.Substring(0, startOfResponseDestinations).LastIndexOf("\r\n");
                }

                //extract title
                int titleStart = 0;
                int titleEnd = tagsPresent ? currentLineText.IndexOf("[") : endOfFirstLine;
                UnityEngine.Assertions.Assert.IsTrue(titleEnd > 0, "Maybe you have a node with no responses?");
                string title = currentLineText.Substring(titleStart, titleEnd).Trim();

                //extract tags (if any)
                string tags = tagsPresent ? currentLineText.Substring(titleEnd + 1, (endOfFirstLine - titleEnd) - 2) : "";

                if (!string.IsNullOrEmpty(tags) && tags[tags.Length - 1] == ']')
                {
                    tags = tags.Substring(0, tags.Length - 1);
                }

                //extract message & responses
                string messageText = currentLineText.Substring(endOfFirstLine, startOfResponses - endOfFirstLine).Trim();
                string responseText = currentLineText.Substring(startOfResponses).Trim();

                Node currentNode = new Node();
                currentNode.title = title;
                currentNode.text = messageText;
                currentNode.tags = new List<string>(tags.Split(new string[] { " " }, StringSplitOptions.None));

                if (currentNode.tags.Contains(kStart))
                {
                    titleOfStartNode = currentNode.title;
                }

                // Note: response messages are optional (if no message then destination is the message)
                // With Message Format: "\r\n Message[[Response One]]"
                // Message-less Format: "\r\n [[Response One]]"

                currentNode.responses = new List<Response>();
                if (!lastNode)
                {
                    List<string> responseData = new List<string>(responseText.Split(new string[] { "\r\n" }, StringSplitOptions.None));
                    for (int k = responseData.Count - 1; k >= 0; k--)
                    {
                        string currentResponseData = responseData[k];
                        if (string.IsNullOrEmpty(currentResponseData))
                        {
                            responseData.RemoveAt(k);
                            continue;
                        }

                        Response currentResponse = new Response();
                        int destinationStart = currentResponseData.IndexOf("[[");
                        int destinationEnd = currentResponseData.IndexOf("]]");
                        UnityEngine.Assertions.Assert.IsFalse(destinationStart == -1, "No destination around in node titled '" + currentNode.title + "'");
                        UnityEngine.Assertions.Assert.IsFalse(destinationEnd == -1, "No destination around in node titled '" + currentNode.title + "'");
                        string destination = currentResponseData.Substring(destinationStart + 2, (destinationEnd - destinationStart) - 2);
                        currentResponse.destinationNode = destination;
                        if (destinationStart == 0)
                        {
                            currentResponse.displayText = ""; //if message-less, then message is an empty string
                        }
                        else
                        {
                            currentResponse.displayText = currentResponseData.Substring(0, destinationStart);
                        }
                        currentNode.responses.Add(currentResponse);
                    }
                }
                nodes[currentNode.title] = currentNode;
            }
        }
    }


}

