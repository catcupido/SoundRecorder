/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Windows {
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Threading;
    using System.Windows.Threading;

    public partial class WindowMain {

        class VolumeIndicatorEventArgs : EventArgs {
            internal VolumeIndicatorEventArgs(double volume, bool maximumReached) {
                this.Volume = volume;
                this.MaximumReached = maximumReached;
            } //VolumeIndicatorEventArgs
            internal double Volume { get; private set; }
            internal bool MaximumReached { get; private set; }
        } //class VolumeIndicatorEventArgs

        class IndicatorThreadWrapper {
            internal IndicatorThreadWrapper() {
                thread = new Thread(Body);
            } //IndicatorThreadWrapper
            internal void Start() { thread.Start(); }
            internal void Stop() { exit = true; }
            internal EventHandler<VolumeIndicatorEventArgs> VolumeLevelMeasured;
            void Body() {
                Wave.Mci.StartLevelMeter();
                while (!exit) {
                    if (VolumeLevelMeasured != null) {
                        double max;
                        double level = Wave.Mci.GetLevel(10, out max, 1);
                        bool maxReached = level >= Wave.Mci.MaximumLevel;
                        VolumeLevelMeasured.Invoke(this, new VolumeIndicatorEventArgs(level, maxReached));
                    } //if
                    Thread.Sleep(1);
                } //loop
                Wave.Mci.CloseLevelMeter();
            } //Body
            bool exit;
            Thread thread;
        } //class IndicatorThreadWrapper

        void SetupIndicator() {
            indicatorThreadWrapper.VolumeLevelMeasured += (sender, eventArgs) => {
                Dispatcher.Invoke(new Action<WindowMain>((wnd) => {
                    wnd.volumeIndicator.Value = eventArgs.Volume;
                    if (eventArgs.MaximumReached)
                        wnd.volumeIndicator.Foreground = overflowIndicator;
                    else
                        wnd.volumeIndicator.Foreground = normalIndicator;
                    if (wnd.checkBoxUseSoundActivation.IsChecked == true && wnd.state == State.Waiting && eventArgs.Volume > wnd.volumeIndicator.Threshold) {
                        int delay;
                        if (int.TryParse(wnd.textBoxDelay.Text, out delay))
                            System.Threading.Thread.Sleep(delay);
                        wnd.StartRecording();
                    } //if                    
                }), this);
            }; //indicatorThreadWrapper.VolumeLevelMeasured
            overflowIndicator = this.volumeIndicator.Background;
            volumeIndicator.Background = Brushes.Transparent;
            normalIndicator = this.volumeIndicator.Foreground;
            volumeIndicator.Maximum = Wave.Mci.MaximumLevel;
            DispatcherTimer recordingBlinkingTimer = new DispatcherTimer();
            recordingBlinkingTimer.Interval = new TimeSpan(0, 0, 0, 0, 280);
            recordingBlinkingTimer.Tick += (sender, eventArgs) => { this.activityIndicator.Flash(); };
            recordingBlinkingTimer.Start();
            DispatcherTimer watchTimer = new DispatcherTimer();
            watchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            watchTimer.Tick += (sender, eventArgs) => { this.watchBox.Refresh(); };
            watchTimer.Start();
        } //SetupCapture

        protected override void OnContentRendered(EventArgs e) {
            LoadDefaultPreferences();
            base.OnContentRendered(e);
            MinHeight = MaxHeight = ActualHeight;
            MinWidth = ActualWidth;
            indicatorThreadWrapper.Start();
        } //OnContentRendered

        protected override void OnClosed(EventArgs e) {
            indicatorThreadWrapper.Stop();
            Wave.Mci.Close();
            base.OnClosed(e);
        } //OnClosed
    
        Brush normalIndicator, overflowIndicator;
        IndicatorThreadWrapper indicatorThreadWrapper = new IndicatorThreadWrapper();

    } //class WindowMain

} //namespace SoundRecorder.Windows
