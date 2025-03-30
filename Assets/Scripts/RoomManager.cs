using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DESCRIPTION: Handles room order using the generated trial list.

// USAGE:
// 1. Apply to an empty gameobject.
// 2. Drag and drop TrialGenerator game object (that has the trial generator script on it) into the empty field in inspector.

public class RoomManager : MonoBehaviour
{
    [SerializeField] private TrialGenerator trialGenerator;

    // Start is called before the first frame update
    void Start()
    {
        // access the list
        List<Trial> retrievedTrials = trialGenerator.trialList;

        // do something here
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
