using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CondBtn : MonoBehaviour {
  private TMP_Text _text;

  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(DeleteThisCondition);
  }

  public void DeleteThisCondition() {
    SpeechKeywordsManager.instance.DeleteCondition(gameObject);
  }
}