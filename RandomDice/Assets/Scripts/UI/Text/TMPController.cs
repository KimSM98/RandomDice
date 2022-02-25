using UnityEngine;
using TMPro;

public class TMPController : MonoBehaviour, ITextSetter
{
    public TextMeshProUGUI TMPText;

    public void SetText(string content)
    {
        TMPText.text = content;
    }
}
