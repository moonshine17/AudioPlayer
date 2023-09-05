using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace SimpleAudioPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private DispatcherTimer timer = new DispatcherTimer();
        private ObservableCollection<string> playlist = new ObservableCollection<string>();
        private int currentIndex = -1;
        private bool isDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();

            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Добавьте пути к аудио файлам в плейлист
            playlist.Add("C:\\Users\\furao\\Music\\Dido-Thank You -SLOW - REVERB--kissvk.com.mp3");
            playlist.Add("C:\\Users\\furao\\Music\\The Chords-Sh-Boom -Life Could Be a Dream--kissvk.com.mp3");
            playlist.Add("C:\\Users\\furao\\Music\\Смешарики-Тема уютности-kissvk.com.mp3");
            // И так далее...

            playlistView.ItemsSource = playlist;

            // Запускаем анимацию фона
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            sliderPosition.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingSlider)
            {
                sliderPosition.Value = mediaPlayer.Position.TotalSeconds;
            }

            // Обновляем состояние анимации на основе позиции воспроизведения аудио
            Storyboard animationStoryboard = (Storyboard)FindResource("backgroundAnimation");
            DoubleAnimation animation = (DoubleAnimation)animationStoryboard.Children[0];
            animation.From = 0;
            animation.To = 400 * (sliderPosition.Value / sliderPosition.Maximum);
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (currentIndex >= 0 && currentIndex < playlist.Count)
            {
                mediaPlayer.Open(new Uri(playlist[currentIndex]));
                mediaPlayer.Play();
                timer.Start();
            }
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
            timer.Stop();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
            mediaPlayer.Position = TimeSpan.Zero;
        }

        private void playlistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentIndex = playlistView.SelectedIndex;
        }

        private void sliderPosition_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!isDraggingSlider)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            }
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sliderVolume.Value;
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (currentIndex < playlist.Count - 1)
            {
                currentIndex++;
                btnPlay_Click(null, null); // Начинаем воспроизведение следующего трека в плейлисте
            }
            else
            {
                mediaPlayer.Stop();
                timer.Stop();
                mediaPlayer.Position = TimeSpan.Zero;
            }
        }

        private void sliderPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDraggingSlider = true;
            mediaPlayer.Pause(); // При начале перемотки, останавливаем воспроизведение.
        }

        private void sliderPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDraggingSlider = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(sliderPosition.Value);
            mediaPlayer.Play(); // При окончании перемотки, начинаем воспроизведение с новой позиции.
        }
    }
}
