using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathAction : ActionBase
{
    [SerializeField] private Player player;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameManager gameManager;

    public override void ExecuteAction(params ActionParameter[] parameters)
    {
        gameObject.SetActive(false);
        foreach (var weapon in player.GetWeapons())
        {
            weapon.gameObject.SetActive(false);
        }
        cameraController.enabled = false;
        uiManager.SetActiveDeathPanel();
        gameManager.SetActiveDeathReload();
    }
}
