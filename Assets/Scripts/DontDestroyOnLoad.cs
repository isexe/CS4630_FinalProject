using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is used to keep singleton of GameController between scenes
*  May replace later as this causes a whole bunch of other issues that aren't worth the effort
*  These issues are 'fixed' by setting all gameobjects referenced by the GameController as children
*  Could just look for the gameobjects and set in script, but for now it's good enough.
*/
public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if(objs.Length > 1){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
