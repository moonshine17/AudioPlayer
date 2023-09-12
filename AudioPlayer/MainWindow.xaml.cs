using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SimpleAudioPlayer
{
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private DispatcherTimer timer = new DispatcherTimer();
        //private ObservableCollection<SimpleAudioPlayer.Song> playlist = new ObservableCollection<SimpleAudioPlayer.Song>();
        private ObservableCollection<string> playlist = new ObservableCollection<string>();
        List<string> сокращенныеНазвания = new List<string>();
        private int currentIndex = -1;
        private bool isDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();

            videoPlayer.Play();
            videoPlayer.MediaEnded += VideoPlayer_MediaEnded;


            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;



            // Добавьте пути к аудио файлам в плейлист
            playlist.Add("C:\\Users\\furao\\Music\\Dido-Thank You -SLOW - REVERB--kissvk.com.mp3"); 
            playlist.Add("C:\\Users\\furao\\Music\\The Chords-Sh-Boom -Life Could Be a Dream--kissvk.com.mp3"); 
            playlist.Add("C:\\Users\\furao\\Music\\Смешарики-Тема уютности-kissvk.com.mp3");
            //// И так далее...

            foreach (string полноеНазвание in playlist) // вашПлейлист - это список полных названий песен
            {
                string сокращенноеНазвание = ReplaceName(полноеНазвание); // Используйте вашу функцию для сокращения
                сокращенныеНазвания.Add(сокращенноеНазвание);
            }
            playlistView.ItemsSource = сокращенныеНазвания;
        }

        public string ReplaceName(string FullName)
        {
            FullName = FullName.ReplaceFirst("C:\\Users\\furao\\Music\\", "");
            FullName = FullName.ReplaceFirst("C:\\Users\\furao\\Downloads\\", ""); // Удаляем путь
            FullName = FullName.ReplaceFirst("--kissvk.com.mp3", ""); // Удаляем завершение файла
            FullName = FullName.ReplaceFirst("-kissvk.com.mp3", ""); // Удаляем завершение файла
            return FullName;
        }
        private void VideoPlayer_MediaStart(object sender, EventArgs e)
        {
            videoPlayer.Play();
        }
        private void VideoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Воспроизводим видео снова с начала при его завершении
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
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
            if(sliderPosition.Value == sliderPosition.Maximum)
            {
                if (currentIndex < playlist.Count - 1)
                {
                    currentIndex++;
                    btnPlay_Click(null, null);

                }
                else
                {
                    mediaPlayer.Stop();
                    timer.Stop();
                    mediaPlayer.Position = TimeSpan.Zero;
                }
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

        private void AddSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true; // Разрешаем выбирать несколько файлов
            openFileDialog.Filter = "MP3 файлы (*.mp3)|*.mp3|Все файлы (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                // Получаем список выбранных файлов и добавляем их в ваш плейлист
                foreach (string filename in openFileDialog.FileNames)
                {
                    // Добавьте filename в ваш плейлист
                    // Например: вашПлейлист.Add(filename);
                    playlist.Add(filename);
                }
            }
            foreach (string полноеНазвание in playlist) // вашПлейлист - это список полных названий песен
            {
                string сокращенноеНазвание = ReplaceName(полноеНазвание); 
                bool содержитЭлемент = сокращенныеНазвания.Contains(сокращенноеНазвание);
                if(содержитЭлемент == true)
                {
                    continue;
                }
                else{
                    сокращенныеНазвания.Add(сокращенноеНазвание);
                }
            }
            playlistView.ItemsSource = сокращенныеНазвания;
            ICollectionView view = CollectionViewSource.GetDefaultView(playlistView.ItemsSource);
            view.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation heightAnimation = new DoubleAnimation();
            DoubleAnimation opacityAnimation = new DoubleAnimation();

            if (sliderVolume.Visibility == Visibility.Collapsed)
            {
                // Анимация появления слайдера
                sliderVolume.Visibility = Visibility.Visible;

                heightAnimation.From = 0;
                heightAnimation.To = sliderVolume.ActualHeight; // Используем текущую высоту слайдера
                opacityAnimation.From = 0;
                opacityAnimation.To = 1; // Изменяем прозрачность на 1 для полной видимости
            }
            else
            {
                // Анимация исчезновения слайдера
                heightAnimation.From = sliderVolume.ActualHeight; // Используем текущую высоту слайдера
                heightAnimation.To = 0;
                opacityAnimation.From = 1; // Изменяем прозрачность на 1 для полной видимости
                opacityAnimation.To = 0;

                // После окончания анимации скрыть слайдер
                heightAnimation.Completed += (sen, eventArgs) =>
                {
                    sliderVolume.Visibility = Visibility.Collapsed;
                };
            }

            // Продолжительность анимации
            Duration duration = new Duration(TimeSpan.FromSeconds(0.5));

            // Применение анимации высоты слайдера
            Storyboard heightStoryboard = new Storyboard();
            heightStoryboard.Children.Add(heightAnimation);
            Storyboard.SetTarget(heightAnimation, sliderVolume);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath("Height"));
            heightStoryboard.Duration = duration;
            
            // Применение анимации прозрачности
            Storyboard opacityStoryboard = new Storyboard();
            opacityStoryboard.Children.Add(opacityAnimation);
            Storyboard.SetTarget(opacityAnimation, sliderVolume);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));
            opacityStoryboard.Duration = duration;

            // Запуск анимации
            heightStoryboard.Begin();
            opacityStoryboard.Begin();
        }
    }
    public static class StringExtensions
    {
        public static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }
}
