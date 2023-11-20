using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GenKeywordBtn : MonoBehaviour {
  void Start() {
    Button btn = GetComponent<Button>();
    btn.onClick.AddListener(AddNextNode);
  }
  
  void AddNextNode() {
    MainManager.instance.AddNode(
      keyword: GetComponentInChildren<TMP_Text>().text,
      parent: NodeSphere.SelectedNode
    );
  }
}
