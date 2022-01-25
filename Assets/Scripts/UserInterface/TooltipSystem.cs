using System.Collections;
using UnityEngine;

namespace UserInterface
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem _current;
        public ToolTip tooltip;
        public void Awake()
        {
            _current = this;
        }

        public static void Show(string content, string header = "")
        {
            _current.tooltip.SetText(content, header);
            _current.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            _current.tooltip.gameObject.SetActive(false);
        }
        public static IEnumerator TooltipHideBuffer(float bufferDuration)
        {
            yield return new WaitForSeconds(bufferDuration);
            _current.tooltip.gameObject.SetActive(false);
        }
    }
}
