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
  public GameObject conditionBtnsField;

  [HeaderAttribute("-------   Prefab    ------- ")]
  public GameObject conditionBtnPrefab;

  [HeaderAttribute("-------   temp public    ------- ")]
  public List<string> speechKeywords;

  private List<GameObject> _condBtns;

  // Singleton
  public static SpeechKeywordsManager instance;

  void Awake() {
    if (instance == null) instance = this;
    _condBtns = new List<GameObject>();
  }

  void Start() {
    AddCondition("outdoor");
    AddCondition("game");
  }

  void Update() {
    // DEBUG: AddCondition
    if (Input.GetKeyDown(KeyCode.Space)) {
      AddCondition("outdoor");
    }

    updateContextKeywords();
    UpdateSpeechKeywordsText();
  }

  public void AddCondition(string condition) {
    MainManager.instance.AddCondition(condition);
    
    GameObject newCondBtn = Instantiate(conditionBtnPrefab);
    _condBtns.Add(newCondBtn);
    newCondBtn.transform.SetParent(conditionBtnsField.transform);
    newCondBtn.GetComponentInChildren<TMP_Text>().text = condition;

    newCondBtn.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
    newCondBtn.transform.localScale = new Vector3(2149.65f, 2149.65f, 2149.65f);

    AdjustConditionBtnsPos();
  }

  public void DeleteCondition(GameObject targetObj) {
    string targetStr = targetObj.GetComponentInChildren<TMP_Text>().text;
    MainManager.instance.DeleteCondition(targetStr);

    _condBtns.Remove(targetObj);
    Destroy(targetObj);
    AdjustConditionBtnsPos();
  }

  private void AdjustConditionBtnsPos() {
    Debug.Log("here");
    for (int i = 0; i < _condBtns.Count; ++i) {
      GameObject btn = _condBtns[i];

      btn.transform.localPosition = new Vector3(
        -400 + 200 * (i % 5),
        130 - 120 * (i / 5),
        0
      );
    }
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
      speechKeywords.Add(s);
    }
  }

  private void UpdateSpeechKeywordsText() {
    for (int i = 0; i < 8; ++i) {
      int targetIndex = speechKeywords.Count - 1 - i;

      if (targetIndex >= 0)
          speechKeywordsText[i].text = speechKeywords[targetIndex];
    }
  }
}
