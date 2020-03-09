using UnityEngine.Events;
using UnityEngine;
using TMPro;

namespace Wreckless.UI
{
    public class Button : MonoBehaviour
    {

        public TextMeshProUGUI buttonTxt;
        public GameObject selectorIcon;

        public UnityEvent OnSelect = new UnityEvent();

        public void SetButtonTxt(string newTxt)
        {
            buttonTxt.text = newTxt;
        }

        public void SetSelectorActive(bool isActive)
        {
            selectorIcon.SetActive(isActive);
        }

        public void Select()
        {
            OnSelect.Invoke();
        }

    }
}