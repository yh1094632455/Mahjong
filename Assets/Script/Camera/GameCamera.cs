using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{

    private Vector3 mouse_position;

    private void Start()
    {
        mouse_position = Input.mousePosition;
    }

    void Update()
    {
        if (Vector3.Distance(mouse_position, Input.mousePosition) > 0.1)
        {
            Vector3 dis = (mouse_position - Input.mousePosition) * 0.1F;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + dis.y, transform.rotation.eulerAngles.y - dis.x, 0);
            mouse_position = Input.mousePosition;
        }
    }
}
