using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// DESCRIPTION:
// Script for generating trial list and order. 

// USAGE:
// 1. Add script to empty gameobject
// 2. Input trial counts in the inspector. Default: AX shift: 42, AX no shift: 42, 6, 6, 6, 6, 6, 6

public class Trial
{
    public string Condition;
    public char Cue;
    public char Probe;
}

public class TrialGenerator : MonoBehaviour
{
    // Trial counts to be inputted in inspector
    [SerializeField] private int axShiftCount = 42;
    [SerializeField] private int axNoShiftCount = 42;
    [SerializeField] private int bxShiftCount = 6;
    [SerializeField] private int bxNoShiftCount = 6;
    [SerializeField] private int ayShiftCount = 6;
    [SerializeField] private int ayNoShiftCount = 6;
    [SerializeField] private int byShiftCount = 6;
    [SerializeField] private int byNoShiftCount = 6;

    private static System.Random rng = new System.Random();
    private static List<char> allLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList();

    // Allowed letters for BX trials: any letter except A, Y, and K.
    private List<char> GetAllowedLettersForBX()
    {
        return allLetters.Where(c => c != 'A' && c != 'Y' && c != 'K').ToList();
    }

    // Allowed letters for AY probe: any letter except A, X, Y, and K.
    private List<char> GetAllowedLettersForAY()
    {
        return allLetters.Where(c => c != 'A' && c != 'X' && c != 'Y' && c != 'K').ToList();
    }

    // Allowed letters for BY probe: any letter except X, Y, K, and the selected cue letter.
    private List<char> GetAllowedLettersForBYProbe(char cue)
    {
        return allLetters.Where(c => c != 'X' && c != 'Y' && c != 'K' && c != cue).ToList();
    }

    // Returns a random element from the provided list.
    private T GetRandomItem<T>(List<T> list)
    {
        int index = rng.Next(list.Count);
        return list[index];
    }

    public List<Trial> GenerateTrials()
    {
        List<Trial> trials = new List<Trial>();

        // Build trial list:

        trials.AddRange(Enumerable.Repeat("AX_shift",   axShiftCount)   .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("AX_no_shift",axNoShiftCount) .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("BX_shift",   bxShiftCount)   .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("BX_no_shift",bxNoShiftCount) .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("AY_shift",   ayShiftCount)   .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("AY_no_shift",ayNoShiftCount) .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("BY_shift",   byShiftCount)   .Select(cond => new Trial { Condition = cond }));
        trials.AddRange(Enumerable.Repeat("BY_no_shift",byNoShiftCount) .Select(cond => new Trial { Condition = cond }));

        // Shuffle the trials list.
        trials = trials.OrderBy(t => rng.Next()).ToList();

        // Assign cue and probe letters according to each condition's rules.
        foreach (Trial trial in trials)
        {
            if (trial.Condition.StartsWith("AX"))
            {
                // AX: Cue is always 'A' and Probe is always 'X'.
                trial.Cue = 'A';
                trial.Probe = 'X';
            }
            else if (trial.Condition.StartsWith("AY"))
            {
                // AY: Cue is 'A'; Probe is randomly selected from letters excluding A, X, Y, K.
                trial.Cue = 'A';
                List<char> allowedForAY = GetAllowedLettersForAY();
                trial.Probe = GetRandomItem(allowedForAY);
            }
            else if (trial.Condition.StartsWith("BX"))
            {
                // BX: Probe is 'X'; Cue is randomly selected from letters excluding A, Y, K.
                List<char> allowedForBX = GetAllowedLettersForBX();
                trial.Cue = GetRandomItem(allowedForBX);
                trial.Probe = 'X';
            }
            else if (trial.Condition.StartsWith("BY"))
            {
                // BY: Cue is randomly selected from letters excluding A, Y, K; Probe is chosen from letters excluding X, Y, K, and the cue.
                List<char> allowedForBX = GetAllowedLettersForBX();
                trial.Cue = GetRandomItem(allowedForBX);
                List<char> allowedForBY = GetAllowedLettersForBYProbe(trial.Cue);
                trial.Probe = GetRandomItem(allowedForBY);
            }
            else
            {
                Debug.LogError("Unknown trial condition encountered: " + trial.Condition);
            }
        }

        return trials;
    }

    // For demonstration purposes, store the trial list here.
    public List<Trial> trialList;

    // Generate and print trials on Start.
    private void Start()
    {
        trialList = GenerateTrials();

        // Debug output to verify your trial generation. Remove or comment out for production.
        foreach (Trial trial in trialList)
        {
            Debug.Log($"Room: {trial.Condition}, Cue: {trial.Cue}, Probe: {trial.Probe}");
        }
    }
}
