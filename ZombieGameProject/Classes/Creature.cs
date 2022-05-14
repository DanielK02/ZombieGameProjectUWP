using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ZombieGameProject.Classes
{
    class Creature //Base class for player and enemies
    {

        #region Class members

        protected Canvas _creaCanvas;
        protected Image _creaImage;
        protected string _creaImgSource;
        //private bool _isAlive;
        protected int _movementSpeed = 10;
        protected int _creaLives;
        Random rnd = new Random();

        #endregion


        #region Creature class properties


        public Image CreaImage // Image of creature
        {
            get { return _creaImage; }
            set { _creaImage = value; }
        }
        public double GetSetCreaTop
        {
            get { return Canvas.GetTop(_creaImage); }
            set { Canvas.SetTop(_creaImage, value); }
        }
        public double GetSetCreaLeft
        {
            get { return Canvas.GetLeft(_creaImage); }
            set { Canvas.SetLeft(_creaImage, value); }
        }
        public string GetCreaImgSource
        {
            get { return _creaImgSource; }
            set { _creaImgSource = value; }
        }
        #endregion


        #region Creature class methods
        public virtual void MoveUp()
        {
            if (Canvas.GetTop(_creaImage) <= _movementSpeed)
                return;
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage) - _movementSpeed);
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage));
            
            if(_creaImgSource != "ms-appx:///Assets/playerUp.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playerUp.png"));
            }
        }
        public virtual void MoveDown()
        {
            if (Canvas.GetTop(_creaImage) + _movementSpeed + _creaImage.Height > _creaCanvas.ActualHeight - _creaImage.Height - _movementSpeed)
                return;
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage) + _movementSpeed);
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/playerDown.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playerDown.png"));
            }
        }
        public virtual void MoveLeft()
        {
            if (Canvas.GetLeft(_creaImage) <= _movementSpeed)
                return;
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage) - _movementSpeed);
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/playerLeft.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playerLeft.png"));
            }
        }
        public virtual void MoveRight()
        {
            if (Canvas.GetLeft(_creaImage) + _movementSpeed + _creaImage.Width > _creaCanvas.ActualWidth)
                return;
            Canvas.SetLeft(_creaImage, Canvas.GetLeft(_creaImage) + _movementSpeed);
            Canvas.SetTop(_creaImage, Canvas.GetTop(_creaImage));

            if (_creaImgSource != "ms-appx:///Assets/playerRight.png")
            {
                _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playerRight.png"));
            }
        }
        public void MoveRandom()
        {
            Canvas.SetLeft(_creaImage, rnd.Next((int)_creaCanvas.ActualWidth - (int)_creaImage.ActualWidth));
            Canvas.SetTop(_creaImage, rnd.Next((int)_creaCanvas.ActualHeight - (int)_creaImage.ActualHeight));
        }

        #endregion

    }
}
