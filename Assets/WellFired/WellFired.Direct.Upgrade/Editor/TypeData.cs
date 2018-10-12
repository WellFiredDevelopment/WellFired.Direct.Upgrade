using System;
using System.Collections.Generic;
using UnityEngine;

namespace WellFired.Direct.Upgrade.Editor
{
    [Serializable]
    public class TypeData
    {
        [SerializeField] private List<TypeDataEntry> _data = new List<TypeDataEntry>();

        public List<TypeDataEntry> Data
        {
            get { return _data; }
        }

        public void AddEntry(TypeDataEntry typeDataEntry)
        {
            if (Data.Contains(typeDataEntry))
                return;
            
            Data.Add(typeDataEntry);
        }
    }
}