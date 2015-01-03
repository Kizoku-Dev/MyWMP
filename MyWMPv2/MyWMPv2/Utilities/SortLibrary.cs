using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MyWMPv2.Model;

namespace MyWMPv2.Utilities
{
    class SortLibrary
    {
        private static GridViewColumnHeader _lastHeaderClicked = null;
        private static ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public static void SortColumn(RoutedEventArgs e, IEnumerable<IMyMedia> list, List<ThemeElem> theme)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding)
            {
                ListSortDirection direction;
                if (headerClicked != _lastHeaderClicked)
                    direction = ListSortDirection.Ascending;
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending)
                        direction = ListSortDirection.Descending;
                    else
                        direction = ListSortDirection.Ascending;
                }
                string header = headerClicked.Column.Header as string;
                header = ReplaceThemeValue(header, theme);
                Sort(header, direction, list);
                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }

        private static String ReplaceThemeValue(String header, List<ThemeElem> theme)
        {
            foreach (var elem in theme)
            {
                if (header.Equals(elem.Value))
                {
                    header = elem.DefaultValue;
                    break;
                }
            }
            return header;
        }

        private static void Sort(string sortBy, ListSortDirection direction, IEnumerable<IMyMedia> list)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(list);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
    }
}
