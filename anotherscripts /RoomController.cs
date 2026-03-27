using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    [Header("Room UI Elements")]
    public Text letterDisplay;
    public Text instructionDisplay;

    private AdvancedRoomManager manager;
    private bool isRoom1; // whether it's Room1 or Room2
    private bool taskCompleted = false;

    public void Setup(string letter, AdvancedRoomManager mgr, bool room1)
    {
        manager = mgr;
        isRoom1 = room1;
        taskCompleted = false;

        if (letterDisplay != null)
            letterDisplay.text = letter;

        if (instructionDisplay != null)
        {
            instructionDisplay.text = isRoom1
                ? "Observe the letter, then go to the door →"
                : "Walk to the marker and press the correct button";
        }
    }

    // Detect when player reaches the exit door
    private void OnTriggerEnter(Collider other)
    {
        if (!isRoom1 && other.CompareTag("Player") && !taskCompleted)
        {
            taskCompleted = true;
            Debug.Log(" Trial completed — next trial starting...");
            manager.OnTrialCompleted();
        }
    }
}
