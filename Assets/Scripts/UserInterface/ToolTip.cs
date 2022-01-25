using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    [ExecuteInEditMode()]
    public class ToolTip : MonoBehaviour
    {
        public TextMeshProUGUI headerField;
        public TextMeshProUGUI contentField;
        public LayoutElement layoutElement;
        public int characterWrapLimit;

        
        //Text to be set in the tooltip, and triggering it on and off.
        public void SetText(string content, string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerField.gameObject.SetActive(false);
            }
            else
            {
                headerField.gameObject.SetActive(true);
                headerField.text = header;
            }

            contentField.text = content;
        }

        private void Update()
        {
            if (Application.isEditor)
            {
                int headerLength = headerField.text.Length;
                int contentLength = contentField.text.Length;

                layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
            }
        }
    }
}
