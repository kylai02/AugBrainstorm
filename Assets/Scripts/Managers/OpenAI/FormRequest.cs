using System.Collections.Generic;
using UnityEngine;

public class FormRequest : MonoBehaviour {
  /// <summary>-------   Singleton stuff    ------- </summary>
  public static FormRequest instance;

  void Awake() {
    instance ??= this;
  }

  /// <summary>---------------------------------------------------------------------- </summary> 
  /// <summary>-------   Form extract context keywords request from prompt    ------- </summary> 
  /// <summary>---------------------------------------------------------------------- </summary>   

  public string FormExtractContextKeywordRequest(string newSpeechInput) {
    return
      "Main Sentence: " + newSpeechInput + "\n" +

      "You are a reasarcher reviewing an work and taking notes, noting down the keywords. " +

      "Please extract three to five keywords from the previous Main Sentence. Which is also showed as follow: " + "\n" +

      "I repeat, please extract only three to five keywords from the previous Main Sentence. Which is also showed as follow: " + "\n" +

      "Main Sentence: " + newSpeechInput + "\n" +

      "The extracted Keywords must only exist in the Main Sentence. " + "\n" +

      "Please do not extract keywords that are preposition, greeting words or other words that are irrelevant to the sentence." +

      "Please give me only the keywords. " + "\n" +

      "Please provide me with the keywords in a format that keywords are separated by a newline, not a comma, and without an order number. ";
  }

    /// <summary>------------------------------------------------------------------ </summary> 
    /// <summary>-------   Derive related keywords from original keyword    ------- </summary> 
    /// <summary>------------------------------------------------------------------ </summary>   


    // public string FormDerivedKeywordRequest(string keyword, List<string> previousLayerKeywords)
    // {

    //     int requestKeywordNumber = (ControlPanel.instance.Style == 1) ? (ControlPanel.instance.SpawnBubbleNumber + 2) : ControlPanel.instance.WidthNumber * ControlPanel.instance.DepthNumber + 2;                                                   // Little bit more

    //     string currentContextKeywords = keyword;
    //     currentContextKeywords += ", " + getShowingContextKeywords();



    //     string previousContext = getContext();

    //     // if (previousLayerKeywords.Count > 0) {

    //     //     string previousLayerKeywordDerivedKeywords = getPreviousLayerKeywordDerivedKeywords(previousLayerKeywords);
    //     //     currentContextKeywords += ", " + previousLayerKeywordDerivedKeywords;

    //     // }

    //     if (RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary.ContainsKey(keyword))
    //     {                                                       // Previous keywords exists                               

    //         string previousKeywordsToString = string.Join(",", RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary[keyword]);

    //         return Prompts.instance.DeriveKeywordsPrompt(previousContext, keyword, previousKeywordsToString, requestKeywordNumber, currentContextKeywords, true);               // Context and previous keywords

    //     }

    //     else
    //     {

    //         return Prompts.instance.DeriveKeywordsPrompt(previousContext, keyword, " ", requestKeywordNumber, currentContextKeywords, false);                                    // Context 

    //     }

    // }


    // /// <summary>--------------------------------------------------------- </summary> 
    // /// <summary>-------   Derive related keywords from context    ------- </summary> 
    // /// <summary>--------------------------------------------------------- </summary>   


    // public string FormDerivedContextKeywordRequest(string keyword, List<string> previousLayerKeywords)
    // {

    //     int requestKeywordNumber = (ControlPanel.instance.Style == 1) ? (ControlPanel.instance.SpawnBubbleNumber / 2 + 2) : ControlPanel.instance.WidthNumber * ControlPanel.instance.DepthNumber / 2 + 2;                                               // Little bit more

    //     string doNotOverLapWords = getShowingContextKeywords();

    //     if (RelatedKeywordsManager.instance.RelatedContextKeywordsList.Count > 0)
    //     {

    //         string previousRelatedContextKeywords = string.Join(",", RelatedKeywordsManager.instance.RelatedContextKeywordsList);
    //         doNotOverLapWords += ", " + previousRelatedContextKeywords;

    //     }

    //     if (previousLayerKeywords.Count > 0)
    //     {

    //         string previousLayerKeywordDerivedKeywords = getPreviousLayerKeywordDerivedKeywords(previousLayerKeywords);
    //         doNotOverLapWords += ", " + previousLayerKeywordDerivedKeywords;

    //     }

    //     if (RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary.ContainsKey(keyword))
    //     {                                                       // Previous keywords exists                               

    //         // string previousKeywordsToString = string.Join(",", RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary[keyword]);

    //         // if (HistoryTranscript.instance.Transcripts.Count > ControlPanel.instance.minContextSentenceToSpawnContextRelatedKeywords ){                                                                                             // Spawn context related

    //         //     string previousContext = string.Join("\n", HistoryTranscript.instance.Transcripts);
    //         //     return  Prompts.instance.DeriveContextKeywordsPrompt(keyword, previousKeywordsToString, previousContext, requestKeywordNumber, currentContextKeywords, true, true);        // Context and previous keywords


    //         // }

    //         // else {

    //         //     return  Prompts.instance.DeriveKeywordsPrompt(keyword, previousKeywordsToString, " ", requestKeywordNumber, currentContextKeywords, false, true);                   // Previous keywords

    //         // }
    //         string previousKeywordsToString = string.Join(",", RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary[keyword]);
    //         string previousContext = getContext();

    //         return Prompts.instance.DerivePureKeywordsPrompt(keyword, previousKeywordsToString, previousContext, requestKeywordNumber, doNotOverLapWords, true);

    //     }

    //     else
    //     {

    //         string previousContext = getContext();

    //         return Prompts.instance.DerivePureKeywordsPrompt(keyword, "", previousContext, requestKeywordNumber, doNotOverLapWords, false);

    //     }

    // }

    // public string FormGenerateSentenceRequest(string context, string selectedKeywordsToString)
    // {



    //     if (ModeManager.instance.Mode == 1)
    //     {

    //         if (SelectedKeywordsManager.instance.QuestionKeywordList.Contains("?"))
    //         {

    //             if (SelectedKeywordsManager.instance.QuestionKeywordList.Count > 1)
    //             {

    //                 string questionKeywordsToString = string.Join(",", SelectedKeywordsManager.instance.QuestionKeywordList);
    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, questionKeywordsToString, true, true);

    //             }


    //             else
    //             {

    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, " ", false, true);
    //                 // return "Please generate three question sentences, the generated question sentences must all contain the following keywords: " + selectedKeywordsToString + "." + "\n" + "Please seperate the sentences by newLine and without order number.";   

    //             }


    //         }

    //         else
    //         {

    //             if (SelectedKeywordsManager.instance.QuestionKeywordList.Count > 0)
    //             {


    //                 string questionKeywordsToString = string.Join(",", SelectedKeywordsManager.instance.QuestionKeywordList);
    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, questionKeywordsToString, true, false);

    //             }


    //             else
    //             {

    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, " ", false, false);
    //                 // return "Please generate three question sentences, the generated question sentences must all contain the following keywords: " + selectedKeywordsToString + "." + "\n" + "Please seperate the sentences by newLine and without order number.";   

    //             }

    //         }

    //     }

    //     else if (ModeManager.instance.Mode == 8)
    //     {

    //         if (RefineManager.instance.selectedQuestion.Contains("?"))
    //         {

    //             if (RefineManager.instance.selectedQuestion.Count > 1)
    //             {

    //                 string questionKeywordsToString = string.Join(",", RefineManager.instance.selectedQuestion);
    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, questionKeywordsToString, true, true);

    //             }

    //             else
    //             {

    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, " ", false, true);

    //             }

    //         }

    //         else
    //         {

    //             if (RefineManager.instance.selectedQuestion.Count > 0)
    //             {

    //                 string questionKeywordsToString = string.Join(",", RefineManager.instance.selectedQuestion);
    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, questionKeywordsToString, true, false);

    //             }


    //             else
    //             {

    //                 return Prompts.instance.GenerateSentencesPrompt(context, selectedKeywordsToString, " ", false, false);

    //             }

    //         }

    //     }

    //     else return "";

    // }

    // public string getShowingContextKeywords()
    // {

    //     if (ModeManager.instance.Mode == 1)
    //     {

    //         //return string.Join(",", ContextKeywordsManager.instance.ContextKeywordList1) + string.Join(",", ContextKeywordsManager.instance.ContextKeywordList2); 
    //         List<string> contextKeywords = new List<string>();
    //         if (HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count >= ControlPanel.instance.NumberEachRow * 2)
    //         {

    //             contextKeywords.AddRange(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.GetRange(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - ControlPanel.instance.NumberEachRow * 2, ControlPanel.instance.NumberEachRow * 2));
    //         }

    //         else
    //         {

    //             contextKeywords = new List<string>(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList);

    //         }

    //         return string.Join(",", contextKeywords);

    //     }

    //     else if (ModeManager.instance.Mode == 8)
    //     {

    //         string showingContext = "";

    //         foreach (List<string> contextList in RefineManager.instance.RefiningContextKeywwordList)
    //         {

    //             showingContext += string.Join(",", contextList);

    //         }

    //         return showingContext;

    //     }

    //     else
    //     {

    //         Debug.Log("Mode Error");
    //         return "";

    //     }

    // }

    // private string getPreviousLayerKeywordDerivedKeywords(List<string> previousLayerKeywords)
    // {

    //     string previousLayerKeywordDerivedKeywords = "";

    //     foreach (string previousLayerKeyword in previousLayerKeywords)
    //     {

    //         if (RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary.ContainsKey(previousLayerKeyword))
    //         {                                                          // Current layer keyword to search

    //             previousLayerKeywordDerivedKeywords += string.Join(",", RelatedKeywordHistoryDionary.instance.RelatedKeywordHistoryDictionary[previousLayerKeyword]);                      // Get related keywords

    //         }

    //     }

    //     return previousLayerKeywordDerivedKeywords;

    // }

    // private string getContext()
    // {

    //     string previousContext = "";

    //     if (ModeManager.instance.Mode == 1)
    //     {

    //         if (HistoryTranscript.instance.Transcripts.Count >= ControlPanel.instance.minContextSentenceToSpawnContextRelatedKeywords)
    //         {

    //             int transcriptNumm = ControlPanel.instance.minContextSentenceToSpawnContextRelatedKeywords;
    //             if (HistoryTranscript.instance.Transcripts.Count > ControlPanel.instance.maxContextSentenceToSpawnContextRelatedKeywords)
    //             {

    //                 transcriptNumm = ControlPanel.instance.maxContextSentenceToSpawnContextRelatedKeywords;

    //             }
    //             else
    //             {

    //                 transcriptNumm = HistoryTranscript.instance.Transcripts.Count;

    //             }

    //             previousContext = string.Join(",", HistoryTranscript.instance.Transcripts.GetRange(HistoryTranscript.instance.Transcripts.Count - transcriptNumm, transcriptNumm));

    //         }

    //     }

    //     else if (ModeManager.instance.Mode == 8)
    //     {

    //         previousContext = string.Join("\n", RefineManager.instance.transcript);

    //     }

    //     else
    //     {


    //     }

    //     return previousContext;

    // }





}


