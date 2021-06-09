using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using SnakeGame.GameComponents;

namespace SnakeGame.ViewModel
{
    // Base for all classes implementing INotifyPropertyChanged
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase() { }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void Dispose() { this.OnDispose(); }
        protected virtual void OnDispose() { }
    }


    // Class for Board Size Option
    public class BoardSizeClass : ViewModelBase
    {
        public BoardSizeEnum BoardSize { get; set; }
        public string Title { get; set; }

        private bool _isChecked = false;

        public BoardSizeClass()
        {
            IsChecked = false;
        }
        
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
    }


    // Class for Game Speed Option
    public class GameSpeedClass : ViewModelBase
    {
        public GameSpeedEnum GameSpeed { get; set; }
        public string Title { get; set; }

        private bool _isChecked = false;

        public GameSpeedClass()
        {
            IsChecked = false;
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }
    }


    // ------- Class holding all binded parameters ------
    public class ViewModelClass : ViewModelBase
    {
        private int _score;
        private int _bestScore;
        private bool _newGame;
        private BitmapImage _snakeBitmap;
        private BoardSizeClass _currentBoard;
        private GameSpeedClass _currentSpeed;
        private ObservableCollection<BoardSizeClass> _boardCollection = new ObservableCollection<BoardSizeClass>();
        private ObservableCollection<GameSpeedClass> _speedCollection = new ObservableCollection<GameSpeedClass>();

        public ViewModelClass()
        {
            Score = BestScore = 0;
            NewGame = true;
            foreach (BoardSizeEnum value in Enum.GetValues(typeof(BoardSizeEnum)))
            {
                BoardSizeClass myClass = new BoardSizeClass();
                myClass.BoardSize = value;
                myClass.IsChecked = value == BoardSizeEnum.Medium ? true : false; // default to using Medium board
                myClass.Title = Enum.GetName(typeof(BoardSizeEnum), value);
                if (myClass.IsChecked == true)
                    CurrentBoard = myClass;
                _boardCollection.Add(myClass);
            }
            foreach (GameSpeedEnum value in Enum.GetValues(typeof(GameSpeedEnum)))
            {
                GameSpeedClass myClass = new GameSpeedClass();
                myClass.GameSpeed = value;
                myClass.IsChecked = value == GameSpeedEnum.Normal ? true : false; // default to using Normal speed
                myClass.Title = Enum.GetName(typeof(GameSpeedEnum), value);
                if (myClass.IsChecked == true) 
                    CurrentSpeed = myClass;
                _speedCollection.Add(myClass);
            }
        }

        // Public properties
        public ObservableCollection<BoardSizeClass> BoardCollection
        {
            get { return _boardCollection; }
        }
        public ObservableCollection<GameSpeedClass> SpeedCollection
        {
            get { return _speedCollection; }
        }
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                NotifyPropertyChanged("Score");
            }
        }
        public int BestScore
        {
            get { return _bestScore; }
            set
            {
                _bestScore = value;
                NotifyPropertyChanged("BestScore");
            }
        }
        public bool NewGame
        {
            get { return _newGame; }
            set
            {
                _newGame = value;
                NotifyPropertyChanged("NewGame");
            }
        }
        public BoardSizeClass CurrentBoard
        {
            get { return _currentBoard; }
            set
            {
                _currentBoard = value;
                NotifyPropertyChanged("CurrentBoard");
            }
        }
        public GameSpeedClass CurrentSpeed
        {
            get { return _currentSpeed; }
            set
            {
                _currentSpeed = value;
                NotifyPropertyChanged("CurrentSpeed");
            }
        }
        public BitmapImage SnakeBitmap
        {
            get { return _snakeBitmap; }
            set
            {
                _snakeBitmap = value;
                NotifyPropertyChanged("SnakeBitmap");
            }
        }

        // Handling commands
        private ICommand _myCommand;
        public ICommand MyCommand
        {
            get
            {
                if (_myCommand == null) 
                    _myCommand = new RelayCommand(new Action<object>(CheckedItemChanged));
                return _myCommand;
            }
        }

        private void CheckedItemChanged(object checkedItem)
        {
            if (checkedItem is BoardSizeEnum)
            {
                BoardSizeEnum myEnum = (BoardSizeEnum)checkedItem;
                ObservableCollection<BoardSizeClass> collection = _boardCollection;
                BoardSizeClass theClass = collection.First<BoardSizeClass>(t => t.BoardSize == myEnum);

                foreach (BoardSizeClass iter in collection)
                    iter.IsChecked = false;
                theClass.IsChecked = true;
                CurrentBoard = theClass;
            }
            else if (checkedItem is GameSpeedEnum)
            {
                GameSpeedEnum myEnum = (GameSpeedEnum)checkedItem;
                ObservableCollection<GameSpeedClass> collection = _speedCollection;
                GameSpeedClass theClass = collection.First<GameSpeedClass>(t => t.GameSpeed == myEnum);

                foreach (GameSpeedClass iter in collection)
                    iter.IsChecked = false;
                theClass.IsChecked = true;
                CurrentSpeed = theClass;
            }
            GameLogic.InitializeGame(CurrentSpeed.GameSpeed, CurrentBoard.BoardSize);
        }
    }



    public class RelayCommand : ICommand
    {
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) { }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }


}
