using Descending.Gui;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public class CombatTextEvent : UnityEvent<CombatText> { }

	[CreateAssetMenu(
	    fileName = "CombatTextVariable.asset",
	    menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "CombatText Event",
	    order = 120)]
	public class CombatTextVariable : BaseVariable<CombatText, CombatTextEvent>
	{
	}
}