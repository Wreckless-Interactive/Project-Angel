using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{

    private bool inTrigger;


    private void Update()
    {
        if (inTrigger && Input.GetKeyDown(KeyCode.E))
            DoInteract();
    }

    protected abstract void DoInteract();

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            inTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            inTrigger = false;
    }

}
