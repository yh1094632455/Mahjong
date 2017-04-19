
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceCamera : MonoBehaviour
{

    public Animator anim;

    private Vector3 mouse_position;

    private void Start()
    {
        mouse_position = Input.mousePosition;

        iTween.MoveBy(gameObject, iTween.Hash("y", 200, "time", 5, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", 0));
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

    public void GetInRoom()
    {
        GetComponent<iTween>().enabled = false;
        anim.enabled = true;

        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadSceneAsync("Game");
    }
}
