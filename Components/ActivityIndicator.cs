/*
Sound Recorder

Copyright (C) by Sergey A Kryukov, 2014
http://www.SAKryukov.org
*/

namespace SoundRecorder.Components {
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Windows.Controls;

    public partial class ActivityIndicator : UserControl {

        static class DefinitionSet {
            internal const double StrokeThickness = 1;
            internal static readonly Brush High = Brushes.Red;
            internal static readonly Brush Low = SystemColors.ControlLightLightBrush;
            internal static readonly Brush Inactive = SystemColors.ControlBrush;
        } //class DefinitionSet

        public ActivityIndicator() {
            Content = ellipse;
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = DefinitionSet.Inactive;
            ellipse.StrokeThickness = DefinitionSet.StrokeThickness;
            ellipse.HorizontalAlignment = HorizontalAlignment.Stretch;
            ellipse.VerticalAlignment = VerticalAlignment.Stretch;
        } //Indicator

        public bool IsActive {
            get { return isActive; }
            set {
                if (value == isActive) return;
                isActive = value;
                if (isActive) {
                    state = true; // to start with red color
                    Flash();
                } else
                    ellipse.Fill = DefinitionSet.Inactive;
            } //set IsActive
        } //IsActive

        internal void Flash() {
            if (isActive) {
                if (state)
                    ellipse.Fill = DefinitionSet.High;
                else
                    ellipse.Fill = DefinitionSet.Low;
                state = !state;
            } //if
        } //Flash

        Ellipse ellipse = new Ellipse();
        bool state, isActive;

    } //class ActivityIndicator

} //namespace SoundRecorder.Components