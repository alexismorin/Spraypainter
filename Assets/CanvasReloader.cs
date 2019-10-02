using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasReloader : MonoBehaviour
{
    public GameObject newInstance;
    public void Reload()
    {
        Destroy(GameObject.Find("Door"));
        Instantiate(newInstance, Vector3.zero, Quaternion.identity);
    }
}
