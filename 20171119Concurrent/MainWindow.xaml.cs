using System;
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

         async Task MyMethod(IProgress<double> progress = null)
        {
            bool done = false;
            double percentConplete = 0;
            while (!done)
            {
                //....
                await Task.Delay(TimeSpan.FromSeconds(3));
                percentConplete += 5;
                if (progress != null)
                {
                    progress.Report(percentConplete);
                }
            }
        }

         async Task CallMyMethod()
        {
            var progress = new Progress<double>();
            progress.ProgressChanged += (e, s) => 
            {
                //MessageBox.Show("ok");
                btn_n5.Content = progress.ToString();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            };
            await MyMethod(progress);
        }

        private async void btn_n5_Click(object sender, RoutedEventArgs e)
        {
            await CallMyMethod();
        }

        private async Task<String> DownloadAllAsync(IEnumerable<string> urls)
        {
            var httpClient = new HttpClient();
            //定义每一个url的使用方法
            //注意到这里序列还没有开始求值，所以所有任务都还没有真正启动
            var downloads = urls.Select(url => httpClient.GetStringAsync(url));
            //所有任务开始执行
            Task<string>[] downloadTasks = downloads.ToArray();
            //用异步的方式等待所有下载完成
            string[] htmlPages = await Task.WhenAll(downloadTasks);
            return string.Concat(htmlPages);

        }

        static async Task ThrowNotImplementedExceptionAsync()
        {
            throw new NotImplementedException();
        }

        static async Task ThrowInvalidOperationExceptionAsync()
        {
            throw new InvalidOperationException();
        }

        static async Task ObserveOneExceptionAsync()
        {
            var task1 = ThrowNotImplementedExceptionAsync();
            var task2 = ThrowInvalidOperationExceptionAsync();

            try
            {
                await Task.WhenAll(task1, task2);
            }
            catch (Exception)
            {

                throw;
            }
        }

        static async Task ObserveAllExceptionAsync()
        {
            var task1 = ThrowNotImplementedExceptionAsync();
            var task2 = ThrowInvalidOperationExceptionAsync();
            Task allTasks = Task.WhenAll(task1, task2);
            try
            {
                await allTasks;
            }
            catch (Exception)
            {
                AggregateException allException = allTasks.Exception;
            }
        }

        private async Task TestWhenAll()
        {
            Task t1 = Task.Delay(TimeSpan.FromSeconds(1));
            Task t2 = Task.Delay(TimeSpan.FromSeconds(2));
            Task t3 = Task.Delay(TimeSpan.FromSeconds(1));
            await Task.WhenAll(t1, t2, t3);
        }

        private static async Task<int> FirstRespondingUrlAsync(string urlA, string urlB)
        {
            var httpClient = new HttpClient();
            //并发的下载两个任务
            Task<byte[]> downloadTaskA = httpClient.GetByteArrayAsync(urlA);
            Task<byte[]> downloadTaskB = httpClient.GetByteArrayAsync(urlB);
            //等待任意一个任务完成
            Task<byte[]> completedTask =
                await Task.WhenAny(downloadTaskA, downloadTaskB);
            byte[] data = await completedTask;
            return data.Length;
        }

        private async void btn_n6_Click(object sender, RoutedEventArgs e)
        {
            await ProcessTasksAsync1();
        }

        async Task ProcessTasksAsync()
        {
            Task<int> tasl1 = DelayAndReturnAsync(2);
            Task<int> task2 = DelayAndReturnAsync(3);
            Task<int> task3 = DelayAndReturnAsync(1);
            var tasks = new[] { tasl1, task2, task3};
            foreach (var item in tasks)
            {
                var result = await item;
                btn_n6.Content = result.ToString();
                Trace.WriteLine(result);
            }
        }

        async Task AwaitAndProcessAsync(Task<int> task)
        {
            var result = await task;
            btn_n6.Content = result.ToString();
            Trace.WriteLine(result);
        }

        async Task ProcessTasksAsync1()
        {
            Task<int> tasl1 = DelayAndReturnAsync(2);
            Task<int> task2 = DelayAndReturnAsync(3);
            Task<int> task3 = DelayAndReturnAsync(1);
            var tasks = new[] { tasl1, task2, task3 };

            //var processingTask = (from t in tasks
            //                      select AwaitAndProcessAsync(t)).ToArray();


            var processingTask = tasks.Select(async t => 
            {
                var result = await t;
                Trace.WriteLine(result);
            }).ToArray();

            await Task.WhenAll(processingTask);
        }

        async Task<int> DelayAndReturnAsync(int val)
        {
            await Task.Delay(TimeSpan.FromSeconds(val));
            return val;
        }

        async Task ResumeOnContextAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        async Task ResumeWithoutContextAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(true);
            btn_n7.Content = "test Context";
        }

        private async void btn_7_Click(object sender, RoutedEventArgs e)
        {
            await ResumeWithoutContextAsync();
        }
    }

    sealed class MyAsyncCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public async Task Executes(object parameter)
        {

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
