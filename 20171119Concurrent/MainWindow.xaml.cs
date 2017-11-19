using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
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

namespace _20171119Concurrent
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btn_n1_ClickAsync(object sender, RoutedEventArgs e)
        {
            await DoSomethingAsync();
        }

        async Task DoSomethingAsync()
        {
            int val = 13;
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            val *= 2;
            await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
            MessageBox.Show(val.ToString());
        }

        async Task TrySomethingAsync()
        {
            Task task = PossibleExceptionAsync();
            try
            {
                await task;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Task PossibleExceptionAsync()
        {
            throw new NotImplementedException();
        }

        private async void btn_n2_Click(object sender, RoutedEventArgs e)
        {
            await TrySomethingAsync();
        }

        private async void btn_n3_Click(object sender, RoutedEventArgs e)
        {
            Deadlock();
        }

        async Task WaitAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
        }

        async void Deadlock()
        {
            await WaitAsync();
            //task.Wait();
        }

        void RotateMatices(IEnumerable<Matrix> matrixes, float degrees)
        {
            Parallel.ForEach(matrixes, x => x.Rotate(degrees));
        }

        IEnumerable<bool> PrimalityTest(IEnumerable<int> values)
        {
            return values.AsParallel().Select(x => x % 2 == 0);
        }
    }
}
