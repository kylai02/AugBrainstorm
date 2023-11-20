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

  private LineRenderer _lineRenderer;

  void Awake() {
    children = new List<NodeSphere>();
  }

  void Start() {
    _lineRenderer = GetComponent<LineRenderer>();
    _lineRenderer.positionCount = 2;
    // Button btn = GetComponent<Button>() ?? null;
    // if (btn) {
    //   btn.onClick.AddListener(AddNodeToSelectedNode);
    // }
  }

  void Update() {
    if (parent.keyword != "root") {
      _lineRenderer.SetPosition(0, transform.position);
      _lineRenderer.SetPosition(1, parent.transform.position);
    }
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
