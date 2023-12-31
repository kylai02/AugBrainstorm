using UnityEngine;
using System;
using System.Collections.Generic;

public class Prompts : MonoBehaviour
{
    public static Prompts instance;

    void Awake()
    {
        instance ??= this;
    }

    /// <summary>-------------------------------------------------- </summary> 
    /// <summary>----------   Prompt to generate ideas   ---------- </summary> 
    /// <summary>-------------------------------------------------- </summary> 

    public string GenerateIdeasPrompt(   // form prompt for idea generation
        List<string> selectedKeywords,   // selected keywords
        int requestIdeaNumber,   // how many ideas to generate
        int requestDescriptionLen  // the length limit of each description
    )
    {
        // int ideaNumber = 3;  // For test
        // int descriptionLen = 3; // For test

        string request = "";    // prompt for ChatGPT

        // request +=

        // "Please help me brainstorm creative ideas through the keyword list I provide." + "\n\n" +
        // "The keywords in the list, seperated by comma, represent terms you need to reference. The generated ideas should be related to all the keywords in this list, but the degree of relevance of these ideas to each keyword can be randomly determined by you to generate several different concepts." + "\n\n" +
        // "Ideas can be as divergent as possible, with lower inter-idea correlations being preferable." + "\n\n" +
        // "Here are the output format requirements. Please strictly adhere to them, and any output that deviates from these guidelines is not allowed:" + "\n" +
        // "1. Each Idea's output consists of two parts: idea name and description. Idea name is the name of the idea, and description is a brief description of up to " + requestDescriptionLen + " words." + "\n" +
        // "2. Use a single line break to separate the idea name and description for each idea." + "\n" +
        // "3. Only provide the name and description for each idea; do not include any additional words or phrases. There is no need to reply with 'Certainly' or 'Sure,' etc." + "\n\n" +
        // "Here's an example, If you've come up with two ideas, 'Band rhythm game' and 'Guitar solo festival,' with their respective descriptions being 'A simple music rhythm game where gamers can act as a band member' and 'An event where guitarists can meet each other,' you should output them in the following format:" + "\n" +
        // "Band rhythm game" + "\n" +
        // "A simple music rhythm game where gamers can act as a band member" + "\n" +
        // "Guitar solo festival" + "\n" +
        // "An event where guitarists can meet each other" + "\n\n" +
        // "After generating, please review your response content and remove the beginnings such as 'Idea1:', 'Idea2:', 'Idea3:', 'name:', 'description:'and so on." + "\n\n" +
        // "Based on the description above, please come up with " + requestIdeaNumber + " ideas related to all the keywords. Here is the keyword list I provide:" + "\n";

        // for(int i = 0 ; i < selectedKeywords.Count ; i++)
        // {
        //     request += selectedKeywords[i];
        //     if(i != selectedKeywords.Count - 1){
        //         request += ", ";
        //     }
        // }

        request += "Please help me brainstorm creative ideas through the keyword list I provide.\n\n" +
                   "The keywords in the list, separated by comma, represent terms you need to reference. " +
                   "The generated ideas should be related to all the keywords in this list, but the degree of relevance " +
                   "of these ideas to each keyword can be randomly determined by you to generate several different concepts.\n\n" +
                   "Ideas can be as divergent as possible, with lower inter-idea correlations being preferable.\n\n" +
                   "Each Idea should be a short name then followed by a newline. On the next line, please provide a description " +
                   "of up to " + requestDescriptionLen + " words.\n\n" +
                   "Provide me with the ideas generated above in a format that ideas are separated by a newline, not a comma, " +
                   "and without any numbered list or bullet point. " +
                   "Do not include any additional sentences or punctuation marks in your response. No need to provide the original " +
                   "path; just the words are sufficient. No need to reply with 'Understood' or provide reasons.\n\n" +
                   "Please do not present the output in the form of a numbered or bulleted list. This is important.\n\n" +
                   "Please also delete all the \"\" and - marks from your response.\n\n" +
                   "Based on the description above, please come up with " + requestIdeaNumber + " ideas related to all the keywords. " +
                   "Here is the keyword list I provide:\n";

        for (int i = 0; i < selectedKeywords.Count; i++)
        {
            request += selectedKeywords[i];
            if (i != selectedKeywords.Count - 1)
            {
                request += ", ";
            }
        }

        // Debug.Log("Request: \n" + request + "\n");

        return request;

    }

    /// <summary>--------------------------------------------------------- </summary> 
    /// <summary>----------   Prompt to generate new keywords   ---------- </summary> 
    /// <summary>--------------------------------------------------------- </summary>     

    public string GenerateKeywordsPrompt(   // form prompt for keyword generation
        List<string> preKeywords,   // former path of keywords (include the current one)
        List<string> conditions,    // conditions from user
        int requestKeywordNumber   // how many new keywords to generate
    )
    {
        int times = 10;    // The importance multiplier between the current node and the previous node

        int testKeywordNumber = 16;  // For test

        string bannedKeywords = ""; // Keywords to ban (preKeywords + conditions)
        string path20 = "";   // 20% of thought path (1st keyword)
        string path40 = "";   // 40% of thought path (2nd keyword)
        string path60 = "";   // 60% of thought path (3rd keyword)
        string path80 = "";   // 80% of thought path (4th、5th keywords)
        string path100 = "";   // 100% of thought path (6th、7th、8th keywords)
        string conditionsStr = ""; // string of the conditions
        string request = "";    // prompt for ChatGPT

        int len = preKeywords.Count;    // the length of preKeywords
        int len20 = (int)Math.Ceiling(len * 0.2);
        int len40 = (int)Math.Ceiling(len * 0.4);
        int len60 = (int)Math.Ceiling(len * 0.6);
        int len80 = (int)Math.Ceiling(len * 0.8);
        int condLen = conditions.Count; // the length of conditions

        // forming banned keywords, thought path and conditions string

        // forming the thought path + banned keywords
        for (int i = 0; i < len; i++)
        {
            bannedKeywords += preKeywords[i];
            path100 += preKeywords[i];

            if (i == len20 - 1)
            {
                path20 = path100;
            }
            if (i == len40 - 1)
            {
                path40 = path100;
            }
            if (i == len60 - 1)
            {
                path60 = path100;
            }
            if (i == len80 - 1)
            {
                path80 = path100;
            }

            if (i != len - 1)
            {
                path100 += " -> ";
            }

            bannedKeywords += ", ";
        }

        // forming the conditions string + banned keywords
        for (int i = 0; i < condLen; i++)
        {
            bannedKeywords += conditions[i];
            conditionsStr += conditions[i];

            if (i != condLen - 1)
            {
                bannedKeywords += ", ";
                conditionsStr += ", ";
            }
        }

        request +=

        "Banned keywords: " + bannedKeywords + "\n" +
        "Thought path trees:" + "\n" +
        "1: " + path20 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "2: " + path20 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "3: " + path40 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "4: " + path40 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "5: " + path60 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "6: " + path60 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "7: " + path60 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "8: " + path80 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "9: " + path80 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "10: " + path80 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "11: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "12: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "13: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "14: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "15: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "16: " + path100 + " (Conditions: "+ conditionsStr + ")" + "\n" +
        "\n" +
        "I would like to conduct an English keyword brainstorming. Please use the provided " + testKeywordNumber + " thought path tree to generate 1 keywords each, which is related to the cooresponding thought path tree. Those " + testKeywordNumber + " generated words should be distinct and must not be the same as any provided Banned keywords. Notice that the " + testKeywordNumber + " generated keywords should be under the conditions, provided after each thought path tree." + "\n" +
        "\n" +
        "The further along the thought path tree a node is, the stronger the relevance should be to the generated keywords. The relevance of each node with the new keyword must be " + times + " times that of the parent node." + "\n" +
        "\n" +
        "Within a node in the thought path tree, there may be multiple words separated by commas. The words within the same node need to have the same level of relevance to the generated new keywords." + "\n" +
        "\n" +
        "If there is only one word in the path, it means that only that word is under consideration. Please generate new keywords based on that word as well." + "\n" +
        "\n" +
        "Please note that for each thought path tree and condition, generate only one new word. Do not generate two words separated by a comma." + "\n" +
        "\n" +
        "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. Do not include any additional sentences in your response. No need to provide the original path; just the words are sufficient. No need to reply with 'Understood' or provide reasons.";

        // request +=

        // "Banned keywords: " + bannedKeywords + "\n" +
        // "Thought path trees:" + "\n" +
        // "1: " + path20 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "2: " + path40 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "3: " + path60 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "4: " + path80 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "5: " + path80 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "6: " + path100 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "7: " + path100 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "8: " + path100 + " (Conditions: " + conditionsStr + ")" + "\n" +
        // "\n" +
        // "I would like to conduct an English keyword brainstorming. Please use the provided " + requestKeywordNumber + " thought path tree to generate 1 keywords each, which is related to the cooresponding thought path tree. Those " + requestKeywordNumber + " generated words should be distinct and must not be the same as any provided Banned keywords. Notice that the " + requestKeywordNumber + " generated keywords should be under the conditions, provided after each thought path tree." + "\n" +
        // "\n" +
        // "The further along the thought path tree a node is, the stronger the relevance should be to the generated keywords. The relevance of each node with the new keyword must be " + times + " times that of the parent node." + "\n" +
        // "\n" +
        // "Within a node in the thought path tree, there may be multiple words separated by commas. The words within the same node need to have the same level of relevance to the generated new keywords." + "\n" +
        // "\n" +
        // "If there is only one word in the path, it means that only that word is under consideration. Please generate new keywords based on that word as well." + "\n" +
        // "\n" +
        // "Please note that for each thought path tree and condition, generate only one new word. Do not generate two words separated by a comma." + "\n" +
        // "\n" +
        // "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. Do not include any additional sentences in your response. No need to provide the original path; just the words are sufficient. No need to reply with 'Understood' or provide reasons.";

        Debug.Log("Request: \n" + request + "\n");

        return request;

    }

    /// <summary>---------------------------------------------------- </summary> 
    /// <summary>----------   Prompt to merge conditions   ---------- </summary> 
    /// <summary>---------------------------------------------------- </summary>     

    public string MergeConditionsPrompt(   // Merge multiple conditions into one word
        List<string> conditions  // Conditions from user
    )
    {
        string conditionsStr = ""; // string of the conditions
        string request = "";    // prompt for ChatGPT

        int len = conditions.Count;    // the length of preKeywords

        // forming conditions string
        for (int i = 0; i < len; i++)
        {
            conditionsStr += conditions[i];

            if (i != len - 1)
            {
                conditionsStr += ", ";
            }
        }

        request +=

        "Keyword list: " + conditionsStr + "\n" +
        "\n" +
        "I would like to conduct an English keyword brainstorming. Please use the provided keyword list to generate 1 word, which is related to all keywords in the keyword list." + "\n" +
        "Please provide only the keywords brainstormed based on the above conditions. Do not include any additional phrases, and avoid any introductions.";

        Debug.Log("Request: \n" + request + "\n");

        return request;
    }
}