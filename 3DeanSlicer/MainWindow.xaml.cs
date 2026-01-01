using _3DeanSlicer.Core.Contracts;
using _3DeanSlicer.Gui.PartsList;
using System.Windows;
using System.Windows.Controls;

namespace _3DeanSlicer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IAppWindow
    {
        private IAppContent? _appContent;

        public MainWindow()
        {
            InitializeComponent();
            SetAppContent(new PartsListContent()); //'//////////////////////////////////////////////////////////////////                                                       ;
        }

        public Grid GetMainGrid()
        {
            return MainGrid;
        }

        public Menu GetMenuBar()
        {
            return MenuBar;
        }

        public Window GetWindow()
        {
            return this;
        }

        public void SetAppContent(IAppContent content)
        {
            _appContent = content;
            content.OnLoad(this);
        }
    }
}