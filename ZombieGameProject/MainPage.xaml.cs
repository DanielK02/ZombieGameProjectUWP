using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZombieGameProject.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ZombieGameProject
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Game _game;
        AudioEngine _audio = new AudioEngine();
 


        public MainPage()
        {
            this.InitializeComponent();
            _game = new Game(Playground, LivesTB, GameWonContentDialog, GameLostContentDialog);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            _audio.PlayMenuMusic();
        }


        #region Command bar buttons
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _game.gameStart();
            _audio.StopMenuMusic();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_game.IsPause == false) //Pause
            {
                _game.IsPause = true;
                _game.gamePause();
            }
            else // Continue
            {
                _game.IsPause = false;
                _game.gamePause();
            } 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _game.gameSave();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            _game.gameLoad();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
        #endregion

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    if (_game.IsPlay)
                        _game.PlayerMoveUp();
                    break;
                case Windows.System.VirtualKey.Down:
                    if (_game.IsPlay == true)
                        _game.PlayerMoveDown();
                    break;
                case Windows.System.VirtualKey.Left:
                    if (_game.IsPlay == true)
                        _game.PlayerMoveLeft();
                    break;
                case Windows.System.VirtualKey.Right:
                    if (_game.IsPlay == true)
                        _game.PlayerMoveRight();
                    break;
                case Windows.System.VirtualKey.R:
                    if (_game.IsPlay == true)
                        _game.PlayerRandom();
                    break;
                case Windows.System.VirtualKey.Space:
                    if (_game.IsPlay == true)
                    {
                        _game.ShootAndBulletCollision();
                        if (_game.BulletActive == false)
                        {
                            _audio.PlayGunshot();
                            _game.BulletActive = true;
                        }
                        else
                            return;
                    }
                    break;
            }
        }
    }
}
