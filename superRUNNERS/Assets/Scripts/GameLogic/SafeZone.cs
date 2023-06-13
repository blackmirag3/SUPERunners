using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public GameControl game;

    [SerializeField] private string playerTag = "Player";

    private void OnTriggerExit(Collider other)
    {
        if (!game.GameInProgress && other.gameObject.CompareTag(playerTag))
        {
            game.StartGame();
            enabled = false;
        }
    }
}
