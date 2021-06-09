using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Net.Http;

using SnakeGame.ViewModel;
using SnakeGame.GameComponents;


namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModelClass viewModel;
       
        public MainWindow()
        {
            InitializeComponent();
            GameLogic.Rand = new Random();
            GameLogic.timer = new DispatcherTimer();
            GameLogic.timer.Tick += GameLogic.MoveSnake;

            viewModel = new ViewModelClass();
            this.DataContext = viewModel;
            GameLogic.viewModel = viewModel;
            GameLogic.Canvas = BoardCanvas;
            GameLogic.InitializeGame(viewModel.CurrentSpeed.GameSpeed, viewModel.CurrentBoard.BoardSize);
            this.KeyDown += GameLogic.OnKeyDown;

            RoutedCommand newCmd = new RoutedCommand();
            newCmd.InputGestures.Add(new KeyGesture(Key.V, ModifierKeys.Alt));
            CommandBindings.Add(new CommandBinding(newCmd, New_Game_Click));
        }

        // ------------- Event Handlers ---------------
        private void Menu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.NewGame == true) return;
            if (GameLogic.timer.IsEnabled) 
                GameLogic.timer.Stop();
            MessageBoxResult result = MessageBox.Show("Do you want to start new game? Your current points will be lost.", "New Game", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                GameLogic.timer.Start();
            else
                GameLogic.InitializeGame(viewModel.CurrentSpeed.GameSpeed, viewModel.CurrentBoard.BoardSize);
        }


    }
}
