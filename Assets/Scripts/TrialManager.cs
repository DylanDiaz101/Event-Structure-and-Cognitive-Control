using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrialManager : MonoBehaviour
{
    public TrialGenerator generator;

    public TextMeshPro cueText;
    public TextMeshPro probeText;

    private List<Trial> trials;
    private int currentIndex = 0;
    private Trial currentTrial;

    void Start()
    {
        trials = generator.trialList;
        StartNextTrial();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Show Cue");
            ShowCue();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Show Probe");
            ShowProbe();
        }
    }

    public void StartNextTrial()
    {
        if (currentIndex >= trials.Count)
        {
            Debug.Log("Experiment Finished");
            return;
        }

        currentTrial = trials[currentIndex];
        currentIndex++;

        cueText.text = "";
        probeText.text = "";
    }

    public void ShowCue()
    {
        cueText.text = currentTrial.Cue.ToString();
    }

    public void ShowProbe()
    {
        probeText.text = currentTrial.Probe.ToString();
    }
}