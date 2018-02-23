using System.Collections.Generic;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.CoreAudioApi;
using System.IO;

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

        private WasapiOut waveOut;
        private MixingSampleProvider mixer;
        private Dictionary<string, ISampleProvider> mixerInputs =
            new Dictionary<string, ISampleProvider>();
        private Dictionary<string, byte[]> sampleData =
            new Dictionary<string, byte[]>();

        public PlaybackEngine()
        {
            LoadSampleData();
            //Ahora será la misma instancia todo el tiempo
            waveOut = new WasapiOut(AudioClientShareMode.Shared, 50);
            //Inicializar el mixer
            mixer =
                new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1));
            mixer.ReadFully = true;
            waveOut.Init(mixer);
            waveOut.Play();
        }

        void LoadSampleData()
        {
            foreach(var kvp in noteFiles)
            {
                var reader =
                    new WaveFileReader("samples\\" + kvp.Value);
                var data = new byte[reader.Length];
                reader.Read(data, 0, (int)reader.Length);
                sampleData[kvp.Key] = data;
            }
        }

        public void StartNote(string note)
        {
            byte[] data;
            if (sampleData.TryGetValue(note, out data))
            {
                var sampleStream =
                    new RawSourceWaveStream(
                        new MemoryStream(data),
                        new WaveFormat(44100, 32, 1));
                var sampleProvider =
                    sampleStream.ToSampleProvider();
                mixerInputs[note] = sampleProvider;
                mixer.AddMixerInput(sampleProvider);
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