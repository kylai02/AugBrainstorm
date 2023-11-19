using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSphere : MonoBehaviour {
  public string keyword;
  public NodeSphere parent;
  public List<NodeSphere> children;

  public Material unselectedMat;
  public Material selectedMat;

  public static NodeSphere SelectedNode;

  void Awake() {
    children = new List<NodeSphere>();
  }

  void Start() {
    // Button btn = GetComponent<Button>() ?? null;
    // if (btn) {
    //   btn.onClick.AddListener(AddNodeToSelectedNode);
    // }
  }

  public void SelectNode() {
    if (SelectedNode != this && SelectedNode != null) {
      SelectedNode.gameObject.GetComponent<MeshRenderer>().material = unselectedMat;
      gameObject.GetComponent<MeshRenderer>().material = selectedMat;
    }
    else if (SelectedNode == null) {
      gameObject.GetComponent<MeshRenderer>().material = selectedMat;
    }

    // Update selectedNode
    SelectedNode = this;
    MainManager.instance.ChoseSelectedNode();

    Debug.Log("onClick");
  }
}
