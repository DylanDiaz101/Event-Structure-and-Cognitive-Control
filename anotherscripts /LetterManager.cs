using UnityEngine;

public class LetterManager : MonoBehaviour
{
    private string[] firstLetters = { "A", "B" };
    private string[] secondLetters = { "X", "Y" };

    public (string, string) GetLettersForTrial(int index)
    {
        // Cycle through combinations A/X, A/Y, B/X, B/Y, then repeat
        int i = index % firstLetters.Length;
        int j = (index / firstLetters.Length) % secondLetters.Length;
        return (firstLetters[i], secondLetters[j]);
    }
}
