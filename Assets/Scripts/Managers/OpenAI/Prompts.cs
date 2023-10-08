using UnityEngine;

public class Prompts : MonoBehaviour
{
    public static Prompts instance;

    void Awake()
    {
        instance ??= this;
    }

    public string GenerateKeywordsPrompt(   // new function
        List<string> preKeywords,   // former path of keywords (include the current one)
        int requestKeywordNumber,   // how many new keywords to generate
    )
    {
        string request = "";    // prompt for ChatGPT
        string path = "";   // thought path

        request +=

        "I would like to conduct an English keyword brainstorming. Please use the provided thought path tree to generate " + requestKeywordNumber + " keywords related to this path. The further along the path tree a word is, the stronger the relevance should be to the " + requestKeywordNumber + " generated keywords." + "\n" +
        "The term 'keywords' can also include names of people, places, proper nouns, and so on, without any specific restrictions. The new keywords generated doesn't have to be synonyms. Could be related in any way." + "\n" +
        "The direction of nodes in the path tree is represented by ->, with the first word in the path being the root node and the last word being the current leaf node." + "\n" +
        "Please do not alter or extend this path but generate " + requestKeywordNumber + " possible new leaf nodes from the existing leaf nodes of this path tree." + "\n" +
        "If there is only one word in the path, it means that only that word is under consideration. Please generate " + requestKeywordNumber + " new keywords based on that word as well." + "\n" +
        "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. Do not include any additional sentences in your response. No need to provide the original path; just the words are sufficient. No need to reply with 'Understood' or provide reasons." + "\n" +
        "Here is my current path of thought:" + "\n";

        for(int i = 0 ; i < preKeywords.Length ; i++)   // form the thought path
        {
            path += preKeywords[i];

            if(i != preKeywords.Length - 1)
            {
                path += " -> ";
            }
        }

        request += path;

        return request;

    }
}
//   /// <summary>--------------------------------------------------------- </summary> 
//   /// <summary>-------   Extract context keywords from speech    ------- </summary> 
//   /// <summary>--------------------------------------------------------- </summary>     




//   /// <summary>------------------------------------ </summary> 
//   /// <summary>-------   Derive keywords    ------- </summary> 
//   /// <summary>------------------------------------ </summary>  
//   public string DeriveKeywordsPrompt(
//     string context, 
//     string keyword, 
//     string previousKeywordsToString, 
//     int requestKeywordNumber, 
//     string currentContextKeywords, 
//     bool keywords
//   ) {
//     string request = "";

//         request +=
        
//         "I would like to conduct an English keyword brainstorming. Please use the provided thought path tree to generate 3 keywords related to this path. The further along the path tree a word is, the stronger the relevance should be to the three generated keywords." + "\n" +

//         "The term 'keywords' can also include names of people, places, proper nouns, and so on, without any specific restrictions. The new keywords generated doesn't have to be synonyms. Could be related in any way." + "\n" +

//         // "The generated words must not overlap with the following words, nor have the same the lemma of a word, which are listed as follows: " + "\n" +
//         // currentContextKeywords + previousKeywordsToString + "\n" +

//         // "I repeat, The generated words must not overlap with the following words, nor have the same the lemma of a word, which are isted as follows: " + "\n" +
//         // currentContextKeywords + previousKeywordsToString + "\n" +

//         // "This is very important." + "\n" +
//         // "For example, if Words Do Not Overlap With contains the word talk, then words like talking, talked and talks are not allowed to be in the generated words since they all share the same lemma of a word. " + "\n" +
//         // "Generate as much as you could as long as it does not overlaps, any related words would do. ";

//         "The direction of nodes in the path tree is represented by ->, with the first word in the path being the root node and the last word being the current leaf node." + "\n" +

//         "Please do not alter or extend this path but generate three possible new leaf nodes from the existing leaf nodes of this path tree." + "\n" +

//         "If there is only one word in the path, it means that only that word is under consideration. Please generate 3 new keywords based on that word as well." + "\n" +

//         "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. Do not include any additional sentences in your response. No need to provide the original path; just the words are sufficient. No need to reply with 'Understood' or provide reasons." + "\n" +

//         "Here is my current path of thought:" + "\n" +
//         "Music -> instrument -> Guitar"; // Path of thought

//         // /// <summary>-------   Listing information if needed    ------- </summary>    

//         // request +=

//         // "Context: " + "" + context + "GazeNoter: Co-Piloted AR Note-Taking via Gaze Selection of Large LanguageModels Suggestions to Match Users’ Intentions. Note-taking is critical in speeches and discussions for not only afterward summarization and organization but also real-time question and opinion reminding in question-and-answer sessions or appropriate timing in discussions. Manually typing on smartphones for note-taking could distract users and increase the cognitive load. Some methods leverage large language models (LLM) to automatically generate summaries and highlights, but without user input or interaction, the artificial intelligence (AI) generative content may not match users’ intentions. Therefore, we propose an AI-copiloted augmented reality (AR) system, GazeNoter, to allow users to swiftly select LLM-generated suggestions via gaze on an AR headset for real-time note-taking. GazeNoter combines the diverse suggestion generation of LLM and efficient gaze selection of AR headsets to achieve user-in-the-loop real-time note-taking matching users’ intentions. We conducted a user study to verify the usability of GazeNoter and performed a case study to observe and showcase the real-time note-taking of GazeNoter in the face-to-face discussion." + "\n"
//         //  + "List of Words That Do Not Overlap With: " + "\n"
//         //  + currentContextKeywords;

//         // /// <summary>-------  Add previous keywords    ------- </summary> 
//         // if (keywords)
//         // {

//         //     request +=

//         //     ", " + previousKeywordsToString + "\n";

//         // }

//         // else
//         // {

//         //     request += "\n";

//         // }


//         // request +=

//         // "Main Keyword: " + keyword + "\n";

//         // /// <summary>-------   keywords ...    ------- </summary>   

//         // request +=

//         // "Based on the above-mentioned Context, which is also showed as follow:  " + context + "\n" +
//         // "Generate " + requestKeywordNumber + " words that are related to the Main keyword: " + keyword + ", doesn't have to be synonyms. Could be related in any way. " + "\n" +
//         // //"The generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, which are also listed as follows: " + "\n" + 
//         // //"List of Words That Do Not Overlap With: " + currentContextKeywords + previousKeywordsToString + "\n" +
//         // "The generated words must not overlap with the following words, nor have the same the lemma of a word, which are isted as follows: " + "\n" +
//         // currentContextKeywords + previousKeywordsToString + "\n" + //  previousKeywordsToString + "\n" + 
//         // "I repeat, The generated words must not overlap with the following words, nor have the same the lemma of a word, which are isted as follows: " + "\n" +
//         // currentContextKeywords + previousKeywordsToString + "\n" + //+ previousKeywordsToString
//         // "This is very important." + "\n" +
//         // "For example, if Words Do Not Overlap With contains the word talk, then words like talking, talked and talks are not allowed to be in the generated words since they all share the same lemma of a word. " + "\n" +
//         // "Generate as much as you could as long as it does not overlaps, any related words would do. ";

//         // // "Based on the above-mentioned Context, which is also showed as follow:  " + context + "\n" + 
//         // // "Generate " + requestKeywordNumber + " words that are related to the Main keyword: "  + keyword +  ", doesn't have to be synonyms. Could be related in any way. " + "\n" +
//         // // //"The generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, which are also listed as follows: " + "\n" + 
//         // // //"List of Words That Do Not Overlap With: " + currentContextKeywords + previousKeywordsToString + "\n" +
//         // // "The generated words must not overlap with the following words, nor have the same the lemma of a word, which are isted as follows: " + "\n" +
//         // // currentContextKeywords + previousKeywordsToString  + "\n" + //  previousKeywordsToString + "\n" + 
//         // // "I repeat, The generated words must not overlap with the following words, nor have the same the lemma of a word, which are isted as follows: " + "\n" +
//         // // currentContextKeywords + previousKeywordsToString  + "\n" + //+ previousKeywordsToString
//         // // "This is very important." + "\n" + 
//         // // "For example, if Words Do Not Overlap With contains the word talk, then words like talking, talked and talks are not allowed to be in the generated words since they all share the same lemma of a word. " + "\n" + 
//         // // "Generate as much as you could as long as it does not overlaps, any related words would do. ";

//         // /// <summary>-------   format    ------- </summary>   

//         // request +=

//         // "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. " + "\n";

//         return request;

//     }

//     // public string DerivePureKeywordsPrompt(string keyword, string previousKeywordsToString, string previousContext, int requestKeywordNumber, string currentContextKeywords, bool keywords)
//     // {

//     //     string request = "";

//     //     /// <summary>-------   Listing information if needed    ------- </summary>        

//     //     request +=

//     //     "Main Keyword: " + keyword + "\n";


//     //     request +=

//     //     "List of Words That Do Not Overlap With: " + currentContextKeywords;

//     //     /// <summary>-------  Add previous keywords    ------- </summary> 
//     //     if (keywords)
//     //     {

//     //         request +=

//     //         ", " + previousKeywordsToString + "\n";

//     //     }

//     //     else
//     //     {

//     //         request += "\n";

//     //     }

//     //     /// <summary>-------   context, keywords ...    ------- </summary>   

//     //     request +=

//     //     "Generate " + requestKeywordNumber + " words that are related to the Main Keyword showed in above and also showed as follows: " + "\n" +
//     //     "Main Keyword: " + keyword + "\n" +
//     //     "The generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, which are also listed as follows: " + "\n" +
//     //     "List of Words That Do Not Overlap With: " + currentContextKeywords + previousKeywordsToString + "\n" +
//     //     "I repeat, the generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, especially do not overlap with these words: " + FormRequest.instance.getShowingContextKeywords() + "\n" +
//     //     "For example, if Words Do Not Overlap With contains the word talk, then words like talking, talked and talks are not allowed to be in the generated words since they all share the same lemma of a word. " + "\n";

//     //     // "Generate " + requestKeywordNumber + " words that are related to the context of the Content showed in above and also showed as follows: "  + "\n" + 
//     //     // "Content: " + previousContext + "\n" + 
//     //     // "The generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, which are also listed as follows: " + "\n" + 
//     //     // "List of Words That Do Not Overlap With: " + currentContextKeywords + previousKeywordsToString + "\n" +
//     //     // "I repeat, the generated words must not overlap with the words in the above-mentioned List of Words That Do Not Overlap With nor have the same the lemma of a word, especially do not overlap with these words: " + FormRequest.instance.getShowingContextKeywords() + "\n" + 
//     //     // "For example, if Words Do Not Overlap With contains the word talk, then words like talking, talked and talks are not allowed to be in the generated words since they all share the same lemma of a word. " + "\n";


//     //     /// <summary>-------   format    ------- </summary>   
//     //     request +=

//     //         "Please provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. " + "\n";

//     //     // Debug.Log(request); 
//     //     return request;

//     // }

//     /// <summary>-------   Step by step version    ------- </summary>    
//     // public string DeriveKeywordsPrompt(string keyword, string previousKeywordsToString, string previousContext, int requestKeywordNumber, bool context, bool keywords) {

//     //     string request = "";

//     //     /// <summary>-------   Listing information if needed    ------- </summary>        
//     //     if (context) {

//     //         request += 

//     //         "Previous Content: "  + "\n" + previousContext + "\n"; 

//     //     }  

//     //     if (keywords) {

//     //         request +=

//     //         "Previous Related Words: " + previousKeywordsToString + "\n";  

//     //     }

//     //         request +=

//     //         "Main Keyword: "  + keyword + "\n";

//     //     /// <summary>-------   context, keywords ...    ------- </summary>   
//     //     if (context && keywords) {

//     //         request +=

//     //         "First, summarize the above-mentioned Previous Content, which is also showed as follows: " + previousContext + "\n"+ 

//     //         "Second, generate " + requestKeywordNumber + " words that are related to the Main keyword (" + keyword + ") and also related to the context of the summarization of Previous Content, the words must not overlap with the above-mentioned Previous Keywords, which are also listed as follows: " + previousKeywordsToString+ "\n";


//     //     }

//     //     else if (context) {

//     //         request +=

//     //         "First, summarize the above-mentioned Previous Content, which is also showed as follows: " + previousContext + "\n"+ 

//     //         "Second, generate " + requestKeywordNumber + " words that are related to the Main keyword (" + keyword + ") and also related to the context of the summarization of Previous Content." + "\n";


//     //     }

//     //     else if (keywords) {

//     //         request +=

//     //         "Generate " + requestKeywordNumber + " words that are related to the Main keyword (" + keyword + "), the words must not overlap with the above-mentioned Previous Keywords, which are also listed as follows: " + previousKeywordsToString + "\n";


//     //     }

//     //     else {

//     //         request +=

//     //         "Generate " + requestKeywordNumber + " words that are related to the Main keyword (" + keyword + "). " + "\n";

//     //     }

//     //     /// <summary>-------   format    ------- </summary>   
//     //     request +=

//     //         "Please provide me with the related words generated above in a format that words are separated by a newline, not a comma, and without an order number. ";


//     //     return request;

//     // }

//     /// <summary>--------------------------------------- </summary> 
//     /// <summary>-------   Generate sentences    ------- </summary> 
//     /// <summary>--------------------------------------- </summary>  

//     // private string Introduction = "You are an HCI (human-computer interaction) researcher reviewing an HCI work. Please generate sentences base on your role, but the generated sentences must not contain words similar to HCI";
//     private string Introduction = "You are an researcher reviewing an work and takin notes, including facts and questions";

//     public string GenerateSentencesPrompt(string context, string selectedKeywordsToString, string questionKeywordsToString, bool questionKeywords, bool questionSentence)
//     {

//         string request = "";
//         request += Introduction + "\n";

//         request +=
//         "Context: " + context + "\n";


//         if (questionKeywords)
//         {

//             request +=

//             "Please generate three question sentences, starting with these question words: " + questionKeywordsToString + "." + "\n" +
//             "The generated question sentences must all contain the following keywords: " + selectedKeywordsToString + "." + "\n" +
//             "The generated sentences must be clear and concise, not too long, no more than 10 words. " + "\n" +
//             "The generated sentences must be questions related to the above-mentioned Context paragraph. ";

//         }

//         else if (questionSentence)
//         {

//             request +=

//             "Please generate three question sentences, but DO NOT starting with these question words: " + "Who, What, Where, How, When, Why, What" + "." + "\n" +
//             "I repeat, DO NOT starting the question sentences with these question words, DO NOT : " + "Who, What, Where, How, When, Why, What" + "." + "\n" +
//             "The generated question sentences must all contain the following keywords or share the same lemma of word: " + selectedKeywordsToString + "." + "\n" +
//             "The generated sentences must be clear and concise, not too long, no more than 10 words. " + "\n" +
//             "The generated sentences must be questions related to the above-mentioned Context paragraph. ";

//         }

//         else
//         {

//             request +=

//             "Please generate three fact sentences, not question sentences. The generated sentences must all contain the following keywords or share the same lemma of word: " + selectedKeywordsToString + "." + "\n" +
//             "The generated sentences must be clear and concise, not too long, no more than 10 words. " + "\n" +
//             "The generated sentences could be the brief summary of the above-mentioned Context paragraph, and must contain the following keywords or share the same lemma of word: " + selectedKeywordsToString + "." + "\n" +
//             "The generated sentences must be related to the above-mentioned Context paragraph. ";

//         }

//         request +=
//         "Please provide me with the the sentences in a format that sentences are separated by a newline, not a comma, and without an order number. ";
//         return request;

//     }

//     public string formContextDKRequest(string keyword, string context)
//     {

//         string request = "";

//         request +=

//         "Based on the above-mentioned Context, which is also showed as follow:  " + "\n" + context + "\n" +

//         "Generate " + 12 + " words that are related to the Main keyword: " + keyword + ", doesn't have to be synonyms. Could be related in any way. " + "\n" +

//         "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. " + "\n";

//         return request;

//     }

//     public string formPureDKRequest(string keyword)
//     {

//         string request = "";

//         request +=

//         "Generate " + 12 + " words that are related to the Main keyword: " + keyword + ", doesn't have to be synonyms. Could be related in any way. " + "\n" +

//         "Provide me with the words generated above in a format that words are separated by a newline, not a comma, and without an order number. " + "\n";

//         return request;

//     }

}
/// <summary>-------   Derived keywords V2    ------- </summary>    
// public string PromptWithPreviousKeywordsAndContext(string keyword, string previousKeywordsToString, string previousContext, int requestKeywordNumber, bool context, bool keywords) {

//     string request = "";

//     /// <summary>-------   Listing information if needed    ------- </summary>        
//     if (context) {

//         request += 

//         "Previous Content: "  + "\n" + previousContext + "\n"; 

//     }  

//     if (keywords) {

//         request +=

//         "Previous Related Words: " + previousKeywordsToString + "\n";  

//     }

//         request +=

//         "Main Keyword: "  + keyword + "\n";


//     if (context) {

//         request +=

//         "Goal: Given the above Previous Content and Main Keyword, please generate " + requestKeywordNumber + " words that are related to the Main keyword and also related to the context of Previous Content. " + "\n";

//     }

//     else {

//         request +=

//         "Goal: Given the above Main Keyword, please generate " + requestKeywordNumber + " words that are related to the Main keyword. " + "\n";

//     }

//     if (keywords) {

//         request +=

//         "Please provide new related words that do not overlap with the following words: " + previousKeywordsToString + "\n";  

//     }

//     request +=

//         // "Please generate as much as possible. Doesn't have to be highly related. " + "\n" +      

//         "Please provide me with the related words in a format that words are separated by a newline, not a comma, and without an order number. ";     

//     return request;

// }