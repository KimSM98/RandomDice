using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class TMPController : MonoBehaviour, ITextSetter
{
    public TextMeshProUGUI TMPText;

    public void SetText(string content)
    {
        TMPText.text = content;
    }
}
