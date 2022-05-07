using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 30f;
    private Rigidbody player;
    public bool alive;

    // Sets player as alive and snags rigidbody for setting velocity
    void Start()
    {
        player = this.gameObject.GetComponent<Rigidbody>();
        alive = true;    
    }

    // Calls fixed update to set the velocity of the player
    void FixedUpdate()
    {
        // only move if alive
        if(alive){
            // Grab mouse position in as world position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // zero out the z
            mousePos.z = 0;
            // do some vector math to calculate the wanted direction
            Vector3 dir = mousePos - this.transform.position;
            // calc velocity equal to speed in wanted direction
            Vector3 vel = dir * speed;

            // look at mouse to improve looks
            this.gameObject.transform.LookAt(mousePos, Vector3.up);
            // set velocity
            player.velocity = vel;
        }
    }

    // Checks if player touches something
    void OnTriggerEnter(Collider obj){
        // switches based on what the player touches
        switch (obj.tag)
        {
            // if bad something, punish
            case "Enemy":
                // lets gamecontroller do heavy lifting
                GameController.S.HitEnemy();
                // play dead
                IsDead();
                break;

            // if good something, reward
            case "Balloon":
                // lets GameController do more heavy lifting
                // passes obj for calculating point value and destroying
                GameController.S.HitBalloon(obj.gameObject);
                break;
        }
    }

    // stops the player and prevents moving
    void IsDead(){
        alive = false;
        player.velocity = Vector3.zero;
    }
}
