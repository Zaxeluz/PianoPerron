using System.Collections.Generic;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;

namespace NAudioPianoDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PlaybackEngine playbackEngine;

        private readonly Dictionary<Key, string> noteNames = new Dictionary<Key, string>
        {
            {Key.Z, "C"},
            {Key.S, "C#"},
            {Key.X, "D"},
            {Key.D, "D#"},
            {Key.C, "E"},
            {Key.V, "F"},
            {Key.G, "F#"},
            {Key.B, "G"},
            {Key.H, "G#"},
            {Key.N, "A"},
            {Key.J, "A#"},
            {Key.M, "B"},
        };

        public MainWindow()
        {
            InitializeComponent();
            playbackEngine = new PlaybackEngine();
            PianoControl.NoteOn += OnNoteOn;
            PianoControl.NoteOff += OnNoteOff;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            string noteName;
            if (noteNames.TryGetValue(keyEventArgs.Key, out noteName))
            {
                playbackEngine.StopNote(noteName);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.IsRepeat) return;
            string noteName;
            if (noteNames.TryGetValue(keyEventArgs.Key, out noteName))
            {
                playbackEngine.StartNote(noteName);
            }
        }

        void OnNoteOff(object sender, NoteEventArgs e)
        {
            playbackEngine.StopNote(e.NoteName);
        }

        private void OnNoteOn(object sender, NoteEventArgs e)
        {
            playbackEngine.StartNote(e.NoteName);
        }
    }
}
