using UnityEngine;

public class AdvancedRoomManager : MonoBehaviour
{
    [Header("Room Prefabs (assign in Inspector)")]
    public GameObject room1Prefab;
    public GameObject room2Prefab;

    [Header("Settings")]
    [Tooltip("Total number of trials to simulate (e.g., 50, 80, 120)")]
    public int totalTrials = 120;
    [Tooltip("Distance between rooms along Z axis")]
    public float roomSpacing = 25f;

    private GameObject activeRoom1;
    private GameObject activeRoom2;
    private int currentTrial = 0;
    private bool room1Active = true;  // Toggles which room is next

    private LetterManager letterManager;

    void Start()
    {
        // Initialize letter generator
        letterManager = GetComponent<LetterManager>();
        if (letterManager == null)
        {
            letterManager = gameObject.AddComponent<LetterManager>();
        }

        // Spawn the first two rooms
        SpawnInitialRooms();
    }

    void SpawnInitialRooms()
    {
        activeRoom1 = Instantiate(room1Prefab, Vector3.zero, Quaternion.identity);
        activeRoom2 = Instantiate(room2Prefab, new Vector3(0, 0, roomSpacing), Quaternion.identity);

        // Assign controllers
        var r1Ctrl = activeRoom1.GetComponent<RoomController>();
        var r2Ctrl = activeRoom2.GetComponent<RoomController>();

        var letters = letterManager.GetLettersForTrial(currentTrial);
        r1Ctrl.Setup(letters.Item1, this, true);
        r2Ctrl.Setup(letters.Item2, this, false);
    }

    // Called by Room2 when the player exits
    public void OnTrialCompleted()
    {
        currentTrial++;
        if (currentTrial >= totalTrials)
        {
            Debug.Log(" All " + totalTrials + " trials completed!");
            return;
        }

        // Move the inactive room ahead and update its content
        if (room1Active)
        {
            MoveAndRefreshRoom(activeRoom1, activeRoom2.transform.position.z + roomSpacing);
        }
        else
        {
            MoveAndRefreshRoom(activeRoom2, activeRoom1.transform.position.z + roomSpacing);
        }

        room1Active = !room1Active;
    }

    private void MoveAndRefreshRoom(GameObject room, float newZ)
    {
        // Move the room
        room.transform.position = new Vector3(0, 0, newZ);

   
        var ctrl = room.GetComponent<RoomController>();
        var letters = letterManager.GetLettersForTrial(currentTrial);
        ctrl.Setup(letters.Item1, this, room1Active); // alternate which is first
    }
}
