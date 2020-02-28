using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TranslatableString
{
	public const int LANG_EN = 0;
	public const int LANG_RU = 1;
	public const int LANG_DE = 2;

	[SerializeField]
	private string english;
	[SerializeField]
	private string russian;
	[SerializeField]
	private string german;

	public static implicit operator string(TranslatableString translatableString)
	{
		int languageId = /*PlayerPrefs.GetInt("language_id")*/1;
		switch (languageId)
		{
			case LANG_EN:
				return translatableString.english;
			case LANG_RU:
				return translatableString.russian;
			case LANG_DE:
				return translatableString.german;
		}
		Debug.LogError("Wrong languageId in config");
		return translatableString.english;
	}
}
