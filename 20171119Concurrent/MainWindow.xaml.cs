﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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

        public void TestExtensionReactive()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Timestamp()
                .Where(x => x.Value % 2 == 0)
                .Select(x => x.Timestamp)
                .Subscribe(c => Trace.WriteLine(c));

           
        }

        private async void btn_n4_Click(object sender, RoutedEventArgs e)
        {
            var res = await DelayResult<int>(2, TimeSpan.FromSeconds(3));
            MessageBox.Show("2");
        }

        static async Task<T> DelayResult<T>(T result, TimeSpan delay)
        {
            await Task.Delay(delay);
            return result;
        }

        static async Task<String> DownloadStringWithRetries(string url)
        {
            using (var client = new HttpClient())
            {
                var nextDaly = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(url);
                    }
                    catch (Exception)
                    {

                    }
                    await Task.Delay(nextDaly);
                    nextDaly = nextDaly + nextDaly;
                }
                return await client.GetStringAsync(url);
            }
        }

        static async Task<string> DownloadStringWithTimeout(string url)
        {
            using (var client = new HttpClient())
            {
                var downloadTask = client.GetStringAsync(url);
                var timeoutTask = Task.Delay(3000);
                var conpletedTask = await Task.WhenAny(downloadTask, timeoutTask);
                if(conpletedTask == timeoutTask)
                {
                    return null;
                }
                return await downloadTask;
            }
        }

        static Task<T> NotImplementedAsync<T>()
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetException(new NotImplementedException());
            return tcs.Task;
        }

        static void MyMethod(IProgress<double> progress = null)
        {
            bool done = false;
            double percentConplete = 0;
            while (!done)
            {
                //....

                if (progress != null)
                {
                    progress.Report(percentConplete);
                }
            }
        }
    }

    interface IMyAsyncInterface
    {
        Task<int> GetValueAsync();
    }

    class MyAsyncImplement : IMyAsyncInterface
    {
        public Task<int> GetValueAsync()
        {
            return Task.FromResult(13);
        }
    }
}
