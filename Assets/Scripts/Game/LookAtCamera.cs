using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera cameraToLookAt;

    private void Start()
    {
        cameraToLookAt = Camera.main;
        transform.forward = cameraToLookAt.transform.forward;
    }

}
