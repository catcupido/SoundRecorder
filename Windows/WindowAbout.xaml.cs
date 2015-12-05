/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Windows {
    using System;
    using System.Windows;
    using Application;

    public partial class WindowAbout : Window {
    
        public WindowAbout() {
            InitializeComponent();
            SoundRecorderApplication app = SoundRecorderApplication.Current;
            Version version = app.AssemblyVersion;
            Title = string.Format("About {0}", app.ProductName);
            this.textBlockProduct.Text = string.Format("{0} v.{1}.{2}", app.ProductName, version.Major, version.Minor);
            this.texBlockCopyright.Text = SoundRecorderApplication.Current.Copyright;
            this.buttonOk.Click += (sender, eventArgs) => { Close(); };
        } //WindowAbout

    } //class WindowAbout

} //namespace SoundRecorder.Windows
