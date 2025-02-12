using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TriggerPad : MonoBehaviour
{
    private string filePath;
    private int touchedCube1 = 0;

    void Start()
    {
        // Define the path where the CSV file will be saved.
        filePath = Path.Combine(Application.persistentDataPath, "playerData.csv");

        // If the file doesn't exist, create it with a header.
        if (!File.Exists(filePath))
        {
            string header = "Timestamp,touchedCube1";
            File.WriteAllText(filePath, header + Environment.NewLine);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchedCube1++; // Increment counter
            Debug.Log("Player touched the cube! Count: " + touchedCube1);

            // Log the event to the CSV file
            LogToCSV();
        }
    }

    private void LogToCSV()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string dataLine = $"{timestamp},{touchedCube1}";

        File.AppendAllText(filePath, dataLine + Environment.NewLine);
    }
}
