using System;
using System.Collections;
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
