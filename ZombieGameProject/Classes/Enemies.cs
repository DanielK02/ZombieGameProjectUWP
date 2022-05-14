using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ZombieGameProject.Classes
{
    class Enemies : Creature
    {

        double _startTop;
        double _startLeft;
        int _size = 45;
        int _enemySpeed = 5;
        int path;
        int _zombieNumber;
        Random enemyImageRND = new Random();

        public Enemies(Canvas canvas, int startLeft, int startTop)
        {
            path = enemyImageRND.Next(0, 4);
            _startLeft = startLeft;
            _startTop = startTop;
            _creaCanvas = canvas;
            _creaImage = new Image();
            _creaImage.Source = new BitmapImage(new Uri(_creaImgSource = "ms-appx:///Assets/Zombie" + path + "/enemyDead.png"));
            _creaImage.Height = _size;
            _creaImage.Width = _size;
            _zombieNumber = path;
            Canvas.SetLeft(_creaImage, _startLeft);
            Canvas.SetTop(_creaImage, _startTop);

            _creaCanvas.Children.Add(_creaImage);
        }
        public int GetEnemySize
        {
            get { return _size; }
        }

        #region Override methods
        public override void MoveUp()
        {
            if (Canvas.GetTop(_creaImage) <= _enemySpeed)
                return;
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage) - _enemySpeed);
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage));

            if(_creaImgSource != "ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyUp.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyUp.png"));
            }
            
        }
        public override void MoveDown()
        {
            if (Canvas.GetTop(_creaImage) + _enemySpeed + _creaImage.Height > _creaCanvas.ActualHeight)
                return;
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage) + _enemySpeed);
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyDown.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyDown.png"));
            }

        }
        public override void MoveLeft()
        {
            if (Canvas.GetLeft(_creaImage) <= _enemySpeed)
                return;
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage) - _enemySpeed);
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyLeft.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyLeft.png"));
            }

        }
        public override void MoveRight()
        {
            if (Canvas.GetLeft(_creaImage) + _enemySpeed + _creaImage.Width > _creaCanvas.ActualWidth)
                return;
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage) + _enemySpeed);
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyRight.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Zombie" + _zombieNumber + "/enemyRight.png"));
            }


        }
        #endregion


    }
}
