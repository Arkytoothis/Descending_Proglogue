using Descending.Tiles;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class MapPositionReference : BaseReference<MapPosition, MapPositionVariable>
	{
	    public MapPositionReference() : base() { }
	    public MapPositionReference(MapPosition value) : base(value) { }
	}
}