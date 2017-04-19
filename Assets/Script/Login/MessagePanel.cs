using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel : MonoBehaviour
{
    public Text text;
    private Vector3 HidePosition;
    private Vector3 ShowPosition;

    private void Start()
    {
        HidePosition = transform.position;
        ShowPosition = HidePosition + new Vector3(0, -40, 0);
    }

    public void ShowMsg(string msg)
    {
        text.text = msg;
        iTween.MoveTo(gameObject, ShowPosition, 0.5f);
        StartCoroutine(Hide(3f));
    }

    public IEnumerator Hide(float time)
    {
        yield return new WaitForSeconds(3f);
        iTween.MoveTo(gameObject, HidePosition, 0.5f);
    }
}
