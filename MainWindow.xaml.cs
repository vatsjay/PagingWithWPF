using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace TestingDataGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Person> Persons = new List<Person>();
        private int _lastIndex; 
        private int _index = 1;
        private int _rowsPerPage = 10;
        public MainWindow()
        {
            InitializeComponent();
            GeneratePersons();
            if (Persons.Count % _rowsPerPage > 0)
            {
                _lastIndex = (Persons.Count / _rowsPerPage) + 1;
            }
            else
                _lastIndex = Persons.Count / _rowsPerPage;

            InfoBox.Text = $"{_index} of {Persons.Count/_rowsPerPage}";
            grid.ItemsSource = Persons.Take(_rowsPerPage);
            PreviousButton.IsEnabled = false;
        }

        private void GeneratePersons()
        {
            for (int i = 0; i < 100; i++)
            {
                Persons.Add(new Person { Age = 20 + i, Male = true, Name = $"person {i}" });
            }
        }

        private void Navigate(Selection pageSelection)
        {
            switch (pageSelection)
            {
                case Selection.First:
                    _index = 1;
                    FirstButton.IsEnabled = false;
                    PreviousButton.IsEnabled = false;
                    grid.ItemsSource = Persons.Take(_rowsPerPage);
                    InfoBox.Text = $"{_index} of {_lastIndex}";
                    NextButton.IsEnabled = true;
                    LastButton.IsEnabled = true;
                    break;

                case Selection.Last:
                    LastButton.IsEnabled = false;
                    NextButton.IsEnabled = false;

                    _index = _lastIndex;

                    grid.ItemsSource = Persons.Skip((_index - 1) * _rowsPerPage);
                    InfoBox.Text = $"{_index} of {_lastIndex}";

                    FirstButton.IsEnabled = true;
                    PreviousButton.IsEnabled = true;
                    break;

                case Selection.Next:
                    FirstButton.IsEnabled = true;
                    PreviousButton.IsEnabled = true;
                    if (Persons.Count - (_index * _rowsPerPage) >= _rowsPerPage)
                    {
                        grid.ItemsSource = Persons.Skip(_index * _rowsPerPage).Take(_rowsPerPage);
                    }
                    else
                    {
                        Navigate(Selection.Last);
                    }
                    _index++;
                    InfoBox.Text = $"{_index} of {_lastIndex}";
                    break;

                case Selection.Previous:
                    NextButton.IsEnabled = true;
                    LastButton.IsEnabled = true;

                    if (_index-- == 1)
                    {
                        Navigate(Selection.First);
                    }
                    else
                    {
                        grid.ItemsSource = Persons.Skip((_index - 1) * _rowsPerPage).Take(_rowsPerPage);
                        InfoBox.Text = InfoBox.Text = $"{_index} of {_lastIndex}";
                    }
                    break;
                default:
                    break;
            }
        }

        private void firstPage(object sender, RoutedEventArgs e)
        {
            Navigate(Selection.First);
        }

        private void nextPage(object sender, RoutedEventArgs e)
        {
            Navigate(Selection.Next);
        }

        private void previousPage(object sender, RoutedEventArgs e)
        {
            Navigate(Selection.Previous);
        }

        private void lastPage(object sender, RoutedEventArgs e)
        {
            Navigate(Selection.Last);
        }
    }

    public class Person
    {
        public int Age { get; set; }

        public bool Male { get; set; }

        public string Name { get; set; }
    }

    public enum Selection
    {
        First,
        Last,
        Next,
        Previous
    }
}
