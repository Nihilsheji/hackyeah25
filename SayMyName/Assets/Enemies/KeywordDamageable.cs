using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeywordDamageable : MonoBehaviour, IKeywordReactable
{
    public void OnKeywordRecognized()
    {
        Destroy(gameObject);
    }
}
