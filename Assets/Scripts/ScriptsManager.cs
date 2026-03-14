using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsManager : MonoBehaviour
{
    [SerializeField] 
    private List<SampleScript> sampleScripts;

    [ContextMenu("Use All Scripts")]
    public void UseAll()
    {
        foreach (var script in sampleScripts)
        {
            if (script != null)
                script.Use();
        }
    }
}
