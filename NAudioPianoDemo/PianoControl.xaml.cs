using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NAudioPianoDemo
{
    /// <summary>
    /// Interaction logic for PianoControl.xaml
    /// </summary>
    public partial class PianoControl : UserControl
    {
        private readonly Dictionary<Rectangle, string> noteNames;

        public event EventHandler<NoteEventArgs> NoteOn;
        public event EventHandler<NoteEventArgs> NoteOff;

        protected virtual void OnNoteOff(NoteEventArgs e)
        {
            EventHandler<NoteEventArgs> handler = NoteOff;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnNoteOn(NoteEventArgs e)
        {
            EventHandler<NoteEventArgs> handler = NoteOn;
            if (handler != null) handler(this, e);
        }

        public PianoControl()
        {
            InitializeComponent();
            noteNames = new Dictionary<Rectangle, string>()
            {
                { RectangleC, "C"},
                { RectangleCSharp, "C#"},
                { RectangleD, "D"},
                { RectangleDSharp, "D#"},
                { RectangleE, "E"},
                { RectangleF, "F"},
                { RectangleFSharp, "F#"},
                { RectangleG, "G"},
                { RectangleGSharp, "G#"},
                { RectangleA, "A"},
                { RectangleASharp, "A#"},
                { RectangleB, "B"},
            };
            foreach (var fe in noteNames)
            {
                fe.Key.MouseDown += OnKeyMouseDown;
                fe.Key.MouseUp += OnKeyMouseUp;
            }
        }

        private void OnKeyMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var rect = sender as Rectangle;
            if (rect != null)
            {
                string noteName;
                if (noteNames.TryGetValue(rect, out noteName))
                {
                    rect.Fill = Brushes.Yellow;
                    OnNoteOn(new NoteEventArgs(noteName));
                }
            }
        }

        void OnKeyMouseUp(object sender, MouseButtonEventArgs e)
        {
            var rect = sender as Rectangle;
            if (rect != null)
            {
                string noteName;
                if (noteNames.TryGetValue(rect, out noteName))
                {
                    rect.Fill = noteName.Length == 1 ? Brushes.White : Brushes.Black;
                    OnNoteOff(new NoteEventArgs(noteName));
                }
            }
        }
    }

    public class NoteEventArgs : EventArgs
    {
        private readonly string noteName;

        public NoteEventArgs(string noteName)
        {
            this.noteName = noteName;
        }

        public string NoteName
        {
            get { return noteName; }
        }
    }
}
