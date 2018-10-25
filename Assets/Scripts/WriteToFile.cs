using UnityEngine;
using System.Collections;
using System.IO;

public class WriteToFile : MonoBehaviour {


    StreamWriter sr;
    string playerName;
    
    public void prepareFile() {
        string logPath = "";
        playerName = PlayerPrefs.GetString("CurrentPlayer");

        if (Application.platform == RuntimePlatform.Android)
        {
            logPath = Application.persistentDataPath;
            logPath += "/" + PlayerPrefs.GetString("CurrentPlayer") + ".txt";

            if (File.Exists(logPath))
            {
                sr = File.AppendText(logPath);
            }
            else
            {
                sr = File.CreateText(logPath);
            }

        }
        else
        {
            if (File.Exists(playerName + ".txt"))
            {
                sr = File.AppendText(playerName + ".txt");
            }
            else
            {
                sr = File.CreateText(playerName + ".txt");
            }
        }
    }

    public void writeToFile(string text) {
        sr.WriteLine(text);
    }

    public void closeFile() {
        sr.Close();
    }
}

