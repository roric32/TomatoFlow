using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace TomatoFlow
{
    /// <summary>
    /// Interaction logic for TomatoWindow.xaml
    /// </summary>
    public partial class TomatoWindow : Window
    {
        private const int scoreBeforeParty = 4;
        private readonly BitmapImage _studyGraphic;
        private readonly BitmapImage _restGraphic;
        private readonly BitmapImage _neutralGraphic;
        private readonly BitmapImage _partyGraphic;
        private readonly DispatcherTimer _tomatoTimer;
        private readonly Uri _studySound;
        private readonly Uri _restSound;
        private readonly Uri _partySound;
        private readonly Uri _pauseSound;
        private readonly System.Media.SoundPlayer _soundPlayer;
        private State _state;
        private int _workTime;
        private int _breakTime;
        private int _partyTime;
        private int _score = 0;
        private int _minutesToUse;
        private int _counter = 0;
        public int Score
        {
            get { return this._score; }
            set { this._score = value; }
        }

        public int WorkTime
        {
            get { return this._workTime; }
            set { this._workTime = value; }
        }

        public int BreakTime
        {
            get { return this._breakTime; }
            set { this._breakTime = value; }
        }

        public int PartyTime
        {
            get { return this._partyTime; }
            set { this._partyTime = value; }
        }

        public enum State
        {
            NEUTRAL,WORK,BREAK,PARTY
        }

        private void ChangeState(State state, Boolean fromButton = false)
        {
            if (this._tomatoTimer.IsEnabled) this._tomatoTimer.Stop();

            ProgressBar.Value = 0;
            _counter = 0;

            this._state = state;

            switch(state)
            {
                case State.WORK:
                    this.PlaySound(_studySound);
                    TomatoGraphic.Source = this._studyGraphic; 
                    this.Pause_Tomato.IsEnabled = true;
                    this.Neutral_Tomato.IsEnabled = true;
                    this._minutesToUse = this.WorkTime;
                    ProgressBar.Maximum = this._minutesToUse * 60 + 1;
                    ToggleProgressBar(true);
                    _tomatoTimer.Start();
                    break;
                case State.BREAK:
                    this.PlaySound(_restSound);
                    TomatoGraphic.Source = this._restGraphic;
                    this.Pause_Tomato.IsEnabled = true;
                    this.Neutral_Tomato.IsEnabled = true;
                    this._minutesToUse = this.BreakTime;
                    ProgressBar.Maximum = this._minutesToUse * 60 + 1;
                    ToggleProgressBar(true);
                    _tomatoTimer.Start();
                    break;
                case State.PARTY:
                    this.PlaySound(_partySound);
                    TomatoGraphic.Source = this._partyGraphic;
                    if(fromButton) this.UpdateScore(0);
                    this.Pause_Tomato.IsEnabled = true;
                    this.Neutral_Tomato.IsEnabled = true;
                    this._minutesToUse = this.PartyTime;
                    ProgressBar.Maximum = this._minutesToUse * 60 + 1;
                    ToggleProgressBar(true);
                    _tomatoTimer.Start();
                    break;
                case State.NEUTRAL:
                    this._soundPlayer.Stop();
                    TomatoGraphic.Source = this._neutralGraphic;
                    this.UpdateScore(0);
                    this.Pause_Tomato.IsEnabled = false;
                    this.Neutral_Tomato.IsEnabled = false;
                    ToggleProgressBar(false);
                    break;
                default:
                    break;
            }

        }

        private void PlaySound(Uri soundUri)
        {
            this._soundPlayer.SoundLocation = soundUri.ToString();
            this._soundPlayer.Load();
            this._soundPlayer.Play();
        }

        private void ToggleProgressBar(Boolean on)
        {
            ProgressBar.Visibility = (on) ? Visibility.Visible : Visibility.Hidden;
        }

        private void UpdateScore(int newValue)
        {
            this.Score = newValue;
            BindingExpression binding = lblScore.GetBindingExpression(Label.ContentProperty);
            binding.UpdateTarget();
        }

        private void TogglePause(Boolean turnOn, Button button)
        {
            List<Button> buttons = new List<Button>{Working_Tomato, Neutral_Tomato, Resting_Tomato, Party_Tomato};

            foreach(Button buttonToChange in buttons)
            {
                buttonToChange.IsEnabled = (!turnOn);
            }

            if(turnOn)
            {
                button.Content = "Resume";
                TomatoGraphic.Opacity = 0.5;
                ProgressBar.Opacity = 0.5;
                _tomatoTimer.Stop();

            } else
            {
                button.Content = "Pause";
                TomatoGraphic.Opacity = 1;
                ProgressBar.Opacity = 1;
                _tomatoTimer.Start();
            }


        }

        private void TomatoTimer_Tick(object sender, EventArgs e)
        {
            if(_counter >= (60 * this._minutesToUse))
            {
                if(this._state == State.WORK)
                {
                    this.UpdateScore(Score+1);
                    State newState = (this.Score % scoreBeforeParty == 0) ? State.PARTY : State.BREAK;
                    this.ChangeState(newState);

                } else
                {
                    this.ChangeState(State.WORK);
                }

            } else
            {
                _counter++;
                ProgressBar.Value++;
            }
        }

        private void BtnWorking_Tomato_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeState(State.WORK);
        }

        private void BtnResting_Tomato_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeState(State.BREAK);
        }

        private void BtnNeutral_Tomato_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeState(State.NEUTRAL);
        }

        private void BtnParty_Tomato_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeState(State.PARTY, true);
        }

        private void BtnPause_Tomato_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySound(_pauseSound);
            Button pauseButton = (sender as Button);
            Boolean pauseNow = (pauseButton.Content.ToString() == "Pause") ? true : false;
            this.TogglePause(pauseNow, pauseButton);
        }

        private void BtnExit_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionsWindow = new OptionsWindow();
            optionsWindow.Show();
        }

        private void BtnAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TomatoFlow is free software. \n" +
                "Feel free to redistribute!", "About TomatoFlow");
        }

        public TomatoWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this._state = State.NEUTRAL;
            this._studyGraphic = new BitmapImage(new Uri("Resources/TomatoStudyFinal.png", UriKind.Relative));
            this._studySound = new Uri("Resources/Study.wav", UriKind.Relative);

            this._restGraphic = new BitmapImage(new Uri("Resources/TomatoRestFinal.png", UriKind.Relative));
            this._restSound = new Uri("Resources/Break.wav", UriKind.Relative);

            this._neutralGraphic = new BitmapImage(new Uri("Resources/TomatoNeutralFinal.png", UriKind.Relative));

            this._partyGraphic = new BitmapImage(new Uri("Resources/TomatoPartyFinal.png", UriKind.Relative));
            this._partySound = new Uri("Resources/Party.wav", UriKind.Relative);

            this._pauseSound = new Uri("Resources/Pause.wav", UriKind.Relative);

            this.WorkTime = Properties.Settings.Default.WorkTime;
            this.BreakTime = Properties.Settings.Default.BreakTime;
            this.PartyTime = Properties.Settings.Default.PartyTime;
            
            this._soundPlayer = new System.Media.SoundPlayer();

            this._tomatoTimer = new DispatcherTimer();
            this._tomatoTimer.Tick += new EventHandler(TomatoTimer_Tick);
            this._tomatoTimer.Interval = new TimeSpan(0, 0, 1);
        }

    }
}
