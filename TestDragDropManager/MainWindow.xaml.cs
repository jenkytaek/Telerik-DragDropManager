using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.DragDrop;

namespace TestDragDropManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<ApplicationInfo> sourceLeft = new ObservableCollection<ApplicationInfo>(ApplicationInfo.GenerateApplicationInfos());
        private readonly ObservableCollection<ApplicationInfo> sourceRight = new ObservableCollection<ApplicationInfo>();

        public MainWindow()
        {
            InitializeComponent();

            DragDropManager.AddDragInitializeHandler(ApplicationListLeft, OnDragInitialize);
            DragDropManager.AddDragInitializeHandler(ApplicationListRight, OnDragInitialize);

            DragDropManager.AddGiveFeedbackHandler(ApplicationListLeft, OnGiveFeedback);
            DragDropManager.AddGiveFeedbackHandler(ApplicationListRight, OnGiveFeedback);

            DragDropManager.AddDropHandler(ApplicationListLeft, OnDrop);
            DragDropManager.AddDropHandler(ApplicationListRight, OnDrop);

            DragDropManager.AddDragDropCompletedHandler(ApplicationListLeft, OnDragCompleted);
            DragDropManager.AddDragDropCompletedHandler(ApplicationListRight, OnDragCompleted);
        }

        public ICollection<ApplicationInfo> SourceLeft
        {
            get { return this.sourceLeft; }
        }

        public ICollection<ApplicationInfo> SourceRight
        {
            get { return this.sourceRight; }
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs args)
        {
            Console.Out.WriteLine("OnDragInitialize started");

            var listbox = sender as RadListBox;

            if (listbox == null)
            {
                return;
            }

            var selectedItem = listbox.SelectedItem as ApplicationInfo;

            if (selectedItem == null)
            {
                return;
            }

            args.AllowedEffects = DragDropEffects.Move;
            var payload = DragDropPayloadManager.GeneratePayload(null);
            payload.SetData("DragData", selectedItem);

            args.Data = payload;
            args.DragVisual = new ContentControl
                {
                    Content = DragDropPayloadManager.GetDataFromObject(args.Data, "DragData") as ApplicationInfo,
                    ContentTemplate = LayoutRoot.Resources["ApplicationTemplateDragged"] as DataTemplate
                };

            Console.Out.WriteLine("OnDragInitialize ended");
        }

        private void OnGiveFeedback(object sender, Telerik.Windows.DragDrop.GiveFeedbackEventArgs args)
        {
            args.SetCursor(Cursors. ArrowCD);
            args.Handled = true;
        }

        public void OnDragCompleted(object sender, Telerik.Windows.DragDrop.DragDropCompletedEventArgs args)
        {
            Console.Out.WriteLine("OnDragCompleted started");

            try
            {
                var listbox = sender as RadListBox;
                var data = DragDropPayloadManager.GetDataFromObject(args.Data, "DragData") as ApplicationInfo;

                if (listbox != null && data != null)
                {
                    var source = listbox.ItemsSource as IList;
                    if (source != null)
                    {
                        source.Remove(data);    
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error " + e.Message);
            }

            Console.Out.WriteLine("OnDragCompleted ended");
        }

        public void OnDrop(object sender, Telerik.Windows.DragDrop.DragEventArgs args)
        {
            Console.Out.WriteLine("OnDrop started");

            try
            {
                var listbox = sender as RadListBox;
                var data = DragDropPayloadManager.GetDataFromObject(args.Data, "DragData") as ApplicationInfo;

                if (listbox != null && data != null)
                {
                    var source = listbox.ItemsSource as IList;
                    if (source != null)
                    {
                        source.Add(data);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error " + e.Message);
            }

            Console.Out.WriteLine("OnDrop ended");
        }
    }

    public class ApplicationInfo
    {
        public Double Price
        {
            get;
            set;
        }
        public String IconPath
        {
            get;
            set;
        }
        public String Name
        {
            get;
            set;
        }
        public String Author
        {
            get;
            set;
        }

        public static ICollection<ApplicationInfo> GenerateApplicationInfos()
        {
            ICollection<ApplicationInfo> result = new List<ApplicationInfo>();

            result.Add(new ApplicationInfo { Name = "Large Collider", Author = "C.E.R.N.", IconPath = @"img/Atom.png" });
            result.Add(new ApplicationInfo { Name = "Paintbrush", Author = "Imagine Inc.", IconPath = @"img/Brush.png" });
            result.Add(new ApplicationInfo { Name = "Lively Calendar", Author = "Control AG", IconPath = @"img/CalendarEvents.png" });
            result.Add(new ApplicationInfo { Name = "Fire Burning ROM", Author = "The CD Factory", IconPath = @"img/CDBurn.png" });
            result.Add(new ApplicationInfo { Name = "Fav Explorer", Author = "Star Factory", IconPath = @"img/favorites.png" });
            result.Add(new ApplicationInfo { Name = "IE Fox", Author = "Open Org", IconPath = @"img/Connected.png" });
            result.Add(new ApplicationInfo { Name = "Charting", Author = "AA-AZ inc", IconPath = @"img/ChartDot.png" });
            result.Add(new ApplicationInfo { Name = "SuperPlay", Author = "EB Games", IconPath = @"img/Games.png" });

            return result;
        }
    }
}
