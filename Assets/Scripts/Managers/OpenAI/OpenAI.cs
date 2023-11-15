using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace OpenAI
{
  public class OpenAI : MonoBehaviour
  {
    /// <summary>-------   Related Keywords Message History    ------- </summary> 
    private List<ChatMessage> generatedIdeasMessages = new List<ChatMessage>();
    private List<ChatMessage> generatedKeywordsMessages = new List<ChatMessage>();
    private List<ChatMessage> mergedConditionsMessages = new List<ChatMessage>();
    private List<ChatMessage> contextKeywordsMessages = new List<ChatMessage>();

    /// <summary>-------   API    ------- </summary> 
    private readonly OpenAIApi openai = new();

    /// <summary>-------   Singleton stuff    ------- </summary>
    public static OpenAI instance;

    private void Awake()
    {
      instance ??= this;
    }

    /// <summary>--------------------------------------------------------- </summary>
    /// <summary>-------   Generate ideas from selected keywords   ------- </summary>
    /// <summary>--------------------------------------------------------- </summary>

    public async Task<List<string>> GetGeneratedIdeasOpenAI(
      List<string> selectedKeywords,
      int requestIdeaNumber,   // how many ideas to generate
      int requestDescriptionLen  // the length limit of each description
    )
    {
      string OpenAIResponse =
        await RequestGeneratedIdeasOpenAI(selectedKeywords, requestIdeaNumber, requestDescriptionLen);

      // Debug.Log("Response: \n" + OpenAIResponse + "\n");

      List<string> responseList = // idea name with even index, description with odd index
        ExtractGeneratedIdeas(OpenAIResponse, 120);

      // Debug.Log("ResponseList:");
      // foreach (string str in responseList)
      // {
      //   Debug.Log(str + "\n");
      // }

      // Still wrong format or nothing is generated
      if (responseList == null || responseList.Count < requestIdeaNumber * 2)
      {
        Debug.Log("Regenerate Ideas");
        // Retry once
        string secondOpenAIResponse =
          await RequestGeneratedIdeasOpenAI(selectedKeywords, requestIdeaNumber, requestDescriptionLen);
        List<string> secondResponseList =
          ExtractGeneratedIdeas(secondOpenAIResponse, 120);  // 20

        return secondResponseList;
      }
      else
      {
        return responseList;
      }
    }

    private async Task<string> RequestGeneratedIdeasOpenAI(
      List<string> selectedKeywords,
      int requestIdeaNumber,   // how many ideas to generate
      int requestDescriptionLen  // the length limit of each description
    )
    {
      // Format of message user's gonna send
      ChatMessage newMessage = new()
      {
        Role = "user",
        Content =
          Prompts.instance.GenerateIdeasPrompt(selectedKeywords, requestIdeaNumber, requestDescriptionLen)
      };

      generatedIdeasMessages.Clear();
      generatedIdeasMessages.Add(newMessage);

      var GeneratedIdeasResponse =
        await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
          Model = "gpt-3.5-turbo",
          Messages = generatedIdeasMessages,
          Temperature = 0.5f,
          MaxTokens = 50,
          // TopP=1.0f,
          FrequencyPenalty = 0.8f,
          PresencePenalty = 0.0f
        });

      // Make sure there's text generated
      if (
        GeneratedIdeasResponse.Choices != null &&
        GeneratedIdeasResponse.Choices.Count > 0
      )
      {
        var message = GeneratedIdeasResponse.Choices[0].Message;

        message.Content = message.Content.Trim(); // Trim result
        generatedIdeasMessages.Add(message); // Append to chat history

        return message.Content;
      }
      else
      {
        return null;
      }
    }

    public List<string> ExtractGeneratedIdeas(string responseContent, int maxCharacterNum)
    {
      List<string> ExtractedList = new();
      char colon = ':';

      /// <summary>-------   Try parse with new line    ------- </summary>  
      string[] words = responseContent.Split('\n');

      // foreach (var word in words)
      // {
      //   if (!string.IsNullOrEmpty(word))
      //   {
      //     ExtractedList.Add(word);
      //   }
      // }

      for (int i = 0 ; i < words.Length ; ++i)
      {
        int colonIndex = words[i].IndexOf(colon);
        if (colonIndex >= 0)
        {
          words[i] = words[i].Substring(colonIndex + 1);
        }
        
        if (!string.IsNullOrEmpty(words[i]))
        {
          ExtractedList.Add(words[i]);
        }
      }

      return ExtractedList;
    }

    /// <summary>-------------------------------------------------------------------- </summary>
    /// <summary>-------   Generate new keywords from previous thought path   ------- </summary>
    /// <summary>-------------------------------------------------------------------- </summary>

    public async Task<List<string>> GetGeneratedKeywordsOpenAI(
      List<string> preKeywords,
      List<string> conditions,
      int requestKeywordNumber
    )
    {
      string OpenAIResponse =
        await RequestGeneratedKeywordsOpenAI(preKeywords, conditions, requestKeywordNumber);

      // Debug.Log("Response: \n" + OpenAIResponse + "\n");

      List<string> generatedKeywordsList =
        ExtractGeneratedKeywords(OpenAIResponse, 20);

      // Debug.Log("KeywordsList: \n");
      // foreach (string str in generatedKeywordsList)
      // {
      //   Debug.Log(str + "\n");
      // }

      // Still wrong format or nothing is generated
      if (generatedKeywordsList == null || generatedKeywordsList.Count < requestKeywordNumber)
      {
        Debug.Log("Regenerate Keywords");
        // Retry once
        string secondOpenAIResponse =
          await RequestGeneratedKeywordsOpenAI(preKeywords, conditions, requestKeywordNumber);
        List<string> secondgeneratedKeywordsList =
          ExtractGeneratedKeywords(secondOpenAIResponse, 120);  // 20

        return secondgeneratedKeywordsList;
      }
      else
      {
        return generatedKeywordsList;
      }
    }

    private async Task<string> RequestGeneratedKeywordsOpenAI(
      List<string> preKeywords,
      List<string> conditions,
      int requestKeywordNumber
    )
    {
      // Format of message user's gonna send
      ChatMessage newMessage = new()
      {
        Role = "user",
        Content =
          Prompts.instance.GenerateKeywordsPrompt(preKeywords, conditions, requestKeywordNumber)
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
      if (
        GeneratedKeywordsResponse.Choices != null &&
        GeneratedKeywordsResponse.Choices.Count > 0
      )
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

    public List<string> ExtractGeneratedKeywords(string responseContent, int maxCharacterNum)
    {
      List<string> ExtractedList = new();
      char colon = ':';

      /// <summary>-------   Try parse with new line    ------- </summary>  
      string[] words = responseContent.Split('\n');

      for (int i = 0 ; i < words.Length ; ++i)
      {
        int colonIndex = words[i].IndexOf(colon);
        if (colonIndex >= 0)
        {
          words[i] = words[i].Substring(colonIndex + 1);
        }
        
        if (!string.IsNullOrEmpty(words[i]))
        {
          ExtractedList.Add(words[i]);
        }
      }

      return ExtractedList;
    }

    /// <summary>--------------------------------------------- </summary>
    /// <summary>-------   Merge multiple conditions   ------- </summary>
    /// <summary>--------------------------------------------- </summary>

    public async Task<List<string>> MergeConditionsOpenAI(
      List<string> conditions
    )
    {
      string OpenAIResponse =
        await RequestMergedConditionsOpenAI(conditions);

      Debug.Log("Response: \n" + OpenAIResponse + "\n");

      List<string> responseList = // idea name with even index, description with odd index
        ExtractMergedConditions(OpenAIResponse, 120);

      // Still wrong format or nothing is generated
      if (responseList == null || responseList.Count < 1)
      {
        Debug.Log("Remerge conditions");
        // Retry once
        string secondOpenAIResponse =
          await RequestMergedConditionsOpenAI(conditions);
        List<string> secondResponseList =
          ExtractMergedConditions(secondOpenAIResponse, 120);  // 20

        return secondResponseList;
      }
      else
      {
        return responseList;
      }
    }

    private async Task<string> RequestMergedConditionsOpenAI(
      List<string> conditions
    )
    {
      // Format of message user's gonna send
      ChatMessage newMessage = new()
      {
        Role = "user",
        Content =
          Prompts.instance.MergeConditionsPrompt(conditions)
      };

      mergedConditionsMessages.Clear();
      mergedConditionsMessages.Add(newMessage);

      var MergedConditionsResponse =
        await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
          Model = "gpt-3.5-turbo",
          Messages = mergedConditionsMessages,
          Temperature = 0.5f,
          MaxTokens = 50,
          // TopP=1.0f,
          FrequencyPenalty = 0.8f,
          PresencePenalty = 0.0f
        });

      // Make sure there's text generated
      if (
        MergedConditionsResponse.Choices != null &&
        MergedConditionsResponse.Choices.Count > 0
      )
      {
        var message = MergedConditionsResponse.Choices[0].Message;

        message.Content = message.Content.Trim(); // Trim result
        mergedConditionsMessages.Add(message); // Append to chat history

        return message.Content;
      }
      else
      {
        return null;
      }
    }

    public List<string> ExtractMergedConditions(string responseContent, int maxCharacterNum)
    {
      List<string> ExtractedList = new();
      char colon = ':';

      /// <summary>-------   Try parse with new line    ------- </summary>  
      string[] words = responseContent.Split('\n');

      for (int i = 0 ; i < words.Length ; ++i)
      {
        int colonIndex = words[i].IndexOf(colon);
        if (colonIndex >= 0)
        {
          words[i] = words[i].Substring(colonIndex + 1);
        }
        
        if (!string.IsNullOrEmpty(words[i]))
        {
          ExtractedList.Add(words[i]);
        }
      }

      return ExtractedList;
    }

    /// <summary>--------------------------------------------------------- </summary> 
    /// <summary>-------   Extract context keywords from speech    ------- </summary> 
    /// <summary>--------------------------------------------------------- </summary>     

    public async Task<List<string>> GetContextKeywordsOpenAI(string newInput)
    {
      string OpenAIResponse = await RequestContextKeywordsOpenAI(newInput);
      List<string> contextKeywordsList = ExtractContextKeywords(OpenAIResponse, 20);

      // Still wrong format or nothing is generated
      if (contextKeywordsList == null || contextKeywordsList.Count < 2)
      {
        Debug.Log("Regenerate Context Keywords");
        // Retry once
        string secondOpenAIResponse = await RequestContextKeywordsOpenAI(newInput);
        List<string> secondcontextKeywordsList =
        ExtractContextKeywords(secondOpenAIResponse, 20);

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

    public List<string> ExtractContextKeywords(string responseContent, int maxCharacterNum)
    {
      List<string> ExtractedList = new();

      /// <summary>-------   Try parse with new line    ------- </summary>  
      string[] words = responseContent.Split('\n');

      foreach (var word in words)
      {
        if (!string.IsNullOrEmpty(word)) // IsNullOrWhiteSpace(word)
        {
          if (word.Length > maxCharacterNum)
          {
            /// <summary>-------   Try parse with comma    ------- </summary>  
            string[] words2 = responseContent.Split(',');

            foreach (var word2 in words2)
            {
              if (!string.IsNullOrEmpty(word2)) // IsNullOrWhiteSpace(word)
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
  }
}


