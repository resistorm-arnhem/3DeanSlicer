using _3DeanSlicer.Gui.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _3DeanSlicer.Gui.Common.TreeViewer
{
    internal class FileTreeView : UserControl
    {
        public static readonly ImageSource FolderIcon = Images.LoadIcon("folder.png");
        public static readonly ImageSource FolderOpenIcon = Images.LoadIcon("folder_open.png");
        public static readonly ImageSource FileIcon = Images.LoadIcon("file.png");
        public static readonly ImageSource DriveIcon = Images.LoadIcon("drive.png");
        private readonly TreeView _treeView;
        public event Action<string>? FileSelected;

        public FileTreeView()
        {
            _treeView = new TreeView();
            Content = _treeView;
            _treeView.SelectedItemChanged += OnSelectedItemChanged;
            LoadDrives();
        }

        public void LoadDrives()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                TreeViewItem item = CreateItem(drive.Name, drive.Name, DriveIcon);
                item.Items.Add(null); // dummy
                item.Expanded += Folder_Expanded;

                _treeView.Items.Add(item);
            }
        }


        private void Folder_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is not TreeViewItem item)
                return;

            // Al geladen?
            if (item.Items.Count != 1 || item.Items[0] != null)
                return;

            item.Items.Clear();

            string path = (string)item.Tag;

            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    FileInfo dirInfo = new FileInfo(dir);
                    if (!dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        item.Items.Add(CreateFolderItem(dir));
                    }
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (!fileInfo.Attributes.HasFlag(FileAttributes.Hidden))
                    {
                        item.Items.Add(CreateFileItem(file));
                    }
                }

            }
            catch
            {
                // toegang geweigerd → negeren
            }
        }

        private TreeViewItem CreateItem(string text, string path, ImageSource icon)
        {
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            panel.Children.Add(new Image
            {
                Source = icon,
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 5, 0)
            });

            panel.Children.Add(new TextBlock
            {
                Text = text
            });

            return new TreeViewItem
            {
                Header = panel,
                Tag = path
            };
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem item &&
                item.Tag is string path )
            {
                FileSelected?.Invoke(path);
            }
        }

        private TreeViewItem CreateFolderItem(string path)
        {
            Image image = new Image
            {
                Source = FolderIcon,
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 5, 0)
            };

            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            panel.Children.Add(image);
            panel.Children.Add(new TextBlock
            {
                Text = Path.GetFileName(path)
            });

            TreeViewItem item = new TreeViewItem
            {
                Header = panel,
                Tag = path
            };

            item.Expanded += (_, _) => image.Source = FolderOpenIcon;
            item.Collapsed += (_, _) => image.Source = FolderIcon;

            item.Items.Add(null);
            item.Expanded += Folder_Expanded;

            return item;
        }

        private TreeViewItem CreateFileItem(string path)
        {
            return CreateItem(
                System.IO.Path.GetFileName(path),
                path,
                FileIcon);
        }
    }
}
