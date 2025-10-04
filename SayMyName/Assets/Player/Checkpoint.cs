using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int thisCheckpointIndex;
    [SerializeField] private IntVariable savedCheckpointIndex;

    public void SaveCheckpointIndex()
    {
        savedCheckpointIndex.Value = thisCheckpointIndex;
    }
}

