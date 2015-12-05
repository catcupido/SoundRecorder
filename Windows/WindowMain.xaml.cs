/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Windows {
    using System;
    using System.Windows;
    using System.Windows.Threading;
    public partial class WindowMain : Window {

        public WindowMain() {
            InitializeComponent();
            this.ShowInTaskbar = true;
            this.Icon = SoundRecorder.Application.SoundRecorderApplication.Current.ApplicationIcon;
            foreach (Window window in new Window[] { about, help }) {
                window.Icon = this.Icon;
                window.ShowInTaskbar = false;
                window.Closing += (sender, eventArgs) => {
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    eventArgs.Cancel = true;
                    (sender as Window).Hide();
                }; //window.Closing
            } //loop
            help.ShowActivated = true;
            Title = SoundRecorder.Application.SoundRecorderApplication.Current.ProductName;
            SetupIndicator();
            SetButtons();
            SetupMenu();
            Action afterExpandingCollapsing = () => {
                MinHeight = 0; MaxHeight = int.MaxValue;    
                this.SizeToContent = SizeToContent.Manual;
                this.SizeToContent = SizeToContent.Height;
                Dispatcher.BeginInvoke(new Action(() => {
                    MinHeight = MaxHeight = ActualHeight;
                }), DispatcherPriority.Input);
            }; //afterExpandingCollapsing
            this.expanderOutput.Expanded += (sender, eventArgs) => { afterExpandingCollapsing(); };
            this.expanderOutput.Collapsed += (sender, eventArgs) => { afterExpandingCollapsing(); };
            this.expanderActivation.Expanded += (sender, eventArgs) => { afterExpandingCollapsing(); };
            this.expanderActivation.Collapsed += (sender, eventArgs) => { afterExpandingCollapsing(); };
            textBoxDelay.PreviewTextInput += (sender, eventArgs) => { eventArgs.Handled = !Char.IsDigit(eventArgs.Text[0]); };
            this.menuItemAbout.Click += (sender, eventArgs) => { about.ShowDialog(); };
            this.menuItemHelp.Click += (sender, eventArgs) => {
                if (help.IsVisible)
                    help.Activate();
                else
                    help.Show();
            }; //menuItemHelp.Click
            /* //experimental:
            #region instead of ownership behavior
            Activated += (sender, eventArgs) => {
                if (help.IsVisible)
                    help.Activate();
            }; //Activated
            help.Activated += (sender, eventArgs) => { Activate(); };
            #endregion instead of ownership behavior
            */
        } //WindowMain

        WindowAbout about = new WindowAbout();
        WindowHelp help = new WindowHelp();
        Window hiddenOwner = new Window();

    } //class WindowMain

} //namespace SoundRecorder.Windows
