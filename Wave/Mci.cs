/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Wave {
    using System;
    using System.Text;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal static class Mci {

        static class DefinitionSet {
            internal const string DllName = "winmm.dll";
            internal const string LevelMeterDeviceId = "soundLevelMeterDevice";
            internal const string SoundRecordDeviceId = "soundRecordDevice";
            internal const string OpenCommandFormat = "open new type waveaudio alias {0}";
            internal const string StatusLevelCommandFormat = "status {0} level";
            internal const string RecordCommandFormat = "record {0}";
            internal const string PauseCommandFormat = "pause {0}";
            internal const string StopCommandFormat = "stop {0}";
            internal const string CloseCommandFormat = "close {0}";
            internal const string SaveCommandFormatFormat = @"save {0} ""{{0}}""";
            internal static readonly string OpenLevelMeterCommand = string.Format(OpenCommandFormat, LevelMeterDeviceId);
            internal static readonly string OpenRecorderCommand = string.Format(OpenCommandFormat, SoundRecordDeviceId);
            internal static readonly string StatusLevelCommand = string.Format(StatusLevelCommandFormat, LevelMeterDeviceId);
            internal static readonly string RecordCommand = string.Format(RecordCommandFormat, SoundRecordDeviceId);
            internal static readonly string PauseCommand = string.Format(PauseCommandFormat, SoundRecordDeviceId);
            internal static readonly string StopCommand = string.Format(StopCommandFormat, SoundRecordDeviceId);
            internal static readonly string CloseRecorderCommand = string.Format(CloseCommandFormat, SoundRecordDeviceId);
            internal static readonly string CloseLevelMeterCommand = string.Format(CloseCommandFormat, LevelMeterDeviceId);
            internal static readonly string SaveCommandFormat = string.Format(SaveCommandFormatFormat, SoundRecordDeviceId);
            internal const int ReturnNumDigits = 0x10;
            internal const int MaximumLevel = 128; //why?
        } //DefinitionSet

        internal class MciException : ApplicationException {
            internal MciException(long mciErrorCode) : base (string.Format("MCI error {0}", mciErrorCode)) { this.MciErrorCode = mciErrorCode; }
            internal long MciErrorCode { get; private set; }
        } //class MciException

        [DllImport(DefinitionSet.DllName)]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr oCallback);

        internal static void StartLevelMeter() {
            mciSendString(DefinitionSet.OpenLevelMeterCommand, null, 0, IntPtr.Zero);
        } //StartLevelMeter

        internal const double MaximumLevel = DefinitionSet.MaximumLevel;
        internal static double GetLevel(int count, out double maxLevel, int delayMs) {
            double buf = 0;
            maxLevel = double.NegativeInfinity;
            for (int index = 0; index < count; ++index) {
                StringBuilder sb = new StringBuilder();
                mciSendString(DefinitionSet.StatusLevelCommand, sb, DefinitionSet.ReturnNumDigits, IntPtr.Zero);
                double value;
                if (!double.TryParse(sb.ToString(), out value))
                    return 0;
                buf += value;
                if (value > maxLevel) maxLevel = value;
                System.Threading.Thread.Sleep(delayMs);
            } //loop
            return buf / count;
        } //GetLevel
        internal static double GetLevel() {
            double dummyMax;
            return GetLevel(1, out dummyMax, 0);
        } //GetLevel
        internal static void CloseLevelMeter() {
            mciSendString(DefinitionSet.CloseLevelMeterCommand, null, 0, IntPtr.Zero);
        } //CloseLevelMeter

        internal static void Open() {
            mciSendString(DefinitionSet.OpenRecorderCommand, null, 0, IntPtr.Zero);
        } //Open

        internal static void Record() {
            mciSendString(DefinitionSet.RecordCommand, null, 0, IntPtr.Zero);
        } //Record

        internal static void Pause() {
            mciSendString(DefinitionSet.PauseCommand, null, 0, IntPtr.Zero);
        } //Pause

        internal static void Stop() {
            mciSendString(DefinitionSet.StopCommand, null, 0, IntPtr.Zero);
        } //Stop

        internal static void Close() {
            mciSendString(DefinitionSet.CloseRecorderCommand, null, 0, IntPtr.Zero);
        } //Close

        internal static void SaveRecording(string fileName) {
            mciSendString(string.Format(DefinitionSet.SaveCommandFormat, fileName), null, 0, IntPtr.Zero);
        } //SaveRecording

    } //class Mci

} //namespace SoundRecorder.Wave
