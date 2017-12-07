using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
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
            EventTrans();
        }

        public void EventTrans()
        {
            var progress = new Progress<int>();
            var progressReport = Observable.FromEventPattern<int>(
                handler => progress.ProgressChanged += handler,
                handler => progress.ProgressChanged -= handler);
            progressReport.Subscribe(x1 => Trace.WriteLine("OnNext: " + x1.EventArgs));
        }

        public void EventTime()
        {
            var timer = new System.Timers.Timer(interval: 1000) { Enabled = true };
        }


        public async Task TranferError()
        {
            try
            {
                var block = new TransformBlock<int, int>(item =>
                {
                    if (item == 1)
                        throw new InvalidOperationException("Blech.");
                    return item * 2;
                });
                block.Post(1);
                await block.Completion;
            }catch(InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private async void btn_n1_ClickAsync(object sender, RoutedEventArgs e)
        {
            //await DoSomethingAsync();
            //await TranferError();
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

        private void btn_n8_Click(object sender, RoutedEventArgs e)
        {
            var uiContext = SynchronizationContext.Current;
            Trace.WriteLine("UI thread is " + Environment.CurrentManagedThreadId);
            Observable.Interval(TimeSpan.FromSeconds(1))
                .ObserveOn(uiContext)
                .Subscribe(x12 => 
                {
                    btn_n8.Content = "news";
                    Trace.WriteLine("Interval" + x12 + "on thread" + Environment.CurrentManagedThreadId);
                });
        }

        private void btn_n9_Click(object sender, RoutedEventArgs e)
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Buffer(2)
                .Subscribe(x => Trace.WriteLine(DateTime.Now.Second + ": Got " + x[0] + " and " + x[1]));
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

        void RoatrMatrics(IEnumerable<Matrix> matrices, float degrees)
        {
            Parallel.ForEach(matrices, x => x.Rotate(degrees));
        }

        void InverMatrices(IEnumerable<Matrix> matrics)
        {
            Parallel.ForEach(matrics, (x, y) => 
            {
                if(!x.IsIdentity)
                {
                    y.Stop();
                }
                else
                {
                    x.Invert();
                }
            });
        }

        void RoateMatricesToken(IEnumerable<Matrix> matrices, float degrees, CancellationToken token)
        {
            Parallel.ForEach(matrices,
                new ParallelOptions { CancellationToken = token},
                x => x.Rotate(degrees));
        }

        void InvertMatrices(IEnumerable<Matrix> martices)
        {
            object mutex = new object();
            int nonInvertibleCount = 0;
            Parallel.ForEach(martices, x => 
            {
                if(x.IsIdentity)
                {
                    x.Invert();
                }
                else
                {
                    lock(mutex)
                    {
                        nonInvertibleCount++;
                    }
                }
            });
        }

        static int ParallelSum(IEnumerable<int> values)
        {
            return values.AsParallel().Aggregate(
                seed: 0,
                func:(sum, item) => sum + item);
        }

        static int ParallelSum1(IEnumerable<int> values)
        {
            return values.AsParallel().Sum();
        }

        static int ParallelSum2(IEnumerable<int> values)
        {
            object mutex = new object();
            int result = 0;
            Parallel.ForEach(source:values,
                localInit: () => 0,
                body: (item, state, localValue) => localValue + item,
                localFinally:localValue => 
                {
                    lock(mutex)
                    {
                        result += localValue;
                    }
                });
            return result;
        }

        static void DoAction20Times(Action action)
        {
            Action[] actions = Enumerable.Repeat(action, 20).ToArray();
            Parallel.Invoke(actions);
        }

        static void DoAction20TimesOne(Action action, CancellationToken token)
        {
            Action[] actions = Enumerable.Repeat(action, 20).ToArray();
            Parallel.Invoke(new ParallelOptions { CancellationToken = token }, actions);
        }

        static IEnumerable<int> MultiplyBy2(IEnumerable<int> values)
        {
            return values.AsParallel().AsOrdered().Select(x => x * 2);
        }

        static int ParallelSumOneEx(IEnumerable<int> values)
        {
            return values.AsParallel().Sum();
        }

        static async void TPL1()
        {
            var multiplyBlock = new TransformBlock<int, int>(item => item * 2);
            var subtractBlock = new TransformBlock<int, int>(item => item - 2);

            var options = new DataflowLinkOptions { PropagateCompletion = true };
            multiplyBlock.LinkTo(subtractBlock);

            multiplyBlock.Complete();
            await subtractBlock.Completion;
        }

        static void TPL2()
        {
            var block = new TransformBlock<int, int>(x1 =>
            {
                if (x1 == 1)
                    throw new InvalidOperationException("Blech.");
                return x1 * 2;
            });
            block.Post(1);
            block.Post(2);
        }

        private void ImmuteList()
        {
            var stack = ImmutableStack<int>.Empty;
            var queue = ImmutableQueue<int>.Empty;
            var list = ImmutableList<int>.Empty;
        }

        async Task<PingReply> PingAsync(string hostNameOrAddress, CancellationToken cancellationToken)
        {
            var ping = new Ping();
            using (cancellationToken.Register(() => ping.SendAsyncCancel()))
            {
                return await ping.SendPingAsync(hostNameOrAddress);
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

    public interface IHttpService
    {
        IObservable<string> GetString(string url);
    }

    public class MyTimeoutClass
    {
        private readonly IHttpService _httpService;


        public MyTimeoutClass(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IObservable<string> GetStringWithTimeout(string url)
        {
            return _httpService.GetString(url)
                .Timeout(TimeSpan.FromSeconds(1));
        }
    }

    interface IMyAsyncInterfaceOne
    {
        Task<int> CountBytesAsync(string url);
    }

    class MyAsyncOne : IMyAsyncInterfaceOne
    {
        public async Task<int> CountBytesAsync(string url)
        {
            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(url);
            return bytes.Length;
        }

        static async Task UseMyInterfaceAsync(IMyAsyncInterfaceOne service)
        {
            var result = await service.CountBytesAsync("http://www.baidu.com");
            Trace.WriteLine(result);
        }
    }

    class MyAsyncTow : IMyAsyncInterfaceOne
    {
        public Task<int> CountBytesAsync(string url)
        {
            return Task.FromResult(13);
        }
    }

    class MyAsyncClassThree
    {
        private MyAsyncClassThree()
        {
        }

        private async Task<MyAsyncClassThree> InitializeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return this;
        }

        public static Task<MyAsyncClassThree> CreateAsync()
        {
            var result = new MyAsyncClassThree();
            return result.InitializeAsync();
        }
    }

    public interface IAsyncInitialization
    {
        Task Initialization { get; }
    }

    public interface IMyFundamentalType
    {

    }

    class MyFundamentalType : IAsyncInitialization
    {   
        public MyFundamentalType()
        {
            Initialization = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        public Task Initialization { get; private set; }
    }

    public static class AsyncInitialization
    {
        static Task WhenAllInitializedAsync(params object[] instances)
        {
            return Task.WhenAll(instances
                .OfType<IAsyncInitialization>()
                .Select(x => x.Initialization));
        }
    }

    public class MyEventArgs : EventArgs
    {

    }
}
