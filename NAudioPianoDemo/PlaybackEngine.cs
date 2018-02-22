using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace NAudioPianoDemo
{
    class PlaybackEngine
    {
        private readonly Dictionary<string, string> noteFiles = new Dictionary<string, string>()
        {
            {"C", "P1D V105 C4.wav"},
            {"D#", "P1D V105 Eb4.wav"},
            {"F#", "P1D V105 Gb4.wav"},
            {"A", "P1D V105 A4.wav"},
        };

        private WaveOut waveOut;
        private MixingSampleProvider mixer;
        private Dictionary<string, ISampleProvider> mixerInputs =
            new Dictionary<string, ISampleProvider>();

        public PlaybackEngine()
        {
            //Ahora será la misma instancia todo el tiempo
            waveOut = new WaveOut();
            //Inicializar el mixer
            mixer =
                new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            mixer.ReadFully = true;
            waveOut.Init(mixer);
            waveOut.Play();
        }

        public void StartNote(string note)
        {
            string file;
            if (noteFiles.TryGetValue(note, out file))
            {
                var reader = new AudioFileReader("samples\\" + file);
                mixerInputs[note] = reader;
                mixer.AddMixerInput((ISampleProvider)reader);
            }
        }

        public void StopNote(string noteName)
        {
            ISampleProvider mixerInput;
            if (mixerInputs.TryGetValue(noteName, out mixerInput))
            {
                mixer.RemoveMixerInput(mixerInput);
            }           
        }
    }
}