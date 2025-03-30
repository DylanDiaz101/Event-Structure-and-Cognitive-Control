using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DESCRIPTION:
// Plays footsteps audio whenever player pushes WASD

// USAGE:
// 1. Add to player gameobject.
// 2. Add footsteps audiosource to the field in the inspector.

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSound;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            footstepsSound.enabled = true;
        }
        else {
            footstepsSound.enabled = false;
        }
    }
}
