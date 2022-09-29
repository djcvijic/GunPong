using System.Collections;
using TMPro;
using UnityEngine;

namespace View.UI
{
    public class CalloutUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageLabel;

        public IEnumerator Show(float duration, string message)
        {
            gameObject.SetActive(true);
            messageLabel.text = message;

            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }
    }
}