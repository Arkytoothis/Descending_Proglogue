using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	[CreateAssetMenu(
	    fileName = "TooltipTextGameEvent.asset",
	    menuName = SOArchitecture_Utility.GAME_EVENT + "TooltipText Event",
	    order = 120)]
	public sealed class TooltipTextGameEvent : GameEventBase<TooltipText>
	{
	}
}