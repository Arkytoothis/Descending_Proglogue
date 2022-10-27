using Descending.Units;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class PartyDataReference : BaseReference<PartyData, PartyDataVariable>
	{
	    public PartyDataReference() : base() { }
	    public PartyDataReference(PartyData value) : base(value) { }
	}
}