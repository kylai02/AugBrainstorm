using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GeneratedKeywordsManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]

  public int requestKeywordNumber = 8;

  public static GeneratedKeywordsManager instance;

  // --- For testing ---
  private List<string> preKeywords = new List<string>{"Music", "Genre", "Rock"};
  private List<string> conditions = new List<string>{"Event", "Outdoor"};
  private float execRate = 10.0f;
  private float nextExec = 0.0f;

  void Awake() {
    if (instance == null) instance = this;
  }

  void Update() {
    // DEBUG: For testing, execute every 10 seconds
    // if (Time.time > nextExec) {   
    //   nextExec = Time.time + execRate;
    //   updateGeneratedKeywords();
    // }
  }

  /// <summary>-------   Generate new keywords and place it   -------</summary>
  private async void updateGeneratedKeywords() {
    List<string> ExtractedGeneratedKeywords = 
      await OpenAI.OpenAI.instance.GetGeneratedKeywordsOpenAI(
        preKeywords,
        conditions, 
        requestKeywordNumber
      );

    if (ExtractedGeneratedKeywords == null || ExtractedGeneratedKeywords.Count == 0)
      return;

    // foreach (string s in ExtractedGeneratedKeywords) {
    //   Debug.Log(s);
    // }
  }
}
