using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Desktop
{
    public class Timer : MonoBehaviour
    {

        public Image image;

        public int time = 10;

        public void Show()
        {
            image.enabled = true;
            StartCoroutine(ShowTimer(time));
        }

        private IEnumerator ShowTimer(int time)
        {
            if (time <= 0)
            {
                //ActiveNext;
                Hide();
                //PhotonNetwork.RaiseEvent(5, null, true, null);
                yield break;
            }

            yield return new WaitForSeconds(1f);

            this.transform.Rotate(new Vector3(0, 0, 1));
            time--;

            StartCoroutine(ShowTimer(time));
        }

        internal void Hide()
        {
            image.enabled = false;
        }
    }
}