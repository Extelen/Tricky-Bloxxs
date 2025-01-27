using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    //Properties
    /// <summary>
    /// Has the data system been initialized for the first time?
    /// </summary>
    public static bool Initialized { get; private set; }

    /// <summary>
    /// This property returns the file with the game data only if the player has selected the file to play.
    /// By default the last played file will be returned.
    /// </summary>
    public static GameData Game { get; set; }

    //Methods
    /// <summary>
    /// Method that saves all files into a persistent text file on the system.
    /// </summary>
    public static void Save()
    {
        if (Game == null)
            return;

        SaveManager.Save(Game);
    }

    /// <summary>
    /// Method that loads all files from the already created text file that is persistent on the system.
    /// </summary>
    public static void Load()
    {
        if (Initialized) return;

        Game = SaveManager.Load<GameData>();
        Initialized = true;
    }
}
