using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICollectible
{
    void OnPickup();
    bool isEquipped {get; set;}
}