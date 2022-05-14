using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace ZombieGameProject.Classes
{
    public class AudioEngine
    {
        private MediaElement _gameMusic = new MediaElement();
        private MediaElement _menuMusic = new MediaElement();


        public async Task<MediaElement> PlayGunshot()
        {
            var GunShotElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
            var file = await folder.GetFileAsync("AR15Gunshot.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            GunShotElement.SetSource(stream, "");
            GunShotElement.Play();
            GunShotElement.Volume = 0.2;  
            return GunShotElement;
        }
        public async Task<MediaElement> PlayMenuMusic()
        {
            var menuMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
            var file = await folder.GetFileAsync("MainMusic.mp3");
            var stream = await file.OpenAsync(FileAccessMode.Read);
            menuMusicElement.SetSource(stream, "");
            menuMusicElement.Play();
            menuMusicElement.Volume = 0.2;
            _menuMusic = menuMusicElement;
            return menuMusicElement;
        }

        public async Task<MediaElement> PlayBackgroundMusic()
        {
            var BgMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Audio");
            var file = await folder.GetFileAsync("GameMusic.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BgMusicElement.SetSource(stream, "");
            BgMusicElement.Play();
            BgMusicElement.Volume = 0.2;  
            _gameMusic = BgMusicElement;
            return BgMusicElement;

        }

        public void StopBgMusic()
        {
            _gameMusic.Stop();

        }     
        public void PauseBgMusic()
        {
            _gameMusic.Pause();
        }
        public void StopMenuMusic()
        {
            _menuMusic.Stop();
        }
        public MediaElement ResumePlayingBGMusic()  // Resumes music from paused moment
        {
            var BgMusicElement = new MediaElement();
            BgMusicElement = _gameMusic;
            BgMusicElement.Play();
            return BgMusicElement;
        }
    }
}

