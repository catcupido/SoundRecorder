/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Application {
    using System;
    using System.Windows;
    using System.Reflection;
    using StringList = System.Collections.Generic.List<string>;
    using IStringList = System.Collections.Generic.IList<string>;
    using StringBuilder = System.Text.StringBuilder;

    class SoundRecorderApplication : Application {

        class DefinitionSet {
            internal const string ExceptionFormat = "{0}:\n{1}";
            internal const string ExceptionStackItemDemiliter = "\n\n";
        } //class DefinitionSet

        protected override void OnStartup(StartupEventArgs e) {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            MainWindow = new Windows.WindowMain();
            MainWindow.Show();
            startupComplete = true;
        } //OnStartup

        [STAThread]
        static void Main() {
            using (var iconStream = new System.IO.MemoryStream()) {
                Application app = new SoundRecorderApplication();
                SoundRecorder.Resources.Resources.IconMain.Save(iconStream);
                iconStream.Seek(0, System.IO.SeekOrigin.Begin);
                Current.ApplicationIcon = System.Windows.Media.Imaging.BitmapFrame.Create(iconStream);
                app.Run();
            } //using
        } //Main

        internal SoundRecorderApplication() {
            DispatcherUnhandledException += (sender, eventArgs) => {
                eventArgs.Handled = true;
                HandleException(eventArgs.Exception);
            }; //DispatcherUnhandledException
        } //MailApplication

        void HandleException(Exception e) {
            Func<Exception, string> exceptionTextFinder = (ex) => {
                Action<Exception, IStringList> exceptionTextCollector = null; // for recursiveness
                exceptionTextCollector = (exc, aList) => {
                    aList.Add(string.Format(DefinitionSet.ExceptionFormat, exc.GetType().Name, exc.Message));
                    if (exc.InnerException != null)
                        exceptionTextCollector(exc.InnerException, aList);
                }; //exceptionTextCollector
                IStringList list = new StringList();
                exceptionTextCollector(ex, list);
                StringBuilder sb = new StringBuilder();
                bool first = true;
                foreach (string item in list)
                    if (first) {
                        sb.Append(item);
                        first = false;
                    } else
                        sb.Append(DefinitionSet.ExceptionStackItemDemiliter + item);
                return sb.ToString();
            };
            MessageBox.Show(
                exceptionTextFinder(e),
                SoundRecorderApplication.Current.ProductName,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            if (!startupComplete)
                Shutdown();
        } //HandleException
        bool startupComplete;

        internal static new SoundRecorderApplication Current { get { return (SoundRecorderApplication)Application.Current; } }

        internal string ProductName {
            get {
                if (productName == null) {
                    object[] attributes = TheAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                    if (attributes == null) return null;
                    if (attributes.Length < 1) return null;
                    productName = ((AssemblyProductAttribute)attributes[0]).Product;
                } //if
                return productName;
            } //get ProductName
        } //ProductName
        internal string Copyright {
            get {
                if (copyright == null) {
                    object[] attributes = TheAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                    if (attributes == null) return null;
                    if (attributes.Length < 1) return null;
                    copyright = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
                } //if
                return copyright;
            } //get Copyright
        } //Copyright
        internal Version AssemblyVersion {
            get {
                if (assemblyVersion == null) {
                    object[] attributes = TheAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    if (attributes == null) return null;
                    if (attributes.Length < 1) return null;
                    assemblyVersion = new Version(((AssemblyFileVersionAttribute)attributes[0]).Version);
                } //if
                return assemblyVersion;
            } //get AssemblyVersion
        } //AssemblyVersion

        Assembly TheAssembly {
            get {
                if (assembly == null)
                    assembly = Assembly.GetEntryAssembly();
                return assembly;
            }
        } //TheAssembly

        Assembly assembly;
        string productName, copyright;
        Version assemblyVersion;
        internal System.Windows.Media.ImageSource ApplicationIcon { get; private set; } 

    } //class MailApplication

} //namespace SoundRecorder.Application
