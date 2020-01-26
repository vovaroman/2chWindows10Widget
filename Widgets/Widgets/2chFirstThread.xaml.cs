using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Widgets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class Thread
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Link { get; set; }
        public List<string> Files { get; set; } = new List<string>();

    }

    public partial class DvachFirstThrread : Window
    {
        public DvachFirstThrread()
        {
            InitializeComponent();
            txtThreadComment.Text = ThreadComment;
            Link.Content = ThreadLink;
            ThreadInfo = null;
            Link.Foreground = SystemParameters.WindowGlassBrush;
            Border.BorderBrush = SystemParameters.WindowGlassBrush;
            #region picture
            //foreach(var file in ThreadFiles)
            //{
            //    Image img = new Image();
            //    var image = new BitmapImage(new Uri(file));
            //    Grid.SetRow(img, 0);
            //    Grid.SetColumn(img, 0);
            //    img.Source = image;
            //    ContentRoot.Children.Add(img);

            //}
            #endregion

            var thread = new System.Threading.Thread(() => _updateValues());
            thread.Start();
        }

        private void _updateValues()
        {
            while(true)
            {

                this.Dispatcher.Invoke(() =>
                {
                    txtThreadComment.Text = ThreadComment;
                    Link.Content = ThreadLink;
                    ThreadInfo = null;
                });
                System.Threading.Tasks.Task.Delay(10000).Wait();

            }

        }

        private Thread _threadInfo { get; set; }
        public Thread ThreadInfo
        {
            get
            {
                if(_threadInfo == null)
                    _threadInfo = _getFirstThread();
                return _threadInfo;
            }
            set
            {
                _threadInfo = value;
            }
        }

        public string ThreadName
        {
            get
            {
                return ThreadInfo.Name;
            }
        }

        public string ThreadComment
        {
            get
            {
                return ThreadInfo.Comment;
            }
        }
        //public List<string> ThreadFiles
        //{
        //    get
        //    {
        //        return ThreadInfo.Files;
        //    }
        //}

        public string ThreadLink
        {
            get
            {
                return ThreadInfo.Link;
            }
        }

        private void _dvachFirstThread_OnLoaded(object sender, RoutedEventArgs e)
        {
            Helper.SendWpfWindowBack(Application.Current.MainWindow);
        }

        private Thread _getFirstThread()
        {
            var result = new Thread();
            try
            {
                using (var client = new WebClient())
                {
                    var json = client.DownloadString("https://2ch.hk/b/catalog.json");
                    
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(json);
                    var firstThread = data.threads[0];

                    byte[] bytes = Encoding.Default.GetBytes(firstThread.subject.ToString());
                    string subject = Encoding.UTF8.GetString(bytes);
                    string link = $"https://2ch.hk/b/res/{firstThread.num}.html";

                    bytes = Encoding.Default.GetBytes(firstThread.comment.ToString());
                    string comment = Encoding.UTF8.GetString(bytes);

                    string resultComment = WebUtility.HtmlDecode(comment);
                    resultComment = Regex.Replace(resultComment, @"<.*?>", String.Empty);
                    #region picture
                    //if(firstThread.files.Count > 0)
                    //{
                    //    if(!Directory.Exists(Environment.CurrentDirectory + "\\Temp"))
                    //    {
                    //        Directory.CreateDirectory(Environment.CurrentDirectory + "\\Temp");
                    //    }
                    //    foreach(var file in firstThread.files)
                    //    {
                    //        var firstPhotoPath = $"https://2ch.hk/{file.path}";

                    //        var fileName = Environment.CurrentDirectory + "\\Temp\\" + firstPhotoPath.Substring(firstPhotoPath.LastIndexOf("/") + 1);

                    //        client.DownloadFile(new Uri(firstPhotoPath), fileName);
                    //        result.Files.Add(fileName);
                    //    }


                    //}
                    #endregion
                    result.Name = subject;
                    result.Comment = resultComment;
                    result.Link = link;
                    

                }

            }
            catch(Exception ex)
            {

            }

            return result;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
            else
                base.OnMouseDown(e);
        }

        private void Link_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("chrome.exe", ThreadLink);
        }
    }
}
