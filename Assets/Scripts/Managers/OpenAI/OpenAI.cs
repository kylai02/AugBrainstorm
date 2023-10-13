using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OpenAI
{
  public class OpenAI : MonoBehaviour
  {
    /// <summary>-------   Related Keywords Message History    ------- </summary> 
    private List<ChatMessage> generatedKeywordsMessages = new List<ChatMessage>();
    private List<ChatMessage> contextKeywordsMessages = new List<ChatMessage>();

    /// <summary>-------   API    ------- </summary> 
    private readonly OpenAIApi openai = new();

    /// <summary>-------   Singleton stuff    ------- </summary>
    public static OpenAI instance;

    private void Awake()
    {
      instance ??= this;
    }

    /// <summary>-------------------------------------------------------------------- </summary> 
    /// <summary>-------   Generate new keywords from previous thought path   ------- </summary> 
    /// <summary>-------------------------------------------------------------------- </summary>     

    public async Task<List<string>> GetGeneratedKeywordsOpenAI(List<string> preKeywords, int requestKeywordNumber)
    {
      string OpenAIResponse = await RequestGeneratedKeywordsOpenAI(preKeywords, requestKeywordNumber);
      List<string> generatedKeywordsList = ExtractWithDifferentFormat(OpenAIResponse, 20);

      // Still wrong format or nothing is generated
      if (generatedKeywordsList == null || generatedKeywordsList.Count < 2)
      {
        Debug.Log("Regenerate Keywords");
        // Retry once
        string secondOpenAIResponse = await RequestGeneratedKeywordsOpenAI(preKeywords, requestKeywordNumber);
        List<string> secondgeneratedKeywordsList = ExtractWithDifferentFormat(secondOpenAIResponse, 20);

        return secondgeneratedKeywordsList;
      }
      else
      {
        return generatedKeywordsList;
      }
    }

    private async Task<string> RequestGeneratedKeywordsOpenAI(List<string> preKeywords, int requestKeywordNumber)
    {
      // Format of message user's gonna send
      ChatMessage newMessage = new()
      {
        Role = "user",
        Content = Prompts.instance.GenerateKeywordsPrompt(preKeywords, requestKeywordNumber)
      };

      generatedKeywordsMessages.Clear();
      generatedKeywordsMessages.Add(newMessage);

      var GeneratedKeywordsResponse =
        await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
          Model = "gpt-3.5-turbo",
          Messages = generatedKeywordsMessages,
          Temperature = 0.5f,
          MaxTokens = 50,
          // TopP=1.0f,
          FrequencyPenalty = 0.8f,
          PresencePenalty = 0.0f
        });

      // Make sure there's text generated
      if (GeneratedKeywordsResponse.Choices != null && GeneratedKeywordsResponse.Choices.Count > 0)
      {
        var message = GeneratedKeywordsResponse.Choices[0].Message;

        message.Content = message.Content.Trim(); // Trim result
        generatedKeywordsMessages.Add(message); // Append to chat history

        return message.Content;
      }
      else
      {
        return null;
      }
    }

    /// <summary>--------------------------------------------------------- </summary> 
    /// <summary>-------   Extract context keywords from speech    ------- </summary> 
    /// <summary>--------------------------------------------------------- </summary>     

    public async Task<List<string>> GetContextKeywordsOpenAI(string newInput)
    {
      string OpenAIResponse = await RequestContextKeywordsOpenAI(newInput);
      List<string> contextKeywordsList = ExtractWithDifferentFormat(OpenAIResponse, 20);

      // Still wrong format or nothing is generated
      if (contextKeywordsList == null || contextKeywordsList.Count < 2)
      {
        Debug.Log("Regenerate Context Keywords");
        // Retry once
        string secondOpenAIResponse = await RequestContextKeywordsOpenAI(newInput);
        List<string> secondcontextKeywordsList =
        ExtractWithDifferentFormat(secondOpenAIResponse, 20);

        return secondcontextKeywordsList;

      }
      else
      {
        return contextKeywordsList;
      }
    }

    private async Task<string> RequestContextKeywordsOpenAI(string newInput)
    {
      // Format of message user's gonna send
      ChatMessage newMessage = new()
      {
        Role = "user",
        Content = FormRequest.instance.FormExtractContextKeywordRequest(newInput)
      };

      // if (ControlPanel.instance.PreserveMessageHistory)
      // {

      //     contextKeywordsMessages.Add(newMessage);

      // }

      // else
      // {

      contextKeywordsMessages.Clear();
      contextKeywordsMessages.Add(newMessage);

      // }

      var ContextKeywordsResponse =
        await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
          Model = "gpt-3.5-turbo",
          Messages = contextKeywordsMessages,
          Temperature = 0.5f,
          MaxTokens = 50,
          // TopP=1.0f,
          FrequencyPenalty = 0.8f,
          PresencePenalty = 0.0f
        });

      // Make sure there's text generated
      if (ContextKeywordsResponse.Choices != null &&
        ContextKeywordsResponse.Choices.Count > 0)
      {
        // Get message
        var message = ContextKeywordsResponse.Choices[0].Message;


        message.Content = message.Content.Trim(); // Trim result
        contextKeywordsMessages.Add(message); // Append to chat histroy

        // ControlPanel.instance.PrintContextKeyword(message.Content);                                                     // Print if needed

        return message.Content;
      }
      else
      {
        return null;
      }
    }

    /// <summary>------------------------------------------- </summary> 
    /// <summary>-------   Spawn Related Keywords    ------- </summary> 
    /// <summary>------------------------------------------- </summary>

    // public async Task<string> generateAllRelatedKeywords(string keyword, List<string> previousLayerKeywords, int getWhat)
    // {


    //     if (getWhat == 1)
    //     {                                     // Only spawn DK

    //         await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //         return null;

    //     }

    //     else if (getWhat == 2)
    //     {                                // Only get CDK

    //         await GetRelatedContextKeywordsOpenAI(keyword, previousLayerKeywords);
    //         return null;

    //     }

    //     else if (getWhat == 3)
    //     {                                // Get both

    //         await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //         await GetRelatedContextKeywordsOpenAI(keyword, previousLayerKeywords);

    //         return null;

    //     }

    //     else
    //     {

    //         return null;

    //     }
    //     //if (ModeManager.instance.Mode == 1) {

    //     // if (HistoryTranscript.instance.Transcripts.Count > ControlPanel.instance.minContextSentenceToSpawnContextRelatedKeywords ){     

    //     //     await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     await GetRelatedContextKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     return null;
    //     // }

    //     // else {

    //     //     await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     return null;

    //     // }

    //     //}

    //     //else if (ModeManager.instance.Mode == 8) {

    //     // if (RefineManager.instance.transcript.Length > 20 ){     

    //     //     await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     await GetRelatedContextKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     return null;

    //     // }

    //     // else {

    //     //     await GetRelatedKeywordsOpenAI(keyword, previousLayerKeywords);
    //     //     return null;

    //     // }

    //     //}

    //     //else {

    //     //return null;

    //     //}



    // }


    // public async Task<List<string>> GetRelatedKeywordsOpenAI(string layerkeyword, List<string> previousLayerKeywords)
    // {

    //     string OpenAIResponse = await SpawnRelatedKeywordsOpenAI(layerkeyword, previousLayerKeywords);
    //     List<string> relatedKeywordsList = ExtractWithDifferentFormat(OpenAIResponse, ControlPanel.instance.MaxRelatedKeywordCharacterNumber);

    //     if (relatedKeywordsList == null || relatedKeywordsList.Count < 2)
    //     {                                                             // Still wrong format or nothing is generated

    //         Debug.Log("Regenerate Related Keywords");
    //         string secondOpenAIResponse = await SpawnRelatedKeywordsOpenAI(layerkeyword, previousLayerKeywords);                                               // Retry once
    //         List<string> secondRelatedKeywordsList = ExtractWithDifferentFormat(secondOpenAIResponse, ControlPanel.instance.MaxRelatedKeywordCharacterNumber);

    //         foreach (string word in secondRelatedKeywordsList)
    //         {

    //             RelatedKeywordHistoryDionary.instance.AddToRelatedKeyowrdDictionary(layerkeyword, word);                                // Add to related keywords history

    //         }

    //         return secondRelatedKeywordsList;

    //     }

    //     else
    //     {

    //         foreach (string word in relatedKeywordsList)
    //         {

    //             RelatedKeywordHistoryDionary.instance.AddToRelatedKeyowrdDictionary(layerkeyword, word);                                // Add to related keywords history

    //         }

    //         return relatedKeywordsList;

    //     }

    // }





    // public async Task<List<string>> GetRelatedContextKeywordsOpenAI(string layerkeyword, List<string> previousLayerKeywords)
    // {

    //     string OpenAIResponse = await SpawnRelatedContextKeywordsOpenAI(layerkeyword, previousLayerKeywords);
    //     List<string> relatedKeywordsList = ExtractWithDifferentFormat(OpenAIResponse, ControlPanel.instance.MaxRelatedKeywordCharacterNumber);

    //     if (relatedKeywordsList == null || relatedKeywordsList.Count < 2)
    //     {                                                             // Still wrong format or nothing is generated

    //         Debug.Log("Regenerate Related Context Keywords");
    //         string secondOpenAIResponse = await SpawnRelatedContextKeywordsOpenAI(layerkeyword, previousLayerKeywords);                 // Retry once
    //         List<string> secondRelatedKeywordsList = ExtractWithDifferentFormat(secondOpenAIResponse, ControlPanel.instance.MaxRelatedKeywordCharacterNumber);

    //         foreach (string word in secondRelatedKeywordsList)
    //         {

    //             RelatedKeywordsManager.instance.RelatedContextKeywordsList.Add(word);                                                   // Add to related keywords history

    //         }

    //         return secondRelatedKeywordsList;

    //     }

    //     else
    //     {

    //         foreach (string word in relatedKeywordsList)
    //         {

    //             RelatedKeywordsManager.instance.RelatedContextKeywordsList.Add(word);                                 // Add to related keywords history

    //         }

    //         return relatedKeywordsList;

    //     }

    // }

    // public void try333(string layerkeyword, List<string> previousLayerKeywords) {

    //     one1(layerkeyword, previousLayerKeywords);
    //     one2(layerkeyword, previousLayerKeywords);

    // }

    // public async Task<List<string>> one1(string layerkeyword, List<string> previousLayerKeywords) {

    //     await Task.Delay(10000);
    //     Debug.Log("first one done");
    //     return null;


    // }


    // public async Task<List<string>> one2(string layerkeyword, List<string> previousLayerKeywords) {

    //     await Task.Delay(11000);
    //     Debug.Log("second done");
    //     return null;


    // }

    // public async Task<string> SpawnRelatedKeywordsOpenAI(string keyword, List<string> previousLayerKeywords)
    // {

    //     ChatMessage newMessage = new()
    //     {                                                                                       // Format of message user's gonna send

    //         Role = "user",
    //         Content = FormRequest.instance.FormDerivedKeywordRequest(keyword, previousLayerKeywords)

    //     };

    //     if (ControlPanel.instance.PreserveMessageHistory)
    //     {                                                                         // Add request to related keywords chat message history    

    //         relatedKeywordsMessages.Add(newMessage);

    //     }

    //     else
    //     {

    //         relatedKeywordsMessages.Clear();
    //         relatedKeywordsMessages.Add(newMessage);

    //     }

    //     var spawnRelatedKeywordsResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
    //     {                    // Send request to OpenAI

    //         Model = "gpt-3.5-turbo",
    //         Messages = relatedKeywordsMessages,                                                                                     // Given whole chat history 
    //         Temperature = 0.3f,
    //         MaxTokens = 300,
    //         N = 1,
    //         // TopP=1.0f,
    //         FrequencyPenalty = 0.8f,
    //         PresencePenalty = 0.0f

    //     });

    //     if (spawnRelatedKeywordsResponse.Choices != null && spawnRelatedKeywordsResponse.Choices.Count > 0)
    //     {                       // Make sure there's text generated

    //         var message = spawnRelatedKeywordsResponse.Choices[0].Message;                                                          // Get message
    //         message.Content = message.Content.Trim();                                                                               // Trim result
    //         relatedKeywordsMessages.Add(message);                                                                                   // Append to chat histroy

    //         ControlPanel.instance.PrintRelatedKeyword(message.Content);                                                             // Print if needed

    //         return message.Content;

    //     }

    //     else return null;

    // }

    // public async Task<string> SpawnRelatedContextKeywordsOpenAI(string keyword, List<string> previousLayerKeywords)
    // {

    //     ChatMessage newMessage = new()
    //     {                                                                                       // Format of message user's gonna send

    //         Role = "user",
    //         Content = FormRequest.instance.FormDerivedContextKeywordRequest(keyword, previousLayerKeywords)

    //     };

    //     if (ControlPanel.instance.PreserveMessageHistory)
    //     {                                                                         // Add request to related keywords chat message history    

    //         relatedKeywordsMessages.Add(newMessage);

    //     }

    //     else
    //     {

    //         relatedKeywordsMessages.Clear();
    //         relatedKeywordsMessages.Add(newMessage);

    //     }

    //     var spawnRelatedKeywordsResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
    //     {                    // Send request to OpenAI

    //         Model = "gpt-3.5-turbo",
    //         Messages = relatedKeywordsMessages,                                                                                     // Given whole chat history 
    //         Temperature = 0.3f,
    //         MaxTokens = 300,
    //         N = 1,
    //         // TopP=1.0f,
    //         FrequencyPenalty = 0.8f,
    //         PresencePenalty = 0.0f

    //     });

    //     if (spawnRelatedKeywordsResponse.Choices != null && spawnRelatedKeywordsResponse.Choices.Count > 0)
    //     {                       // Make sure there's text generated

    //         var message = spawnRelatedKeywordsResponse.Choices[0].Message;                                                          // Get message
    //         message.Content = message.Content.Trim();                                                                               // Trim result
    //         relatedKeywordsMessages.Add(message);                                                                                   // Append to chat histroy

    //         ControlPanel.instance.PrintRelatedContextKeyword(message.Content);                                                             // Print if needed

    //         return message.Content;

    //     }

    //     else return null;

    // }

    // public async Task<List<string>> GetGenerateSentencesOpenAI(string context, List<string> selectedKeywordsList)
    // {

    //     string keywordsToString = string.Join(",", selectedKeywordsList);

    //     string OpenAIResponse = await GenerateSentencesOpenAI(context, keywordsToString);
    //     List<string> generatedSentenceList = ExtractWithDifferentFormat(OpenAIResponse, ControlPanel.instance.MaxSentenceCharacterNumber);

    //     if (generatedSentenceList == null || generatedSentenceList.Count < 2)
    //     {

    //         Debug.Log("Regenerate Sentence");
    //         string secondOpenAIResponse = await GenerateSentencesOpenAI(context, keywordsToString);
    //         List<string> secondGeneratedSentenceList = ExtractWithDifferentFormat(secondOpenAIResponse, ControlPanel.instance.MaxSentenceCharacterNumber);

    //         foreach (string sentence in secondGeneratedSentenceList)
    //         {

    //             SentenceHistoryDictionary.instance.AddToSentenceDictionary(keywordsToString, sentence);                            // Add to related keywords history

    //         }

    //         return secondGeneratedSentenceList;

    //     }

    //     else
    //     {

    //         foreach (string sentence in generatedSentenceList)
    //         {

    //             SentenceHistoryDictionary.instance.AddToSentenceDictionary(keywordsToString, sentence);                            // Add to related keywords history

    //         }

    //         return generatedSentenceList;

    //     }


    // }

    /// <summary>-------------------------------------------------------------- </summary> 
    /// <summary>-------   Generate sentences from selected keywords    ------- </summary>  
    /// <summary>-------------------------------------------------------------- </summary>
    // public async Task<string> GenerateSentencesOpenAI(string context, string selectedKeywordsToString)
    // {

    //     ChatMessage newMessage = new()
    //     {                                                                                            // Format of message user's gonna send

    //         Role = "user",
    //         Content = FormRequest.instance.FormGenerateSentenceRequest(context, selectedKeywordsToString)

    //     };

    //     if (ControlPanel.instance.PreserveMessageHistory)
    //     {                                                                         // Add request to related keywords chat message history    

    //         generateSentencesMessages.Add(newMessage);

    //     }

    //     else
    //     {

    //         generateSentencesMessages.Clear();
    //         generateSentencesMessages.Add(newMessage);

    //     }

    //     var generateSentenceResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
    //     {

    //         Model = "gpt-3.5-turbo",
    //         Messages = generateSentencesMessages,
    //         Temperature = 0.3f,
    //         MaxTokens = 300,
    //         // TopP=1.0f,
    //         FrequencyPenalty = 0.8f,
    //         PresencePenalty = 0.0f

    //     });

    //     if (generateSentenceResponse.Choices != null && generateSentenceResponse.Choices.Count > 0)
    //     {                   // Make sure there's text generated

    //         var message = generateSentenceResponse.Choices[0].Message;                                                  // Get message
    //         message.Content = message.Content.Trim();                                                                   // Trim result

    //         ControlPanel.instance.PrintGeneratedSentence(message.Content);                                              // Print if needed

    //         return message.Content;

    //     }

    //     else return null;

    // }

    public List<string> ExtractWithDifferentFormat(string responseContent, int maxCharacterNum)
    {
      List<string> ExtractedList = new();

      /// <summary>-------   Try parse with new line    ------- </summary>  
      string[] words = responseContent.Split('\n');

      foreach (var word in words)
      {
        if (!string.IsNullOrWhiteSpace(word))
        {
          if (word.Length > maxCharacterNum)
          {
            /// <summary>-------   Try parse with comma    ------- </summary>  
            string[] words2 = responseContent.Split(',');

            foreach (var word2 in words2)
            {
              if (!string.IsNullOrWhiteSpace(word2))
              {
                if (word2.Length > maxCharacterNum)
                {
                  return null;
                }
                else
                {
                  ExtractedList.Add(word2);
                }

              }

            }
          }
          else
          {
            ExtractedList.Add(word);
          }
        }
      }

      return ExtractedList;
    }

    /// <summary>-------   Extracts context keyowrds and check for wrong format    ------- </summary> 
    // private async Task<List<string>> extractContextKeywords(string responseContent, string newInput) {

    //     List<string> ExtractedContextKeywordsList = new List<string>();
    //     string[] words = responseContent.Split('\n');

    //     foreach (var word in words) {

    //         if(!string.IsNullOrWhiteSpace(word)) {

    //             if ( word.Length > 40 ) {    

    //                 List<string> reExtractedContextKeywordsList = await ExtractContextKeywordsOpenAI(newInput);         // Send request again, maybe not a good idea, might cause infinite loop
    //                 return reExtractedContextKeywordsList;

    //             } else {

    //                 ExtractedContextKeywordsList.Add(word);

    //             }


    //         }

    //     }

    //     return ExtractedContextKeywordsList;

    // }

    /// <summary>-------   Extract Related Keywords and check for wrong format    ------- </summary> 
    // private async Task<List<string>> extractRelatedKeywords(string responseContent, string keyword, Transform centerTransform) {

    //     List<string> extractedRelatedKeywordsList = new List<string>();
    //     string[] words = responseContent.Split('\n');

    //     foreach (var word in words) {

    //         if(!string.IsNullOrWhiteSpace(word)) {

    //             if ( word.Length > 40 ) {                                                                                       // OpenAI gave wrong format, normally seperate by comma rather than newline   


    //                 if (RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary.ContainsKey(keyword)){            // Check key before removing it

    //                     RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary.Remove(keyword);

    //                 }   

    //                 List<string> reExtractedRelatedKeywordsList = await SpawnRelatedKeywordsOpenAI(centerTransform, keyword);   // Send request again, maybe not a good idea, might cause infinite loop
    //                 return reExtractedRelatedKeywordsList;

    //             } else {

    //                 extractedRelatedKeywordsList.Add(word);
    //                 RelatedKeywordHistoryDionary.instance.AddToRelatedKeyowrdHistory(keyword, word);                            // Add to related keywords history

    //             }

    //         }

    //     }

    //     return extractedRelatedKeywordsList;

    // }

    /// <summary>-------   Generate sentences and check for wrong format    ------- </summary> 
    // private async Task<List<string>> extractGeneratedSentence(string responseContent, List<string> selectedKeywordsList) {

    //     List<string> generatedSentenceList = new List<string>();
    //     string[] sentences = responseContent.Split('\n');

    //     foreach (var sentence in sentences) {

    //         if (!string.IsNullOrWhiteSpace(sentence)) {

    //             if ( sentence.Length > 500 ) {                                                                                                  // OpenAI gave wrong format, normally seperate by comma rather than newline       

    //                 List<string> reGeneratedSentencesList = await GenerateSentencesOpenAI(selectedKeywordsList);                                // Send request again, maybe not a good idea, might cause infinite loop
    //                 return reGeneratedSentencesList;

    //             } else {

    //                 generatedSentenceList.Add(sentence);

    //             }



    //         }

    //     }

    //     return generatedSentenceList;

    // }


    // public async Task<string> SpawnPureDKOpenAI(string keyword)
    // {

    //     ChatMessage newMessage = new()
    //     {                                                                                       // Format of message user's gonna send

    //         Role = "user",
    //         Content = Prompts.instance.formPureDKRequest(keyword)

    //     };

    //     List<ChatMessage> PureDKMessages = new List<ChatMessage>();
    //     PureDKMessages.Add(newMessage);

    //     var spawnRelatedKeywordsResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
    //     {                    // Send request to OpenAI

    //         Model = "gpt-3.5-turbo",
    //         Messages = PureDKMessages,                                                                                  // Given whole chat history 
    //         Temperature = 0.3f,
    //         MaxTokens = 300,
    //         N = 1,
    //         // TopP=1.0f,
    //         FrequencyPenalty = 0.8f,
    //         PresencePenalty = 0.0f

    //     });

    //     if (spawnRelatedKeywordsResponse.Choices != null && spawnRelatedKeywordsResponse.Choices.Count > 0)
    //     {                       // Make sure there's text generated

    //         var message = spawnRelatedKeywordsResponse.Choices[0].Message;                                                          // Get message
    //         message.Content = message.Content.Trim();                                                                               // Trim result
    //         relatedKeywordsMessages.Add(message);                                                                                   // Append to chat histroy

    //         ControlPanel.instance.PrintRelatedKeyword(message.Content);                                                             // Print if needed

    //         return message.Content;

    //     }

    //     else return null;

    // }

    // public async Task<string> SpawnContextDKOpenAI(string keyword, string context)
    // {

    //     ChatMessage newMessage = new()
    //     {                                                                                       // Format of message user's gonna send

    //         Role = "user",
    //         Content = Prompts.instance.formContextDKRequest(keyword, context)

    //     };


    //     List<ChatMessage> ContextDKMessages = new List<ChatMessage>();
    //     ContextDKMessages.Add(newMessage);

    //     var spawnRelatedKeywordsResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
    //     {                    // Send request to OpenAI

    //         Model = "gpt-3.5-turbo",
    //         Messages = ContextDKMessages,                                                                                     // Given whole chat history 
    //         Temperature = 0.3f,
    //         MaxTokens = 300,
    //         N = 1,
    //         // TopP=1.0f,
    //         FrequencyPenalty = 0.8f,
    //         PresencePenalty = 0.0f

    //     });

    //     if (spawnRelatedKeywordsResponse.Choices != null && spawnRelatedKeywordsResponse.Choices.Count > 0)
    //     {                       // Make sure there's text generated

    //         var message = spawnRelatedKeywordsResponse.Choices[0].Message;                                                          // Get message
    //         message.Content = message.Content.Trim();                                                                               // Trim result
    //         relatedKeywordsMessages.Add(message);                                                                                   // Append to chat histroy

    //         ControlPanel.instance.PrintRelatedKeyword(message.Content);                                                             // Print if needed

    //         return message.Content;

    //     }

    //     else return null;

    // }
  }
}


