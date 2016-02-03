using UnityEngine;
using System.Collections;

public class GameController
{
    private static GameController _instans;

    public static GameController GlobalGameController
    {
        get
        {
            if (_instans == null)
            {

                _instans = new GameController();
                return _instans;
            }
            else
            {

                return _instans;
            }
        }
        set
        {
            _instans = value;
        }
    }


    #region paramater
    //PlayerState playerState;

    #endregion 
    private GameController()
    {
        //playerState = PlayerState.GamePlayerState;
    }
    public void SaveGame()
    {   

    }
    public void LoadGame()
    {   

    }



}
