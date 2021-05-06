using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other is TerrainCollider)
            SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.FOOTSTEP);
    }
}
