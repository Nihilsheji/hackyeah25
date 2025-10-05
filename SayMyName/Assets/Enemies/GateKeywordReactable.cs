using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GateKeywordReactable : MonoBehaviour, IKeywordReactable
{
    public UnityEvent KeywordRecognized;

    public void OnKeywordRecognized()
    {
        KeywordRecognized.Invoke();
    }
}
