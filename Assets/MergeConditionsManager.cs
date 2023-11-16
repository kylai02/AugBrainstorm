using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeConditionsManager : MonoBehaviour
{
    public static MergeConditionsManager instance;

    // --- For testing ---
    private List<string> conditions = new List<string> { "Event", "Music" };
    private float execRate = 10.0f;
    private float nextExec = 0.0f;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Update()
    {
        // DEBUG: For testing, execute every 10 seconds
        if (Time.time > nextExec)
        {
            nextExec = Time.time + execRate;
            updateMergedConditionsKeywords();
        }
    }

    /// <summary>-------   Generate new keywords and place it   -------</summary>
    private async void updateMergedConditionsKeywords()
    {
        List<string> ExtractedMergedConditions =
          await OpenAI.OpenAI.instance.MergeConditionsOpenAI(
            conditions
          );

        if (ExtractedMergedConditions == null || ExtractedMergedConditions.Count == 0)
            return;

        foreach (string s in ExtractedMergedConditions)
        {
            Debug.Log(s);
        }
    }
}
