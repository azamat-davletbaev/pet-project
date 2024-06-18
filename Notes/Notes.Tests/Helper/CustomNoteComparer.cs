using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notes.Model;

namespace Notes.Tests.Helper
{
    public class CustomNoteComparer : IEqualityComparer<Note>
    {
        public bool Equals(Note x, Note y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.Id == y.Id && x.UserId == y.UserId && x.Date == y.Date && x.Text == y.Text;
        }

        public int GetHashCode(Note obj) => HashCode.Combine(obj.Id, obj.UserId, obj.Date, obj.Text);
    }
}
