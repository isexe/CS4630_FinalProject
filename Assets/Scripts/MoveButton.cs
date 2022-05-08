using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject button = GameObject.Find("RespawnButton");
        button.transform.position = this.gameObject.transform.position;
    }
}
