using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Variables

    // Constructor
    public GameData()
    {

    }

    // Methods





    #region Examples
    // Variables
    // [SerializeField]                         // Needs to be serialized to save in a file
    // private List<UserData> m_users;          // Private for encapsulation
    // public List<UserData> Users              // A property only to access to the variable and
    // {                                        //  not to write in it.
    //     get
    //     {                                    // In this case, is a List of UserData, and inside
    //         return m_users;                  // of the [System.Serializable] UserData class, we can
    //     }                                    // put more variables and more classes to save
    // }                                        // in a correct order, for example:
    //                                          // GameData -> InventoryData -> WeaponsData -> Weapon1

    // [SerializeField]
    // private int m_currentUserIndex;          // In this case is only a int variable
    // public int CurrentUserIndex              // If you want to negate the access to write, remove the set.
    // {                                        // This can be a public variable but i always make private 
    //     get                                  // variables and get; set; properties to make a correct encapsulation
    //     {
    //         return m_currentUserIndex;
    //     }

    //     set
    //     {
    //         m_currentUserIndex = value;
    //     }
    // }

    // Constructors
    // public GameData()                        // Inside the constructor we can initialize the values
    // {                                        
    //     m_users = new List<UserData>();      // The list always needs to be initialized
    //     m_currentUserIndex = 0;              // And the variables can have a default value, ex, gold = 100;
    // }

    // Methods
    /// <summary>
    /// Get the current user or get a debug one.
    /// </summary>
    // public UserData GetCurrentUser()                     // And, the idea is put methods to access to 
    // {                                                    // different functionalities of the saved variables
    //     if (m_currentUserIndex == 0)                     // In this case, i only try to get a user 
    //     {                                                // But for example, in the UserData class, i can put
    //         if (m_users.Count == 0)                      // a method that changes the name of the user.
    //             m_users.Add(new UserData("DEBUG"));      

    //         return m_users[0];
    //     }

    //     return m_users[m_currentUserIndex];
    // }
    #endregion
}
