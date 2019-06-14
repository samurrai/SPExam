using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemProgramingExamWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private object lockObject = new object();
        private int[] numbersArray;
        private int number;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThreadStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(numberTextBox.Text, out int result) || result < 1)
            {
                MessageBox.Show("Некорректные данные");
                return;
            }
            int size = int.Parse(numberTextBox.Text);

            numbersArray = new int[size];
            number = 0;

            for (int i = 0; i < numbersArray.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(IncreaseNumber);
            }
            arrayTextBlock.Text = "";
            for (int i = 0; i < numbersArray.Length; i++)
            {
                arrayTextBlock.Text += numbersArray[i];
            }
        }

        private void IncreaseNumber(object state)
        {
            lock (lockObject)
            {
                numbersArray[number] = number + 1;
                number++;
            }
        }

        private async void DownloadButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(urlTextBox.Text))
            {
                MessageBox.Show("Введите URL");
                return;
            }
            if (string.IsNullOrWhiteSpace(fileNameTextBox.Text))
            {
                MessageBox.Show("Введите название файла");
                return;
            }
            using (var client = new WebClient())
            {
                try
                {
                    await client.DownloadFileTaskAsync(new Uri(urlTextBox.Text), fileNameTextBox.Text);
                    using (var context = new LoadedFileContext())
                    {
                        context.Files.Add(new File {
                            Name = fileNameTextBox.Text,
                            Url = urlTextBox.Text
                        });
                        await context.SaveChangesAsync();
                    }
                }
                catch (WebException)
                {
                    MessageBox.Show("Не удалось скачать файл");
                }
                catch (UriFormatException)
                {
                    MessageBox.Show("Некорректный URL");
                }
            }
        }
    }
}
