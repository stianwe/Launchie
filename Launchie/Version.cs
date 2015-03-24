
using System;
using System.Collections.Generic;

namespace Launchie
{
    [Serializable]
    public class Version
    {
        private readonly List<int> _vs = new List<int>();

        public Version(string version)
        {
            foreach (var s in version.Split('.'))
            {
                _vs.Add(int.Parse(s));
            }
        }

        public static bool operator ==(Version v1, Version v2)
        {
            if (v1._vs.Count != v2._vs.Count)
            {
                return false;
            }
            for (int i = 0; i < v1._vs.Count; i++)
            {
                if (v1._vs[i] != v2._vs[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(Version v1, Version v2)
        {
            return !(v1 == v2);
        }

        public static bool operator <(Version v1, Version v2)
        {
            if (v1._vs.Count != v2._vs.Count)
            {
                return false;
            }
            for (int i = 0; i < v1._vs.Count; i++)
            {
                if (v1._vs[i] > v2._vs[i])
                {
                    return false;
                }
                if (v1._vs[i] < v2._vs[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static bool operator >(Version v1, Version v2)
        {
            if (v1._vs.Count != v2._vs.Count)
            {
                return false;
            }
            for (int i = 0; i < v1._vs.Count; i++)
            {
                if (v1._vs[i] > v2._vs[i])
                {
                    return true;
                }
                if (v1._vs[i] < v2._vs[i])
                {
                    return false;
                }
            }
            return false;
        }

        public static bool operator <=(Version v1, Version v2)
        {
            return v1 == v2 || v1 < v2;
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return v1 == v2 || v1 > v2;
        }
    }
}
