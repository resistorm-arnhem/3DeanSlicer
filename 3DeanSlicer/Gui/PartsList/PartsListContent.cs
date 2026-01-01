using _3DeanSlicer.Core.Contracts;
using _3DeanSlicer.Gui.Common.TreeViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _3DeanSlicer.Gui.PartsList
{
    public class PartsListContent : IAppContent
    {
        private IAppWindow? _host;
        private FileTreeView? _fileTreeView;
        private ItemsControl? _contentPanel;
        private WrapPanel? _wrapPanel;
        public void OnLoad(IAppWindow host)
        {
            _host = host;
            Grid grid = _host.GetMainGrid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(280) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _fileTreeView = new FileTreeView();
            Grid.SetRow(_fileTreeView, 0);
            Grid.SetColumn(_fileTreeView, 0);
            grid.Children.Add(_fileTreeView);
            _fileTreeView.FileSelected += FileTreeView_FileSelected;

            _contentPanel = new ItemsControl();
            Grid.SetColumn(_contentPanel, 1);
            grid.Children.Add(_contentPanel);
            _wrapPanel = new WrapPanel();
            _contentPanel.ItemsPanel = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(WrapPanel)));

        }

        private void FileTreeView_FileSelected(string path)
        {
            if (_fileTreeView != null && Directory.Exists(path))
            {
                ShowTiles(path);
            }
            else
            {
                Debug.WriteLine($"Not an existing directory");
            }
        }


        void ShowTiles(string folderPath)
        {
            _contentPanel.Items.Clear();

            foreach (var dir in Directory.GetDirectories(folderPath))
                _contentPanel.Items.Add(CreateItemPanel(dir, true));

            foreach (var file in Directory.GetFiles(folderPath))
                _contentPanel.Items.Add(CreateItemPanel(file, false));
        }

        StackPanel CreateItemPanel(string path, bool isFolder)
        {
            ImageSource icon = isFolder ? FileTreeView.FolderIcon : FileTreeView.FileIcon;

            var panel = new StackPanel { Width = 80, Margin = new Thickness(5) };
            panel.Children.Add(new Image { Source = icon, Width = 48, Height = 48 });
            panel.Children.Add(new TextBlock
            {
                Text = Path.GetFileName(path),
                TextAlignment = TextAlignment.Center,
                TextWrapping = TextWrapping.Wrap
            });

            return panel;
        }

    }
}
