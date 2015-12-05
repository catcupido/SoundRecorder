/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Windows {
    using System;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Application;

    using System.IO;

    public partial class WindowHelp : Window {

        public WindowHelp() {
            Action updateStatus = () => {
                buttonBack.IsEnabled = browser.CanGoBack;
                buttonForward.IsEnabled = browser.CanGoForward;
                buttonHome.IsEnabled = browser.Source != null;
            }; //updateStatus
            InitializeComponent();
            Title = string.Format("{0} Help", SoundRecorderApplication.Current.ProductName);
            buttonHome.Click += (sender, eventArgs) => {
                browser.NavigateToString(SoundRecorder.Resources.Resources.Help);
            }; //buttonHome.Click
            buttonBack.Click += (sender, eventArgs) => {
                if (browser.CanGoBack)
                    browser.GoBack();
            }; //buttonBack.Click
            buttonForward.Click += (sender, eventArgs) => {
                if (browser.CanGoForward)
                    browser.GoForward();
            }; //buttonForward.Click
            browser.LoadCompleted += (sender, eventArgs) => {
                this.status.Content = "Done";
                if (browser.Source != null)
                    this.Uri.Text = browser.Source.ToString();
                else
                    this.Uri.Text = SoundRecorder.Application.SoundRecorderApplication.Current.ProductName;
                updateStatus();
            }; //browser.LoadCompleted
            browser.Navigating += (sender, eventArgs) => {
                this.status.Content = "Loading...";
                updateStatus();
            }; //browser.Navigating
        } //WindowHelp

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            this.status.Width = this.status.ActualWidth;
            browser.NavigateToString(SoundRecorder.Resources.Resources.Help);
            browser.Focus();
        } //OnContentRendered

    } //WindowHelp

} //namespace SoundRecorder.Windows