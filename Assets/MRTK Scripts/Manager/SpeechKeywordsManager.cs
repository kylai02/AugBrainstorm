using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;


public class SpeechKeywordsManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]

  public List<TMP_Text> speechKeywordsText;

  [HeaderAttribute("-------   temp public    ------- ")]
  public List<string> contextKeywords;

  // Singleton
  public static SpeechKeywordsManager instance;

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
    
    // Send keywords to UIManager
    foreach (string s in ExtractedContextKeywords) {
      contextKeywords.Add(s);
    }
  }
}
