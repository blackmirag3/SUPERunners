using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitWaypoint : MonoBehaviour
{
    [SerializeField]
    private Vector3 exit, offset;
    [SerializeField]
    private Transform Player;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private Image waypointSprite;
    private TextMeshProUGUI distanceText;

    private float minX, maxX, minY, maxY;

    private bool waypointEnabled;

    private void Awake()
    {
        distanceText = waypoint.GetComponentInChildren<TextMeshProUGUI>();

        minX = waypointSprite.GetPixelAdjustedRect().width * 0.5f;
        maxX = Screen.width - minX;

        minY = waypointSprite.GetPixelAdjustedRect().height * 0.5f;
        maxY = Screen.width - minY;

        waypointEnabled = waypoint.activeSelf;
    }

    private void Update()
    {
        if (waypointEnabled)
        {
            UpdateWaypointPosition();
        }
    }

    private void UpdateWaypointPosition()
    {
        if (exit == null)
        {
            Debug.LogWarning("Exit vector is null");
            return;
        }
        Vector3 pos = Camera.main.WorldToScreenPoint(exit + offset);

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        if (pos.z < 0)
        {
            if (pos.x < Screen.width * 0.5f)
            {
                pos.x = maxX;
            }
            else
            {
                pos.x = minX;
            }
        }
        int distance = (int)Vector3.Distance(exit, Player.position);
        waypoint.transform.position = pos;
        distanceText.SetText($"{distance}m");
    }

    public void EnableWaypoint(Component sender, object data)
    {
        Debug.Log(exit);
        waypointEnabled = true;
        waypoint.SetActive(true);
    }

    public void DisableWaypoint(Component sender, object data)
    {
        waypointEnabled = false;
        waypoint.SetActive(false);
    }

    public void ExitPositionUpdated(Component sender, object data)
    {
        if (data is Vector3)
        {
            exit = (Vector3)data;
            return;
        }
        Debug.LogWarning($"Waypoint position not received from {sender}");
    }
}
