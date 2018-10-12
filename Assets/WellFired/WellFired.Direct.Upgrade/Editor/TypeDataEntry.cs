using System;
using UnityEngine;

namespace WellFired.Direct.Upgrade.Editor
{
    [Serializable]
    public class TypeDataEntry
    {
        [SerializeField] private string _type;
        [SerializeField] private string _guid;
        [SerializeField] private long _fileId;

        public string Type
        {
            get { return _type; }
        }

        public string GUID
        {
            get { return _guid; }
        }

        public long FileId
        {
            get { return _fileId; }
        }

        private bool Equals(TypeDataEntry other)
        {
            return string.Equals(Type, other.Type) && string.Equals(GUID, other.GUID) && FileId == other.FileId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((TypeDataEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (GUID != null ? GUID.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ FileId.GetHashCode();
                return hashCode;
            }
        }

        public static TypeDataEntry Create(string type, string guid, long fileId)
        {
            return new TypeDataEntry
            {
                _type = type,
                _guid = guid,
                _fileId = fileId
            };
        }
    }
}