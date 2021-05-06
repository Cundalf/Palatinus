using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public ObjectiveZoneController objetiveController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objetiveController.endGameZone();
            other.gameObject.GetComponent<Player>().objetiveComplete();
        }
    }

}
