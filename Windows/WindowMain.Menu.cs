/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Windows {
    using System;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.IO;
    using ModelTop = SoundRecorder.Application.ModelTop;

    public partial class WindowMain {

        partial class MenuDefinitionSet {
            internal const string LoadPreferencesDialogTitle = "Load Preferences";
            internal const string SavePreferencesDialogTitle = "Save Preferences";
            internal const string DataFileExt = "SoundRecorderPreferences.xml";
            internal const string DataDialogFilter = "Preferences files|*." + DataFileExt;
            internal const string DefaultDataFileName = "Default." + DataFileExt;
        } //class MenuDefinitionSet

        void SetupMenu() {
            loadPreferencesDialog.Title = MenuDefinitionSet.LoadPreferencesDialogTitle;
            savePreferencesDialog.Title = MenuDefinitionSet.SavePreferencesDialogTitle;
            savePreferencesDialog.DefaultExt = MenuDefinitionSet.DataFileExt;
            loadPreferencesDialog.Filter = MenuDefinitionSet.DataDialogFilter;
            savePreferencesDialog.Filter = loadPreferencesDialog.Filter;
            menuItemExit.Click += (sender, eventArgs) => { Close(); };
            menuItemLoad.Click += (sender, eventArgs) => { LoadPreferences(); };
            menuItemSave.Click += (sender, eventArgs) => { SavePreferences(); };
        } //SetupMenu

        void SavePreferences() {
            if (savePreferencesDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                UiToData().Store(savePreferencesDialog.FileName);
        } //SavePreferences
        void LoadPreferences(string fileName) {
            ModelTop top = ModelTop.Load(fileName);
            PopulateUi(top);
        } //LoadPreferences
        void LoadPreferences() {
            if (loadPreferencesDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                LoadPreferences(loadPreferencesDialog.FileName);
        } //LoadPreferences

        string FindDefaultPreferencesFile() {
            string[] commandLine = System.Environment.GetCommandLineArgs();
            if (commandLine.Length == 2) {
                string first = commandLine[1];
                if (File.Exists(first))
                    return first;
            } //if
            string fileName = MenuDefinitionSet.DefaultDataFileName;
            if (File.Exists(fileName))
                return fileName;
            string location = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            fileName = Path.Combine(location, fileName);
            if (File.Exists(fileName))
                return fileName;
            return null;
        } //FindDefaultPreferencesFile

        void LoadDefaultPreferences() {
            string fileName = FindDefaultPreferencesFile();
            if (fileName != null)
                LoadPreferences(fileName);
        } //SA???

        ModelTop UiToData() {
            ModelTop data = new ModelTop();
            data.UseSoundActivation = checkBoxUseSoundActivation.IsChecked == true;
            int delay;
            if (!int.TryParse(textBoxDelay.Text, out delay))
                delay = 0;
            data.DelayBeforeActivationMs = delay;
            data.ActivationThreshold = volumeIndicator.Threshold;
            data.AutoRestartOnSave = checkBoxAutoRestart.IsChecked == true;
            data.BaseFileName = textBoxBaseFileName.Text;
            data.NumberOfDigitsInFileNumber = (int)comboBoxWidth.SelectedItem;
            return data;
        } //UiToData

        void PopulateUi(ModelTop data) {
            checkBoxUseSoundActivation.IsChecked = data.UseSoundActivation;
            textBoxDelay.Text = data.DelayBeforeActivationMs.ToString();
            volumeIndicator.Threshold = data.ActivationThreshold;
            checkBoxAutoRestart.IsChecked = data.AutoRestartOnSave;
            textBoxBaseFileName.Text = data.BaseFileName;
            comboBoxWidth.SelectedItem = data.NumberOfDigitsInFileNumber;
        } //PopulateUi

        OpenFileDialog loadPreferencesDialog = new OpenFileDialog();
        SaveFileDialog savePreferencesDialog = new SaveFileDialog();

    } //class WindowMain

} //namespace SoundRecorder.Windows
