using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageOnDrawGizmosSelected : MonoBehaviour
{
    [SerializeField] GameObject target;
    private void OnDrawGizmosSelected() 
    {
        if (target == null) return;
        target.SendMessage("OnDrawGizmosSelected");
    }
}
