// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

/*This script has been, partially or completely, generated by the Fungus.GenerateVariableWindow*/
using UnityEngine;


namespace Fungus
{
    /// <summary>
    /// Character variable type.
    /// </summary>
    [VariableInfo("Other", "Character")]
    [AddComponentMenu("")]
	[System.Serializable]
	public class CharacterVariable : VariableBase<Fungus.Character>
	{ }

	/// <summary>
	/// Container for a Character variable reference or constant value.
	/// </summary>
	[System.Serializable]
	public struct CharacterData
	{
		[SerializeField]
		[VariableProperty("<Value>", typeof(CharacterVariable))]
		public CharacterVariable characterRef;

		[SerializeField]
		public Fungus.Character characterVal;

		public static implicit operator Fungus.Character(CharacterData CharacterData)
		{
			return CharacterData.Value;
		}

		public CharacterData(Fungus.Character v)
		{
			characterVal = v;
			characterRef = null;
		}

		public Fungus.Character Value
		{
			get { return (characterRef == null) ? characterVal : characterRef.Value; }
			set { if (characterRef == null) { characterVal = value; } else { characterRef.Value = value; } }
		}

		public string GetDescription()
		{
			if (characterRef == null)
			{
				return characterVal != null ? characterVal.ToString() : "Null";
			}
			else
			{
				return characterRef.Key;
			}
		}
	}
}