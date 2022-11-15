using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class CombatTextReference : BaseReference<CombatText, CombatTextVariable>
	{
	    public CombatTextReference() : base() { }
	    public CombatTextReference(CombatText value) : base(value) { }
	}
}