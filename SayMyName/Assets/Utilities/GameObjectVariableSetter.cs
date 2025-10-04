using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class GameObjectVariableSetter : MonoBehaviour
{

    [SerializeField] private GameObjectVariable variable;

    private void Awake()
    {
        variable.Value = gameObject;
    }
}
