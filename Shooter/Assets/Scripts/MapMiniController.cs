using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMiniController : MonoBehaviour
{
    [SerializeField] private GameObject miniRoomPrefab;
    [SerializeField] private MapMiniController mapMiniController;
    [SerializeField] private Vector2 minimapSize;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SpawnMiniMap(Vector3 spawnPosition)
    {
        GameObject newMinimapRoom = Instantiate(miniRoomPrefab, mapMiniController.transform);
        newMinimapRoom.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.z, 0);
        newMinimapRoom.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }
}
