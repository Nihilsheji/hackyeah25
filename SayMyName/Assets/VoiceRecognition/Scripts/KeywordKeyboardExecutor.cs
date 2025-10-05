using System;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeywordKeyboardExecutor : MonoBehaviour
{
    [SerializeField] private StringEvent keywordRecognizedEvent;

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("Pressed C Key");
            keywordRecognizedEvent.Raise("lust");
        }
        else if (Input.GetKey(KeyCode.V))
        {
            Debug.Log("Pressed V Key");
            keywordRecognizedEvent.Raise("gluttony");
        }
        else if (Input.GetKey(KeyCode.B))
        {
            Debug.Log("Pressed B Key");
            keywordRecognizedEvent.Raise("greed");
        }
    }
}
