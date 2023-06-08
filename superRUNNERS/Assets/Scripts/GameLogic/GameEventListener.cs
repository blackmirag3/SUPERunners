using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomEvent : UnityEvent<Component, object> {}

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomEvent response;

    private void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventCall(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
