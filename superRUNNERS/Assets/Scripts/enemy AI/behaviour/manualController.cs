using UnityEngine;
using UnityEngine.AI;

public class manualController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Physics.Raycast(ray, out hit); //if raycast hit
            {
                //move agent
                agent.SetDestination(hit.point);
            }
        }
    }
}
