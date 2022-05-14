using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ZombieGameProject.Classes
{
    public class Game
    {
        #region Game Class members

        Canvas _canvas;
        TextBlock _textBlockLives; //Displays Lives
        Player _player1;
        Enemies[] enemyArray = new Enemies[10];
        DispatcherTimer _timer; //Game timer
        DispatcherTimer _timerbullet; //BulletTimer
        Bullet _bullet;
        int _enemyLives; //Variable for the number of enemies alive
        bool _gameLoaded = false; //Boolean so the loaded game won't be overwritten by gameStart
        int _bulletDirection = 0; //0 = Up, 1 = Down, 2 = Left, 3 = Right
        int bulletFly = 0; //For keeping a constant direction while bullet != null
        AudioEngine _audioEngine = new AudioEngine();
        bool _bulletOnCanvas = false; // Bool to make sure sound won't be repeated of gunfire while bullet is on
        Random random = new Random();
        ContentDialog _contentDialogGameLost = new ContentDialog();
        ContentDialog _contentDialogGameWon = new ContentDialog();


        #endregion

        //Constructor
        public Game(Canvas canvas, TextBlock textblockLives, ContentDialog cDialogWin, ContentDialog cDialogLost)
        {
            _canvas = canvas;
            _timer = new DispatcherTimer();
            _timerbullet = new DispatcherTimer();
            _textBlockLives = textblockLives;
            _contentDialogGameWon = cDialogWin;
            _contentDialogGameLost = cDialogLost;
        }


        #region Game class properties
        public bool IsPlay { get; internal set; }
        public bool IsPause { get; internal set; }
        public bool BulletActive // To prevent audio file playing a few times while you can't shoot
        {
            get { return _bulletOnCanvas; }
            set { _bulletOnCanvas = value; }
        } 
        #endregion


        #region Game command bar methods
        public void gameStart()
        {
            IsPlay = true;
            IsPause = false;
            _audioEngine.StopMenuMusic();
            _audioEngine.StopBgMusic();
            _canvas.Children.Clear();
            if (_gameLoaded == false)
            {
                _player1 = new Player(_canvas);
                for (int i = 0; i < enemyArray.Length; i++)
                {
                    enemyArray[i] = new Enemies(_canvas, random.Next(0, (int)_canvas.ActualWidth), random.Next(0, 500));
                }
                _enemyLives = enemyArray.Length;

            }
            _audioEngine.PlayBackgroundMusic();

            _timer.Tick -= TickHandler;
            _timer.Tick += TickHandler;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 90);
            _timer.Start();

            _timerbullet.Tick -= ShootTick;
            _timerbullet.Tick += ShootTick;
            _timerbullet.Interval = new TimeSpan(0, 0, 0, 0, 1);

            _gameLoaded = false;
        }
        public void gamePause()
        {
            switch (IsPause)
            {
                case true:
                    IsPlay = false;
                    _timer.Stop();
                    _audioEngine.PauseBgMusic();
                    break;
                case false:
                    IsPlay = true;
                    _timer.Start();
                    _audioEngine.ResumePlayingBGMusic();
                    break;
                default:
                    break;
            }

        }
        public async void gameSave()
        {

            if(_enemyLives > 1)
            {
                _timer.Stop();
                int nullEnemy = 0; //Will avoid saving null at X,Y Positions of the save file
                string[] SaveFile = new string[(_enemyLives + 2) * 2 - 1];

                //Logic so it won't save null enemies into file (If it does, it causes issues and doesn't work properly)
                for (int i = 0; i < enemyArray.Length; i++)
                {
                    if(enemyArray[i] == null)
                    {
                        nullEnemy += 2;
                        continue;
                    }
                    else if (i == 0)
                    {
                        SaveFile[i * 2] = enemyArray[i].GetSetCreaLeft.ToString();
                        SaveFile[i * 2 + 1] = enemyArray[i].GetSetCreaTop.ToString();
                    }
                    else
                    {
                        SaveFile[i * 2 - nullEnemy] = enemyArray[i].GetSetCreaLeft.ToString();
                        SaveFile[i * 2 + 1 - nullEnemy] = enemyArray[i].GetSetCreaTop.ToString();
                    }
                }
                SaveFile[SaveFile.Length - 2] = _player1.GetSetCreaLeft.ToString(); //Saves player left
                SaveFile[SaveFile.Length - 1] = _player1.GetSetCreaTop.ToString(); // Saves player Top
                SaveFile[SaveFile.Length - 3] = _player1.GetPlayerLives.ToString(); // Player Lives
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.CreateFileAsync("Save.txt", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteLinesAsync(sampleFile, SaveFile);
                if(IsPause != true)
                {
                    _timer.Start();
                }
            }
        }
        public async void gameLoad()
        {
            _gameLoaded = true;

            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync("Save.txt");
            IList<String> LoadFile = await FileIO.ReadLinesAsync(sampleFile);

            int length = LoadFile.Count / 2 - 2;

            gameStart();

            Enemies[] enemyArrayLoad = new Enemies[length];
            Player _playerLoad = new Player(_canvas);
            int DeadEnemies = enemyArray.Length - enemyArrayLoad.Length; //To get correct dead enemies number
            _playerLoad.GetSetCreaLeft = int.Parse(LoadFile[LoadFile.Count - 2]);
            _playerLoad.GetSetCreaTop = int.Parse(LoadFile[LoadFile.Count - 1]);

            for (int i = 0; i < length; i++)
            {
                enemyArrayLoad[i] = new Enemies(_canvas, int.Parse(LoadFile[i * 2]), int.Parse(LoadFile[i * 2 + 1]));
                enemyArray[i] = enemyArrayLoad[i];
            }
            // To make sure the dead enemies are null and there aren't invisible enemies on canvas:
            for (int i = enemyArray.Length - 1; i > enemyArrayLoad.Length - 1; i--) 
            {
                enemyArray[i] = null;
            }

            _player1 = _playerLoad;
            _player1.GetPlayerLives = int.Parse(LoadFile[LoadFile.Count - 3]);
            _enemyLives = enemyArray.Length - DeadEnemies;

            _gameLoaded = false;

        }

        #endregion

        #region Dispatch Timer Ticks
        private void TickHandler(object sender, object e)
        {
            EnemyChasePlayer();
            PlayerEnemyCollision();

            if (_enemyLives == 0)
            {
                GameWon();
            }
            if (_player1.GetPlayerLives == 0 || _player1.GetPlayerLives < 0)
            {
                GameLost();
            }
            _textBlockLives.Text = $"HP: {_player1.GetPlayerLives * 5}";

        }
        private void ShootTick(object sender, object e)
        {
            ShootAndBulletCollision();
        }
        #endregion

        #region Game keyboard methods
        public void PlayerMoveUp()
        {
            _player1.MoveUp();
            _bulletDirection = 0;
        }
        public void PlayerMoveDown()
        {
            _player1.MoveDown();
            _bulletDirection = 1;
        }
        public void PlayerMoveLeft()
        {
            _player1.MoveLeft();
            _bulletDirection = 2;

        }
        public void PlayerMoveRight()
        {
            _player1.MoveRight();
            _bulletDirection = 3;

        }
        public void PlayerRandom()
        {
            _player1.MoveRandom();
        }
        public void ShootAndBulletCollision()
        {
            _timerbullet.Start();

            bool bulletCollision = false;
            int direction = _bulletDirection;
            if(_bullet == null)
            {
                switch (direction)
                {
                    case 0:
                        int pointX = (int)Canvas.GetLeft(_player1.CreaImage) + ((int)_player1.CreaImage.Width / 2 - 3);
                        int pointY = (int)Canvas.GetTop(_player1.CreaImage);
                        _bullet = new Bullet(direction, pointX, pointY);
                        _canvas.Children.Add(_bullet.BulletImage);
                        bulletFly = 0;
                        break;
                    case 1:
                        pointX = (int)Canvas.GetLeft(_player1.CreaImage) + 10;
                        pointY = (int)Canvas.GetTop(_player1.CreaImage) + (int)_player1.CreaImage.Height + 10;
                        _bullet = new Bullet(direction, pointX, pointY);
                        _canvas.Children.Add(_bullet.BulletImage);
                        bulletFly = 1;
                        break;
                    case 2:
                        pointX = (int)Canvas.GetLeft(_player1.CreaImage) - 10;
                        pointY = (int)Canvas.GetTop(_player1.CreaImage) + 25;
                        _bullet = new Bullet(direction, pointX, pointY);
                        _canvas.Children.Add(_bullet.BulletImage);
                        bulletFly = 2;
                        break;
                    case 3:
                        pointX = (int)Canvas.GetLeft(_player1.CreaImage) + (int)_player1.CreaImage.Width / 2 + 10;
                        pointY = (int)Canvas.GetTop(_player1.CreaImage) + 50;
                        _bullet = new Bullet(direction, pointX, pointY);
                        _canvas.Children.Add(_bullet.BulletImage);
                        bulletFly = 3;
                        break;
                    default:
                        break;
                }
            }

            switch (bulletFly) //While bullet is initialized
            {
                case 0:
                    Canvas.SetTop(_bullet.BulletImage, _bullet.yPoint -= _bullet.BulletSpeed);
                    break;
                case 1:
                    Canvas.SetTop(_bullet.BulletImage, _bullet.yPoint += _bullet.BulletSpeed);
                    break;
                case 2:
                    Canvas.SetLeft(_bullet.BulletImage, _bullet.xPoint - _bullet.BulletSpeed);
                    break;
                case 3:
                    Canvas.SetLeft(_bullet.BulletImage, _bullet.xPoint + _bullet.BulletSpeed);
                    break;
                default:
                    break;
            }
            
            // Bullet collision with enemy
            for (int i = 0; i < enemyArray.Length; i++)
            {
                if(enemyArray[i] == null)
                {
                    continue;
                }
                if (_bullet.xPoint >= enemyArray[i].GetSetCreaLeft &&
                    _bullet.xPoint <= enemyArray[i].GetSetCreaLeft + enemyArray[i].CreaImage.Width
                    && _bullet.yPoint >= enemyArray[i].GetSetCreaTop 
                    && _bullet.yPoint <= enemyArray[i].GetSetCreaTop + enemyArray[i].CreaImage.Height) //Analyze this
                {
                    _canvas.Children.Remove(enemyArray[i].CreaImage);
                    enemyArray[i] = null;
                    _enemyLives--;
                    bulletCollision = true;
                }
            }


            if (_bullet.yPoint < 0 || _bullet.yPoint > _canvas.ActualHeight || _bullet.xPoint < 0 || _bullet.xPoint > _canvas.ActualWidth || bulletCollision == true)
            {
                _timerbullet.Stop();
                _canvas.Children.Remove(_bullet.BulletImage);
                _bullet = null;
                BulletActive = false;
            }

        }
        #endregion

        #region Game logic methods
        private void EnemyChasePlayer()
        {
            for (int i = 0; i < enemyArray.Length; i++)
            {
                if(enemyArray[i] == null)
                {
                    continue;
                }
                if (_player1.GetSetCreaTop < enemyArray[i].GetSetCreaTop)
                {
                    enemyArray[i].MoveUp();
                }
                else if (_player1.GetSetCreaTop > enemyArray[i].GetSetCreaTop)
                {
                    enemyArray[i].MoveDown();
                }
                if (_player1.GetSetCreaLeft > enemyArray[i].GetSetCreaLeft)
                {
                    enemyArray[i].MoveRight();
                }
                else if (_player1.GetSetCreaLeft < enemyArray[i].GetSetCreaLeft)
                {
                    enemyArray[i].MoveLeft();
                }
            }
        }
        private void PlayerEnemyCollision()
        {
            int size = 60;
            for (int i = 0; i < enemyArray.Length; i++)
            {
                if (enemyArray[i] == null)
                {
                    continue;
                }
                if (_player1.GetSetCreaTop > enemyArray[i].GetSetCreaTop - size && _player1.GetSetCreaTop < enemyArray[i].GetSetCreaTop + size
                    && _player1.GetSetCreaLeft > enemyArray[i].GetSetCreaLeft - size && _player1.GetSetCreaLeft < enemyArray[i].GetSetCreaLeft + size)
                {
                    _player1.GetPlayerLives--;
                }
            }

        }
        private async void GameWon()
        {
            _timer.Stop();
            _timerbullet.Stop();
            IsPlay = false;
            IsPause = true;
            _audioEngine.StopBgMusic();
            await _contentDialogGameWon.ShowAsync();
            
            //new MessageDialog("win").ShowAsync();
        }
        private async void GameLost()
        {
            _timer.Stop();
            _timerbullet.Stop();
            IsPlay = false;
            IsPause = true;
            _audioEngine.StopBgMusic();

            await _contentDialogGameLost.ShowAsync();
        }

        #endregion
    }

}
