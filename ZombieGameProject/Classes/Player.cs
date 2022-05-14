using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ZombieGameProject.Classes
{
    class Player : Creature
    {

        double _startTop = 800;
        //double _startLeft = 300;
        int _size = 70;

        public Player(Canvas canvas)
        {
            _creaCanvas = canvas;
            _creaImage = new Image();
            _creaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/playerIdle.png"));

            _creaImage.Height = _size;
            _creaImage.Width = _size;
            _creaLives = 20;

            Canvas.SetLeft(_creaImage, canvas.ActualWidth / 2);
            Canvas.SetTop(_creaImage, _startTop);

            _creaCanvas.Children.Add(_creaImage);
        }

        public int GetPlayerLives
        {
            get { return _creaLives; }
            set { _creaLives = value; }
        }

    }
}