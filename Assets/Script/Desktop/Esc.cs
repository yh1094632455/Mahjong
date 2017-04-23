using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esc : MonoBehaviour
{

    public GameObject[] list;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (GameObject o in list)
            {
                o.SetActive(!o.GetActive());
            }
        }
    }
}
