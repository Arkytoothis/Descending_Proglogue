using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "CombatTextGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "CombatText Event",
	    order = 120)]
	public sealed class CombatTextGameEvent : GameEventBase<CombatText>
	{
	}
}