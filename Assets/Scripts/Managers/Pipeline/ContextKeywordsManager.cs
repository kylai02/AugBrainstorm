using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class ContextKeywordsManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]

  /// <summary>-------   Context page    ------- </summary>
  public int transcriptNum;

  /// <summary>-------   Context page    ------- </summary>
  public int contextPageNumber;

  /// <summary>-------   Update keywords order    ------- </summary>
  private int lastModified;

  private int keywordsNum;

  /// <summary>-------   Context page    ------- </summary>
  public List<int> PreviousTranscriptNums;
  public List<int> TempPreviousTranscriptNums;

  /// <summary>-------   Update keywords order    ------- </summary>
  // public List<string> ContextSentences = new List<string>();

  /// <summary>-------   Context keywords lists    ------- </summary>
  public List<string> ContextKeywordList1 = new List<string>();
  public List<string> ContextKeywordList2 = new List<string>();

  /// <summary>-------   Singleton stuff    ------- </summary>
  public static ContextKeywordsManager instance;

  void Awake() {
    if (instance == null) instance = this;
  }

  void Update() {
    updateContextKeywords();
  }

  /// <summary>-------   Generate and place context keywords    -------</summary>
  private async void updateContextKeywords() {
    /// <summary>-------   Get transcribe from Whisper    -------</summary>
    // Get speech from Whisper, if null then = " "  
    string speechInput = TCPManager.instance.TranscribeFromWhipser() ?? " ";

    if (speechInput == " ") return;
            
    TCPManager.instance.ResetReceivedTranscribe();
        //   ControlPanel.instance.PrintTranscribe(speechInput);
        //   HistoryTranscript.instance.UpdateContextSentences(speechInput);
        //   transcriptNum = HistoryTranscript.instance.Transcripts.Count - 1;

    /// <summary>-------   Send to OpenAI to extract context keywords    ------- </summary>
    // Send request to OpenAI 
    List<string> ExtractedContextKeywords = 
        await OpenAI.OpenAI.instance.GetContextKeywordsOpenAI(speechInput);

    if (ExtractedContextKeywords == null || ExtractedContextKeywords.Count == 0) 
        return;

    List<string> ToBeRemoved = new();
    foreach (string word in ExtractedContextKeywords) {
        if (StopWords.instance.ForbiddenContextKeywords.Contains(word)) {
        ToBeRemoved.Add(word);
        }
    }

    foreach (string s in ToBeRemoved) ExtractedContextKeywords.Remove(s);

    foreach (string s in ExtractedContextKeywords) {
      Debug.Log(s);
    }


    // /// <summary>-------   Place context keywords    ------- </summary>
    // UpdatePreviousContextKeywords(ExtractedContextKeywords);
    // // PlaceFullContextKeywordSentences();

    // if (contextPageNumber == 0) {
    //     PlaceSimpleContextKeywords(transcriptNum); // Place context keywords
    // }

    // /// <summary>-------   Preserve keywords    ------- </summary>                                                                                       
    // HistoryContextKeywordsManager.instance.PreserveHistoryContextKeywords(
    //     ExtractedContextKeywords, transcriptNum
    // );
  }

    // /// <summary>------------------------------------------------------------- </summary> 
    // /// <summary>-------   Context keyword placement in full mode    --------- </summary> 
    // /// <summary>------------------------------------------------------------- </summary>
    // private void UpdatePreviousContextKeywords(List<String> newContextKeywordsList)
    // {                                                                               // Sentence-based placement


    //     // ContextKeywordList5 = ContextKeywordList4;
    //     // ContextKeywordList4 = ContextKeywordList3;
    //     // ContextKeywordList3 = ContextKeywordList2;
    //     ContextKeywordList2 = ContextKeywordList1;
    //     ContextKeywordList1 = newContextKeywordsList;
    //     changeLineSize(newContextKeywordsList.Count);

    //     foreach (string s in newContextKeywordsList)
    //     {

    //         PreviousTranscriptNums.Add(transcriptNum);

    //     }

    // }

    // public void changeLineSize(int newContextNumber)
    // {

    //     if (ControlPanel.instance.TouchMode)
    //     {

    //         if (newContextNumber <= 4)
    //         {

    //             Line1.localScale = new Vector3(Mathf.Clamp(-1 * (3.8f * newContextNumber + 0.5f * (newContextNumber - 1)), -200f, 0), 1f, 1f);
    //             Line2.localScale = new Vector3(0f, 1f, 1f);

    //         }

    //         else if (newContextNumber > 4 && newContextNumber <= 8)
    //         {

    //             Line1.localScale = new Vector3(-1 * (3.8f * 4 + 0.5f * (4 - 1)), 1f, 1f);
    //             Line2.localScale = new Vector3(Mathf.Clamp(-1 * (3.8f * (newContextNumber - 4) + 0.5f * (newContextNumber - 5)), -22.7f, 0), 1f, 1f);

    //         }

    //         else
    //         {

    //             Line1.localScale = new Vector3(-1 * (3.8f * 4 + 0.5f * (4 - 1)), 1f, 1f);
    //             Line2.localScale = new Vector3(-1 * (3.8f * (8 - 4) + 0.5f * (8 - 5)), 1f, 1f);

    //         }

    //     }

    //     else
    //     {

    //         if (newContextNumber <= 4)
    //         {

    //             Line1.localScale = new Vector3(Mathf.Clamp(-1 * (3.8f * newContextNumber + 2.5f * (newContextNumber - 1)), -200f, 0), 1f, 1f);
    //             Line2.localScale = new Vector3(0f, 1f, 1f);

    //         }

    //         else if (newContextNumber > 4 && newContextNumber <= 8)
    //         {

    //             Line1.localScale = new Vector3(-1 * (3.8f * 4 + 2.5f * (4 - 1)), 1f, 1f);
    //             Line2.localScale = new Vector3(Mathf.Clamp(-1 * (3.8f * (newContextNumber - 4) + 2.5f * (newContextNumber - 5)), -22.7f, 0), 1f, 1f);

    //         }

    //         else
    //         {

    //             Line1.localScale = new Vector3(-1 * (3.8f * 4 + 2.5f * (4 - 1)), 1f, 1f);
    //             Line2.localScale = new Vector3(-1 * (3.8f * (8 - 4) + 2.5f * (8 - 5)), 1f, 1f);

    //         }

    //     }

    // }


    // // public void PlaceFullContextKeywordSentences() {

    // //     ContextBubblePoolingManager.instance.ClearContextBubble();
    // //     placeSentenceContextKeywords(ContextKeywordList1 , 5);
    // //     placeSentenceContextKeywords(ContextKeywordList2 , 4);
    // //     placeSentenceContextKeywords(ContextKeywordList3 , 3);
    // //     placeSentenceContextKeywords(ContextKeywordList4 , 2);
    // //     // placeSentenceContextKeywords(ContextKeywordList5 , 1);

    // // }



    // /// <summary>----------------------------------------------------------------- </summary> 
    // /// <summary>-------   New Context keyword placement in simple mode    ------- </summary> 
    // /// <summary>----------------------------------------------------------------- </summary>
    // public void PlaceSimpleContextKeywords(int transcriptNumber)
    // {                                                                                                                 // Word-based placement

    //     ContextBubblePoolingManager.instance.ClearContextBubble();
    //     placeNewNewContextKeywords(transcriptNumber);

    //     if (contextPageNumber == 0)
    //     {

    //         Line1.gameObject.SetActive(true);
    //         Line2.gameObject.SetActive(true);

    //     }

    //     else
    //     {

    //         Line1.gameObject.SetActive(false);
    //         Line2.gameObject.SetActive(false);

    //     }
    //     /// <summary>-------   Old sentence base context keywords    ------- </summary> 
    //     // placeSentenceContextKeywords(ContextKeywordList1 , 2, transcriptNumber);
    //     // placeSentenceContextKeywords(ContextKeywordList2 , 1, transcriptNumber -1);

    // }

    // public void placeNewNewContextKeywords(int transcriptNumber)
    // {

    //     int previousContextNum = HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count;
    //     int newContextKeywordNum = ContextKeywordList1.Count;

    //     /// <summary>-------   First few keywords    ------- </summary> 
    //     if (previousContextNum + newContextKeywordNum < ControlPanel.instance.NumberEachRow * 2)
    //     {

    //         /// <summary>-------   Place previous keywords    ------- </summary> 
    //         for (int i = ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum - previousContextNum; i < ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum; i++)
    //         {

    //             int fromzero = i - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum - previousContextNum);
    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(i).gameObject;
    //             obj.transform.GetComponent<Interactable>().transcriptNum = PreviousTranscriptNums[fromzero];           // transcriptNumber;                                      // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum - previousContextNum)]);                    // Set keyword    

    //         }

    //         for (int i = 0; i < newContextKeywordNum; i++)
    //         {

    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(SimpleContextKeywordsContainer2.childCount - newContextKeywordNum + i).gameObject;
    //             obj.transform.GetComponent<Interactable>().transcriptNum = transcriptNumber;                                        // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, ContextKeywordList1[i]);                                 // Set keyword    


    //         }

    //     }
    //     /// <summary>-------   Full keywords    ------- </summary> 
    //     else
    //     {

    //         ModeManager.instance.ToggleContextPageUpButton(true);

    //         /// <summary>-------   Place previous keywords    ------- </summary> 
    //         for (int i = previousContextNum - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum); i < previousContextNum; i++)
    //         {

    //             int fromzero = i - (previousContextNum - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum));
    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(fromzero).gameObject;
    //             // Debug.Log(PreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2) + i);
    //             obj.transform.GetComponent<Interactable>().transcriptNum = PreviousTranscriptNums[PreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2) + fromzero];                                 // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i]);                    // Set keyword    

    //         }

    //         /// <summary>-------   Place new line of keywords    ------- </summary> 
    //         for (int i = 0; i < newContextKeywordNum; i++)
    //         {

    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(SimpleContextKeywordsContainer2.childCount - newContextKeywordNum + i).gameObject;
    //             obj.transform.GetComponent<Interactable>().transcriptNum = transcriptNumber;                                      // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, ContextKeywordList1[i]);                    // Set keyword    

    //         }


    //     }

    //     if (previousContextNum - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum) < 2) return;

    //     /// <summary>-------   Overlap with new keywords    ------- </summary> 
    //     int number = 0;

    //     foreach (string s in ContextKeywordList1)
    //     {


    //         for (int i = 0; i < SimpleContextKeywordsContainer2.childCount - ContextKeywordList1.Count; i++)
    //         {

    //             if (ColorAndShapeManager.instance.GetKeyword(SimpleContextKeywordsContainer2.GetChild(i).transform) == s)
    //             {

    //                 number++;
    //                 ColorAndShapeManager.instance.SwitchKeyword(SimpleContextKeywordsContainer2.GetChild(i).transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum) - number]);
    //                 // Debug.Log(number);
    //                 // Debug.Log(s);
    //                 // Debug.Log(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - (ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum) - number]);




    //             }
    //         }
    //     }

    //     if (previousContextNum - (ControlPanel.instance.NumberEachRow * 2) < 2) return;
    //     int number2 = 0;
    //     /// <summary>-------   Overlap with previous keywords    ------- </summary> 

    //     for (int i = 0; i < ControlPanel.instance.NumberEachRow * 2 - ContextKeywordList1.Count; i++)
    //     {

    //         string s = ColorAndShapeManager.instance.GetKeyword(SimpleContextKeywordsContainer2.GetChild(i).transform);

    //         for (int j = 0; j < ControlPanel.instance.NumberEachRow * 2 - ContextKeywordList1.Count; j++)
    //         {

    //             string k = ColorAndShapeManager.instance.GetKeyword(SimpleContextKeywordsContainer2.GetChild(j).transform);

    //             if (s == k && i != j)
    //             {

    //                 ColorAndShapeManager.instance.SwitchKeyword(SimpleContextKeywordsContainer2.GetChild(i).transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - (ControlPanel.instance.NumberEachRow * 2) - number2]);
    //                 number2++;

    //                 Debug.Log(number2);
    //                 Debug.Log(s);
    //                 Debug.Log(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - (ControlPanel.instance.NumberEachRow * 2) - number2]);
    //             }
    //         }
    //     }
    // }

    // public void previouspage()
    // {

    //     RelatedKeywordsManager.instance.stepNumber++;

    //     if (contextPageNumber == 0)
    //     {

    //         TemporaryContextList = new List<string>(HistoryContextKeywordsManager.instance.HistoryContextKeywordsList);
    //         TempPreviousTranscriptNums = new List<int>(PreviousTranscriptNums);
    //         Line1.gameObject.SetActive(true);
    //         Line1.gameObject.SetActive(true);

    //     }

    //     Line1.gameObject.SetActive(false);
    //     Line2.gameObject.SetActive(false);

    //     int previousContextNum = TemporaryContextList.Count;
    //     // int newContextKeywordNum = ContextKeywordList1.Count;
    //     //int firstPagePreviousNumber = ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum;
    //     //int RestPagePreviousNumber = HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - firstPagePreviousNumber;



    //     /// <summary>-------   Place new line of keywords    ------- </summary> 
    //     if (previousContextNum >= ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 2))
    //     {

    //         ModeManager.instance.ToggleContextPageDownButton(true);
    //         contextPageNumber += 1;

    //         for (int i = previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1); i < previousContextNum - ControlPanel.instance.NumberEachRow * 2 * contextPageNumber; i++)
    //         {

    //             int fromzero = i - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1));
    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(i - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1))).gameObject;
    //             obj.transform.GetComponent<Interactable>().transcriptNum = TempPreviousTranscriptNums[TempPreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1)) + fromzero];                                                                     // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i]);                       // Set keyword    

    //         }

    //     }

    //     else
    //     {

    //         ModeManager.instance.ToggleContextPageDownButton(true);
    //         ModeManager.instance.ToggleContextPageUpButton(false);

    //         if (previousContextNum > ControlPanel.instance.NumberEachRow * 2)
    //         {

    //             contextPageNumber += 1;

    //             for (int i = 0; i < previousContextNum - ControlPanel.instance.NumberEachRow * 2 * contextPageNumber; i++)
    //             {

    //                 int fromzero = i - previousContextNum - ControlPanel.instance.NumberEachRow * 2 * contextPageNumber;
    //                 GameObject obj = SimpleContextKeywordsContainer2.GetChild(i - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1))).gameObject;
    //                 obj.transform.GetComponent<Interactable>().transcriptNum = 1;                                                                         // Place transcript number
    //                 ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i]);                       // Set keyword    

    //             }

    //             for (int i = 0; i < ControlPanel.instance.NumberEachRow * 2 - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * contextPageNumber); i++)
    //             {

    //                 GameObject obj = SimpleContextKeywordsContainer2.GetChild(i).gameObject;
    //                 obj.transform.GetComponent<Interactable>().transcriptNum = 0;                                                                         // Place transcript number
    //                 ColorAndShapeManager.instance.SwitchKeyword(obj.transform, "");                       // Set keyword    

    //             }

    //         }

    //     }

    // }


    // public void Nextpage()
    // {

    //     RelatedKeywordsManager.instance.stepNumber++;

    //     int previousContextNum = HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count;
    //     // int newContextKeywordNum = ContextKeywordList1.Count;
    //     // int firstPagePreviousNumber = ControlPanel.instance.NumberEachRow * 2 - newContextKeywordNum;
    //     // int RestPagePreviousNumber = HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - firstPagePreviousNumber;



    //     /// <summary>-------   Place new line of keywords    ------- </summary> 
    //     if (contextPageNumber > 1)
    //     {

    //         ModeManager.instance.ToggleContextPageDownButton(true);
    //         ModeManager.instance.ToggleContextPageUpButton(true);
    //         contextPageNumber -= 1;

    //         for (int i = previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1); i < previousContextNum - ControlPanel.instance.NumberEachRow * 2 * contextPageNumber; i++)
    //         {

    //             int fromzero = i - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1));
    //             GameObject obj = SimpleContextKeywordsContainer2.GetChild(i - (previousContextNum - ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1))).gameObject;
    //             obj.transform.GetComponent<Interactable>().transcriptNum = TempPreviousTranscriptNums[TempPreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2 * (contextPageNumber + 1)) + fromzero];                                                                    // Place transcript number
    //             ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i]);                       // Set keyword    


    //         }

    //     }

    //     else if (contextPageNumber == 1)
    //     {

    //         ModeManager.instance.ToggleContextPageDownButton(false);
    //         contextPageNumber -= 1;
    //         placeCurrentFirstPageContextKeywords();
    //         Line1.gameObject.SetActive(true);
    //         Line2.gameObject.SetActive(true);

    //     }

    //     else
    //     {

    //     }


    // }


    // /// <summary>-------   Old sentence based context keywords    ------- </summary> 
    // public void placeSentenceContextKeywords(List<String> sentenceContextKeywordsList, int row, int transcript)
    // {

    //     if (sentenceContextKeywordsList.Count == 0) return;

    //     for (int i = 0; i < sentenceContextKeywordsList.Count; i++)
    //     {

    //         GameObject obj = ContextBubblePoolingManager.instance.GetContextBubble();

    //         obj.transform.GetComponent<Interactable>().transcriptNum = transcript;                                      // Place transcript number
    //         obj.SetActive(true);
    //         ColorAndShapeManager.instance.SwitchKeyword(obj.transform, sentenceContextKeywordsList[i]);                 // Set keyword                      
    //         // obj.transform.SetParent(FullContextKeywordsContainer); 
    //         obj.transform.SetParent(SimpleContextKeywordsContainer);
    //         obj.transform.localPosition = getPos(sentenceContextKeywordsList.Count, i, row);
    //         obj.transform.localRotation = Quaternion.identity;

    //     }

    // }

    // private Vector3 getPos(int keywordNum, int num, int row)
    // {

    //     float offset = (keywordNum - 1) * ControlPanel.instance.LargeBubbleAngularRadius + (keywordNum - 1) * 0.5f * ControlPanel.instance.ContextBubbleWidthSpacing;
    //     return new Vector3(-offset + num * (ControlPanel.instance.LargeBubbleAngularRadius * 2 + ControlPanel.instance.ContextBubbleWidthSpacing), 0 - ((row - 1) * (ControlPanel.instance.ContextBubbleHeightRadius * 2 + ControlPanel.instance.ContextBubbleHeightSpacing)), 0);

    // }


    // /// <summary>----------------------------------------------------------------- </summary> 
    // /// <summary>-------   Old Context keyword placement in simple mode    ------- </summary> 
    // /// <summary>----------------------------------------------------------------- </summary>
    // private void OldUpdateSimpleContextKeywords(List<string> newContextKeywordsList)
    // {                                                                           // Word-based placement

    //     ResetColor();

    //     if (newContextKeywordsList.Count < SimpleContextKeywordsContainer.childCount)
    //     {                                                                         // If spawned less than bubble UI number

    //         for (int i = 0; i < newContextKeywordsList.Count; i++)
    //         {

    //             // switchKeyword(SimpleContextKeywordsContainer.GetChild((lastModified) % SimpleContextKeywordsContainer.childCount), newContextKeywordsList[i]);
    //             var obj = SimpleContextKeywordsContainer.GetChild(i);

    //             ColorAndShapeManager.instance.SwitchKeyword(obj, newContextKeywordsList[i]);
    //             ColorAndShapeManager.instance.ChangeShapeMode(obj, 0);

    //             obj.gameObject.tag = "NCK";

    //             lastModified = (lastModified + 1) % SimpleContextKeywordsContainer.childCount;

    //         }

    //         for (int i = 0; i < SimpleContextKeywordsContainer.childCount - lastModified; i++)
    //         {

    //             if (HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count > SimpleContextKeywordsContainer.childCount - (SimpleContextKeywordsContainer.childCount - lastModified) + i)
    //             {

    //                 // switchKeyword(SimpleContextKeywordsContainer.GetChild(lastModified + i), HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count-(SimpleContextKeywordsContainer.childCount - lastModified)+i]);
    //                 ColorAndShapeManager.instance.SwitchKeyword(SimpleContextKeywordsContainer.GetChild(lastModified + i), HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - i - 1]);

    //             }

    //             else
    //             {

    //                 if (HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - i > 0)
    //                 {

    //                     //switchKeyword(SimpleContextKeywordsContainer.GetChild(lastModified + i), HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count-(SimpleContextKeywordsContainer.childCount - lastModified)+i]);
    //                     ColorAndShapeManager.instance.SwitchKeyword(SimpleContextKeywordsContainer.GetChild(lastModified + i), HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count - i - 1]);
    //                 }


    //             }

    //             // lastModified = ( (lastModified + 1) % SimpleContextKeywordsContainer.childCount );

    //         }

    //         lastModified = 0;

    //     }

    //     else
    //     {

    //         for (int i = 0; i < SimpleContextKeywordsContainer.childCount; i++)
    //         {

    //             ColorAndShapeManager.instance.SwitchKeyword(SimpleContextKeywordsContainer.GetChild(i), newContextKeywordsList[i]);

    //         }

    //     }

    // }

    // private void placeCurrentFirstPageContextKeywords()
    // {

    //     int previousContextNum = HistoryContextKeywordsManager.instance.HistoryContextKeywordsList.Count;

    //     ModeManager.instance.ToggleContextPageUpButton(true);

    //     /// <summary>-------   Place previous keywords    ------- </summary> 
    //     for (int i = previousContextNum - (ControlPanel.instance.NumberEachRow * 2); i < previousContextNum; i++)
    //     {

    //         int fromzero = i - (previousContextNum - (ControlPanel.instance.NumberEachRow * 2));
    //         GameObject obj = SimpleContextKeywordsContainer2.GetChild(fromzero).gameObject;
    //         // Debug.Log(PreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2) + i);
    //         obj.transform.GetComponent<Interactable>().transcriptNum = PreviousTranscriptNums[PreviousTranscriptNums.Count - (ControlPanel.instance.NumberEachRow * 2) + fromzero];                                 // Place transcript number
    //         ColorAndShapeManager.instance.SwitchKeyword(obj.transform, HistoryContextKeywordsManager.instance.HistoryContextKeywordsList[i]);                    // Set keyword    

    //     }

    //     // /// <summary>-------   Place new line of keywords    ------- </summary> 
    //     // for (int i = 0; i< newContextKeywordNum; i++ ) {

    //     //     GameObject obj = SimpleContextKeywordsContainer2.GetChild(SimpleContextKeywordsContainer2.childCount - newContextKeywordNum + i).gameObject;
    //     //     obj.transform.GetComponent<Interactable>().transcriptNum = transcriptNumber;                                      // Place transcript number
    //     //     ColorAndShapeManager.instance.SwitchKeyword(obj.transform, ContextKeywordList1[i]);                    // Set keyword    

    //     // }


    // }

    // private void ResetColor()
    // {

    //     foreach (Transform child in SimpleContextKeywordsContainer)
    //     {

    //         ColorAndShapeManager.instance.ChangeShapeMode(child, 0);
    //         child.tag = "Untagged";

    //     }

    // }
}
