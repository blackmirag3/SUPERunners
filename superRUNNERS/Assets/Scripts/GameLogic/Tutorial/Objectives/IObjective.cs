using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IObjective
{
    void CheckProgress();
    bool isComplete {get; set;}
}
