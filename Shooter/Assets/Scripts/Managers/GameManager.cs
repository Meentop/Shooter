using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private InfoInterface infoInterface;
    [SerializeField] private DinemicInterface dinemicInterface;
    private void Awake()
    {
        player.Init(infoInterface, dinemicInterface);
    }
}
