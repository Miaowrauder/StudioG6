using TMPro;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreboardMngr : MonoBehaviour
{
    private FileInfo saveFile = null;
    private string scores = " ";
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        SaveScore("Bob", 5);
        RetrieveScores();
    }
    void RetrieveScores()
    {
        saveFile = new FileInfo("PlayerScores.txt");
        int scoreNo = 1;
        using (StreamReader sr = saveFile.OpenText())
        {
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                scoreText.text += scoreNo + ": " + line + "\n";
                scoreNo++;
            }
        }
    }
    void SaveScore(string name, int score)
    {
        StreamWriter SW = new StreamWriter("PlayerScores.txt", true);
        SW.Write(name + " : " + score);
        SW.Close();
    }
}
