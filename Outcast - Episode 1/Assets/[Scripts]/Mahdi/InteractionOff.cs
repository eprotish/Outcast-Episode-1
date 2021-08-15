using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionOff : MonoBehaviour
{

    public GameDataController gamedata;
    private UiController uiController;

    public void Awake()
    {
        gamedata = GameObject.FindObjectOfType<GameDataController>();
        uiController = GameObject.FindObjectOfType<UiController>();
    }

    public void OnEnable()
    {

        if (gamedata.gameData.isOnCanvas)
              return;
        
        uiController.ShowOff();
        gamedata.gameData.isOnCanvas = true;

    }

    public void OnDisable()
    {
        if (!gamedata.gameData.isOnCanvas)
            return;

        uiController.Show();
        gamedata.gameData.isOnCanvas = false;

    }
}
