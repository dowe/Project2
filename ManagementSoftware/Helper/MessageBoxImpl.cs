using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ManagementSoftware.Helper
{
    [ExcludeFromCodeCoverage]
    public class MessageBoxImpl : IMessageBox
    {
        public void Show(string messageBoxText)
        {
            MessageBox.Show(messageBoxText);
        }
    }
}
