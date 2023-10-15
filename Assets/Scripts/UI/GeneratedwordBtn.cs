using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GeneratedwordBtn : MonoBehaviour {
  // Start is called before the first frame update
  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(AddNextNode);
  }

  void AddNextNode() {
    UIManager.instance.NewKeywordNode(
      GetComponentInChildren<TMP_Text>().text,
      UIManager.instance.selectedNode
    );
  }
}
