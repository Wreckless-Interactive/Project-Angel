using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
