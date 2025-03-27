using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Game { 
    public int highScore;

    public Game()
    {
        highScore = 0;
    }
}
public static class Saving {
    public static Game savedGames = new Game();
			
    //it's static so we can call it from anywhere 
    public static void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located 
        FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want 
        bf.Serialize(file, Saving.savedGames);
        file.Close();
    }	
	
    public static void Load() {
        if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            Saving.savedGames = (Game)bf.Deserialize(file);
            Debug.Log(Saving.savedGames.highScore);
            file.Close();
        }
        else
        {
            Save();
        }
    }
    
    public static bool OverrideIfHigher(int score) {
        if (score > Saving.savedGames.highScore) {
            Saving.savedGames.highScore = score;
            Save();
            return true;
        }
        return false;
    }
}