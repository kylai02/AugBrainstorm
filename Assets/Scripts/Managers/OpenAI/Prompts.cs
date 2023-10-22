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

        request +=

        "Please help me brainstorm creative ideas through the keyword list I provide." + "\n\n" +
        "The keywords in the list, seperated by comma, represent terms you need to reference. The generated ideas should be related to all the keywords in this list, but the degree of relevance of these ideas to each keyword can be randomly determined by you to generate several different concepts." + "\n\n" +
        "Ideas can be as divergent as possible, with lower inter-idea correlations being preferable." + "\n\n" +
        "Here are the output format requirements. Please strictly adhere to them, and any output that deviates from these guidelines is not allowed:" + "\n" +
        "1. Each Idea's output consists of two parts: idea name and description. Idea name is the name of the idea, and description is a brief description of up to " + requestDescriptionLen + " words." + "\n" +
        "2. Use a single line break to separate the idea name and description for each idea, and also use a single line break to separate different ideas." + "\n" +
        "3. Only provide the name and description for each idea; do not include any additional words or phrases. There is no need to reply with 'Certainly' or 'Sure,' etc." + "\n\n" +
        "After generating, please review your response content and remove the beginnings such as 'Idea1:', 'Idea2:', 'Idea3:', 'name:', 'description:'and so on." + "\n\n" +
        "Based on the description above, please come up with " + requestIdeaNumber + " ideas related to all the keywords. Here is the keyword list I provide:" + "\n";

        for(int i = 0 ; i < selectedKeywords.Count ; i++)
        {
            request += selectedKeywords[i];
            if(i != selectedKeywords.Count - 1){
                request += ", ";
            }
        }

        Debug.Log("Request: \n" + request + "\n");

        return request;

    }

    /// <summary>--------------------------------------------------------- </summary> 
    /// <summary>----------   Prompt to generate new keywords   ---------- </summary> 
    /// <summary>--------------------------------------------------------- </summary>     

    public string GenerateKeywordsPrompt(   // form prompt for keyword generation
        List<string> preKeywords,   // former path of keywords (include the current one)
        int requestKeywordNumber   // how many new keywords to generate
    )
    {
        int times = 10;    // The importance multiplier between the current node and the previous node

        int keywordNumber = 8;  // For test

        string path20 = "";   // 20% of thought path (1st keyword)
        string path40 = "";   // 40% of thought path (2nd keyword)
        string path60 = "";   // 60% of thought path (3rd keyword)
        string path80 = "";   // 80% of thought path (4th、5th keywords)
        string path100 = "";   // 100% of thought path (6th、7th、8th keywords)
        string request = "";    // prompt for ChatGPT

        int len = preKeywords.Count;    // the length of preKeywords
        int len20 = (int)Math.Ceiling(len * 0.2);
        int len40 = (int)Math.Ceiling(len * 0.4);
        int len60 = (int)Math.Ceiling(len * 0.6);
        int len80 = (int)Math.Ceiling(len * 0.8);

        // K1 -> K2 -> K3 -> ...
        for (int i = 0; i < len; i++)   // form the thought path
        {
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
        }

        // Debug.Log("20%: " + path20);
        // Debug.Log("40%: " + path40);
        // Debug.Log("60%: " + path60);
        // Debug.Log("80%: " + path80);
        // Debug.Log("100%: " + path100);

        request +=

        "I would like to conduct an English keyword brainstorming. Please use the provided " + keywordNumber + " thought path tree to generate 1 keywords related to each path." + "\n" +
        "The further along the path tree a node is, the stronger the relevance should be to the generated keywords. The relevance of each node with the new keyword must be " + times + " times that of the parent node." + "\n" +
        "The term 'keywords' can also include names of people, places, proper nouns, and so on, without any specific restrictions. The new keywords generated doesn't have to be synonyms. Could be related in any way." + "\n" +
        "Within a node, there may be multiple words separated by commas. The words within the same node need to have the same level of relevance to the generated new keywords." + "\n" +
        "The direction of nodes in the path trees is represented by ->, with the first word in each path being the root node and the last word being the current leaf node." + "\n" +
        "Please do not alter or extend the paths but generate 1 possible new leaf nodes from the existing leaf nodes of each path trees." + "\n" +
        "If there is only one word in the path, it means that only that word is under consideration. Please generate new keywords based on that word as well." + "\n" +
        "If there are cases where multiple thought paths are the same, it is acceptable. Please also help me generate a new keyword for each thought path, while strictly adhering to the rule that all keywords must be distinct from each other." + "\n" +
        "Notice that the newly generated keywords must not be the same as any banned keywords i provided below, seperated by ->. These " + keywordNumber + " keywords should also be entirely distinct from each other." + "\n" +
        "Banned keywords: " + path100 + "\n" +
        "I repeat, the newly generated keywords must not be the same as any banned keywords i provided below, seperated by ->. These " + keywordNumber + " keywords should also be entirely distinct from each other. This is the most important limit, please strictly adhere to it" + "\n" +
        "Banned keywords: " + path100 + "\n" +
        "The thought path trees of each keyword are listed below:" + "\n" +
        "1: " + path20 + "\n" +
        "2: " + path40 + "\n" +
        "3: " + path60 + "\n" +
        "4: " + path80 + "\n" +
        "5: " + path80 + "\n" +
        "6: " + path100 + "\n" +
        "7: " + path100 + "\n" +
        "8: " + path100 + "\n" +
        "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. Do not include any additional sentences in your response. No need to provide the original path; just the words are sufficient. No need to reply with 'Understood' or provide reasons." + "\n" +
        "Please strictly adhere to the output format; any output outside of the specified format is not allowed.";

        Debug.Log("Request: \n" + request + "\n");

        return request;

    }
}