using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MotionRecorder : MonoBehaviour
{
    public GameObject vrcam;
    private StreamWriter writer;

    void Start()
    {
        // Replace colons in the timestamp to avoid issues with file naming on some systems.
        string timestamp = System.DateTime.Now.ToString("s").Replace(":", "-");
        string path = Application.persistentDataPath + "/" + "motion_" + timestamp + ".csv";
        writer = new StreamWriter(path, false);
        writer.AutoFlush = false;

        // Write CSV header: frame index, timestamp, position (x, y, z), and orientation (x, y, z, w)
        writer.WriteLine("frameIndex,timeStamp,posX,posY,posZ,orientX,orientY,orientZ,orientW");
    }

    void LateUpdate()
    {
        int frameIndex = Time.frameCount;
        float timeStamp = Time.time;
        Vector3 position = vrcam.transform.position;
        Quaternion orientation = vrcam.transform.rotation;

        // Create CSV row using the position and orientation values.
        string csvLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
            frameIndex,
            timeStamp,
            position.x, position.y, position.z,
            orientation.x, orientation.y, orientation.z, orientation.w);

        writer.WriteLine(csvLine);
        writer.FlushAsync();
    }

    void OnApplicationQuit()
    {
        writer.Close();
    }
}

