using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SpeechKeywordBtn : MonoBehaviour {
  private TMP_Text _text;
  
  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(AddTextToSelectedWord);

    _text = GetComponentInChildren<TMP_Text>();
  }

  // Add select keyword to SKM
  private void AddTextToSelectedWord() {
    if (_text.text == "") return;
    
    SpeechKeywordsManager.instance.AddCondition(_text.text);
  }
}
