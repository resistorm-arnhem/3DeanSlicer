using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace _3DeanSlicer.Core.Contracts
{
    public interface IAppWindow
    {
        Window GetWindow();
        Menu GetMenuBar();
        Grid GetMainGrid();
        void SetAppContent(IAppContent content);
    }
}
