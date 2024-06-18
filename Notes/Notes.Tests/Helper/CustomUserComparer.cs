using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Model;

namespace Notes.Tests.Helper
{
    public class CustomUserComparer : IEqualityComparer<User>
    {
        public bool Equals(User x, User y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(User obj) => HashCode.Combine(obj.Id, obj.Name);
    }
}
