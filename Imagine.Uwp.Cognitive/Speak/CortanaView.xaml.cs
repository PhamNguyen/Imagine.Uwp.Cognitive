using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Imagine.Uwp.Cognitive
{
    public sealed partial class CortanaView : UserControl
    {
        private String key;

        public String Key
        {
            get { return key; }
            set
            {
                key = value;
            }
        }

        private Language language = Language.English;

        public Language Language
        {
            get { return language; }
            set
            {
                language = value;
                OnLanguageChanged();
            }
        }

        private void OnLanguageChanged()
        {
            switch (Language)
            {
                case Language.English:
                    code = SupportedLanguage.English;
                    break;
                case Language.French:
                    code = SupportedLanguage.French;
                    break;
                case Language.Russian:
                    code = SupportedLanguage.Russian;
                    break;
                case Language.German:
                    code = SupportedLanguage.German;
                    break;
                case Language.Spanish:
                    break;
                default:
                    code = "en-US";
                    break;
            }
        }
        
        private String code = "en-US";

        private Synthesize cortana;

        public CortanaView()
        {
            this.InitializeComponent();
            this.Loaded += CortanaView_Loaded;
        }

        public void Speak(String text)
        {
            if (cortana == null)
                return;
            OnLanguageChanged();
            cortana.Speak(text, code);
        }

        public void Speak(String text, Language language)
        {
            if (cortana == null)
                return;
            this.Language = language;
            Speak(text);
        }

        private async void CortanaView_Loaded(object sender, RoutedEventArgs e)
        {
            cortana = await Synthesize.Create(Key);

            cortana.OnAudioAvailable += PlayAudio;
        }

        private void PlayAudio(object sender, GenericEventArgs<Stream> e)
        {
            var stream = new MemoryStream();

            e.EventData.CopyTo(stream);
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MediaPlayer.SetSource(stream.AsRandomAccessStream(), ".wav");
                MediaPlayer.Play();
            });

        }
    }
}
