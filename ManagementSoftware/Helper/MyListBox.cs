using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ManagementSoftware.Helper
{
    public class MyListBox : ListBox
    {
        public void Select(IEnumerable list)
        {
            this.SetSelectedItems(list);
        }
    }
}
