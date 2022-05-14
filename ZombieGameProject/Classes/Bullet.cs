using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ZombieGameProject.Classes
{
    public class Bullet
    {
        private Image _bulletImage;
        private string _bltImgSrc;
        private int _bulletspeed;
        private int _size = 13;
        private int _bulletDirection = 0;


        public Bullet(int bulletDirection, double xPos, double yPos)
        {
            _bulletDirection = bulletDirection;
            _bulletImage = new Image();
            _bulletImage.Source = new BitmapImage(new Uri(_bltImgSrc = "ms-appx:///Assets/bulletUp.png"));
            _bulletspeed = 20;
            _bulletImage.Height = _size;
            _bulletImage.Width = _size;
            Canvas.SetLeft(_bulletImage, xPos + 10);
            Canvas.SetTop(_bulletImage, yPos - 10);

            switch (bulletDirection)
            {
                case 0:
                    _bulletImage.Source = new BitmapImage(new Uri(_bltImgSrc = "ms-appx:///Assets/bulletUp.png"));
                    break;
                case 1:
                    _bulletImage.Source = new BitmapImage(new Uri(_bltImgSrc = "ms-appx:///Assets/bulletDown.png"));
                    break;
                case 2:
                    _bulletImage.Source = new BitmapImage(new Uri(_bltImgSrc = "ms-appx:///Assets/bulletLeft.png"));
                    break;
                case 3:
                    _bulletImage.Source = new BitmapImage(new Uri(_bltImgSrc = "ms-appx:///Assets/bulletRight.png"));
                    break;
                default:
                    break;
            }

        }
        #region Bullet properties
        public Image BulletImage
        {
            get { return _bulletImage; }
            set { _bulletImage = value; }
        }
        public string BulletImageSource
        {
            get { return _bltImgSrc; }
            set { _bltImgSrc = value; }
        }
        public double xPoint
        {
            get { return Canvas.GetLeft(_bulletImage); }
            set { Canvas.SetLeft(_bulletImage, _bulletspeed); }
        }
        public double yPoint
        {
            get { return Canvas.GetTop(_bulletImage); }
            set { Canvas.SetTop(_bulletImage, _bulletspeed); }
        }
        public int BulletDirection
        {
            get { return _bulletDirection; }
            set { _bulletDirection = value; }
        }
        public int BulletSpeed
        {
            get { return _bulletspeed; }
        }

        #endregion


    }
}
