using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Units
{
    [System.Serializable]
    public class PartyData
    {
        public List<Unit> Heroes = null;

        public PartyData(List<Unit> unitList)
        {
            Heroes = new List<Unit>();

            for (int i = 0; i < unitList.Count; i++)
            {
                Heroes.Add(unitList[i]);
            }
        }
    }
}
