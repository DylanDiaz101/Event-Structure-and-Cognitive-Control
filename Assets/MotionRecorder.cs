using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class MotionRecorder : MonoBehaviour
{
    public GameObject vrcam;
    private StreamWriter writer;
    private Queue<string> csvQueue = new Queue<string>();
    
    async void Start()
    {
        // Set up file path and StreamWriter
        string timestamp = System.DateTime.Now.ToString("s").Replace(":", "-");
        string path = Application.persistentDataPath + "/" + "motion_" + timestamp + ".csv";
        writer = new StreamWriter(path, false);
        writer.AutoFlush = false;
        
        // Write CSV header
        writer.WriteLine("frameIndex,timeStamp,posX,posY,posZ,orientX,orientY,orientZ,orientW");
        
        // Start processing the queue in the background
        ProcessQueue();
    }
    
    void LateUpdate()
    {
        // Capture motion data every frame
        int frameIndex = Time.frameCount;
        float timeStamp = Time.time;
        Vector3 position = vrcam.transform.position;
        Quaternion orientation = vrcam.transform.rotation;
        
        string csvLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}",
            frameIndex,
            timeStamp,
            position.x, position.y, position.z,
            orientation.x, orientation.y, orientation.z, orientation.w);
        
        // Enqueue the CSV line (lock if necessary for thread-safety)
        lock (csvQueue)
        {
            csvQueue.Enqueue(csvLine);
        }
    }
    
    async void ProcessQueue()
    {
        // Continually check for queued data and write it sequentially
        while (true)
        {
            if (csvQueue.Count > 0)
            {
                List<string> lines = new List<string>();
                
                // Drain the queue
                lock (csvQueue)
                {
                    while (csvQueue.Count > 0)
                    {
                        lines.Add(csvQueue.Dequeue());
                    }
                }
                
                // Write all lines at once
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
                
                // Await flush to ensure the data is written before next batch
                await writer.FlushAsync();
            }
            
            // Brief delay to prevent hogging the CPU—adjust as needed
            await Task.Delay(1);
        }
    }
    
    void OnApplicationQuit()
    {
        writer.Close();
    }
}

