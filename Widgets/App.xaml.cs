using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Widgets;

namespace Widgets
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IList<Window> _windows = new List<Window>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Add windows you want to open here.
            _windows.Add(new DvachFirstThrread());
            foreach (var w in _windows)
            {
                w.Show();
            }
        }
    }
}
