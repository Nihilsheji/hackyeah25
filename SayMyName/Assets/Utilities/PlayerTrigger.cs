using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private bool IsOneShot;

    public UnityEvent OnPlayerEnteredTrigger;
    public UnityEvent OnPlayerExitedTrigger;

    private bool _isOneShotDone;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            OnPlayerEnteredTrigger.Invoke();

            if(IsOneShot == true)
            {
                _isOneShotDone = true;
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(_isOneShotDone == true)
            return;

        if(other.tag == "Player")
        {
            OnPlayerExitedTrigger.Invoke();
        }
    }


}
