using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TranslatableUIText : MonoBehaviour
{
    public TranslatableString translatableString;

	Text text;
	TextMeshProUGUI textMeshPro;

	public void Start()
    {
		textMeshPro = GetComponent<TextMeshProUGUI>();
		text = GetComponent<Text>();

		if (textMeshPro)
			textMeshPro.text = translatableString;
		else
			text.text = translatableString;
        //GetComponent<UnityEngine.UI.Text>().text = translatableString;
    }
}
