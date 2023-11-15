using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class GeneratedIdeasManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Reference    ------- ")]
  [SpaceAttribute(10)]

  public int requestIdeaNumber = 3; // how many ideas to generate
  public int requestDescriptionLen = 12;    // the length limit of each description

  public static GeneratedIdeasManager instance;

  // --- For testing ---
  private List<string> selectedKeywords = new List<string>{"Music", "Guitar", "Band"};
  private float execRate = 10.0f;
  private float nextExec = 0.0f;

  void Awake() {
    if (instance == null) instance = this;
  }

  void Update() {
    // DEBUG: For testing, execute every 10 seconds
    // if (Time.time > nextExec) {   
    //   nextExec = Time.time + execRate;
    //   updateGeneratedIdeas();
    // }
  }

  /// <summary>-------   Generate new keywords and place it   -------</summary>
  private async void updateGeneratedIdeas() {
    List<string> IdeaNames = new List<string>();    // names of ideas
    List<string> Descriptions = new List<string>(); // descriptions of ideas

    List<string> ExtractedGeneratedIdeas = 
      await OpenAI.OpenAI.instance.GetGeneratedIdeasOpenAI(
        selectedKeywords,   // selected keywords
        requestIdeaNumber,  // how many ideas to generate
        requestDescriptionLen   // the length limit of each description
      );

    if (ExtractedGeneratedIdeas == null || ExtractedGeneratedIdeas.Count == 0)
      return;

    for (int i = 0 ; i < ExtractedGeneratedIdeas.Count ; i++)
    {
        if (i % 2 == 0) // even: idea name
        {
            IdeaNames.Add(ExtractedGeneratedIdeas[i]);
        }
        else    // odd: description
        {
            Descriptions.Add(ExtractedGeneratedIdeas[i]);
        }
    }

    // // for testing
    // Debug.Log("Ideas and descriptions:");
    // for (int i = 0 ; i < IdeaNames.Count ; i++)
    // {
    //     Debug.Log(IdeaNames[i] + "\n" + Descriptions[i]);
    // }
  }
}
