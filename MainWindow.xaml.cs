using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Elevator_M42_Echipa4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Elevator MyElevator;

        public MainWindow()
        {
            InitializeComponent();

            Uri iconUri = new Uri("pack://application:,,,/elevator.ico", UriKind.Absolute);

            this.Icon = BitmapFrame.Create(iconUri);

            Slider.IsEnabled = false;

            MyElevator = new Elevator { CurrentPosition = 0d, State = ElevatorStates.Idle, ElevatorSpeed = 0.01d , isElevatorStateGoingDown = false , isElevatorStateGoingUp = false, isElevatorStateStopped = false, isElevatorStateIdle = true };

            this.DataContext = MyElevator;

        }

        #region Buttons

        private void FloorButton0_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(0);
        }

        private void FloorButton1_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(1);
        }

        private void FloorButton2_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(2);
        }

        private void FloorButton3_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(3);
        }

        private void FloorButton4_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(4);
        }

        private void FloorButton5_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();
            MoveSlider(5);
        }

        private void Button_Validate_Speed(object sender, RoutedEventArgs e)
        {
            string SpeedOfElevator = TextBox_Speed.Text;
            var isNumeric = double.TryParse(SpeedOfElevator, out double speed);
            if (!isNumeric)
            {
                TextBox_Speed.Text = "0.1";
                MessageBox.Show("Enter a valid number between (0-0.25) !");
            }else
            {
                if(speed>0d && speed <= 0.25d)
                {
                    string tempString = "Validated at "+ speed.ToString();
                    MessageBox.Show(tempString);
                    MyElevator.ElevatorSpeed = speed * Math.Pow(10, -1);
                }
                else
                {
                    TextBox_Speed.Text = "0.25";
                    speed = 0.25;
                    MessageBox.Show("Input has to be lower of equal to the maximum speed !");
                    MyElevator.ElevatorSpeed = speed * Math.Pow(10, -1);
                }
            }
        }

        private void FloorButtonStop_Click(object sender, RoutedEventArgs e)
        {
            StopElevator();
        }

        #endregion

        #region EventHandlers

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            MyElevator.State = ElevatorStates.Stopped;
        }

        #endregion

        #region HelperFunctions

        private void DisableUI()
        {
            FloorButton0.IsEnabled = false;
            FloorButton1.IsEnabled = false;
            FloorButton2.IsEnabled = false;
            FloorButton3.IsEnabled = false;
            FloorButton4.IsEnabled = false;
            FloorButton5.IsEnabled = false;
        }

        private void EnableUI()
        {
            FloorButton0.IsEnabled = true;
            FloorButton1.IsEnabled = true;
            FloorButton2.IsEnabled = true;
            FloorButton3.IsEnabled = true;
            FloorButton4.IsEnabled = true;
            FloorButton5.IsEnabled = true;
        }

        private async void MoveSlider(int destinationPos)
        {
            double elevatorSpeed = MyElevator.ElevatorSpeed;
            MyElevator.determineDirection(MyElevator, destinationPos);
            ElevatorStates ECurrentState = MyElevator.State;
            MyElevator.isElevatorStateStopped = false;
            MyElevator.isElevatorStateIdle = false;
            switch (ECurrentState)
            {
                case ElevatorStates.Idle:
                    MyElevator.isElevatorStateIdle = true;
                    await Task.Delay(300);
                    EnableUI();
                    return;
                case ElevatorStates.GoingUp:
                    MyElevator.isElevatorStateGoingUp = true;
                    await MoveUp(elevatorSpeed, destinationPos);
                    MyElevator.isElevatorStateGoingUp = false;
                    break;
                case ElevatorStates.GoingDown:
                    MyElevator.isElevatorStateGoingDown = true;
                    await MoveDown(elevatorSpeed, destinationPos);
                    MyElevator.isElevatorStateGoingDown = false;
                    break;
                case ElevatorStates.Stopped:
                    MyElevator.isElevatorStateStopped = true;
                    StopElevator();
                    MyElevator.isElevatorStateStopped = false;
                    break;
            }
            EnableUI();
            return;
        }

        private async Task MoveUp(double speed, int destinationPos)
        {
            MyElevator.isElevatorStateGoingUp = true;
            while (Slider.Value < destinationPos)
            {
                if (MyElevator.State == ElevatorStates.Stopped)
                {
                    await Task.Delay(1);
                    return;
                }
                Slider.Value += speed;
                MyElevator.CurrentPosition += speed;
                UpdateCurrentPositionLabel(MyElevator.CurrentPosition);
                await Task.Delay(1);
            }
            Slider.Value = Math.Round(Slider.Value, 0);
            CurrentPositionLabel.Content = $"Current position : {Slider.Value}";
            MyElevator.CurrentPosition = destinationPos;
            MyElevator.State = ElevatorStates.Idle;
            MyElevator.isElevatorStateIdle = true;
        }


        private async Task MoveDown(double speed, int destinationPos)
        {
            while (Slider.Value > destinationPos)
            {
                if (MyElevator.State == ElevatorStates.Stopped)
                {
                    await Task.Delay(1);
                    return;
                }
                Slider.Value -= speed;
                MyElevator.CurrentPosition -= speed;
                UpdateCurrentPositionLabel(MyElevator.CurrentPosition);
                await Task.Delay(1);
            }
            Slider.Value = Math.Round(Slider.Value, 0);
            CurrentPositionLabel.Content = $"Current position : {Slider.Value}";
            MyElevator.CurrentPosition = destinationPos;
            MyElevator.State = ElevatorStates.Idle;
            MyElevator.isElevatorStateIdle = true;
        }

        
        private void StopElevator()
        {
            MyElevator.isElevatorStateStopped = true;
            MyElevator.isElevatorStateIdle = false;
            MyElevator.State = ElevatorStates.Stopped;
        }

        private void UpdateCurrentPositionLabel(double value)
        {
            CurrentPositionLabel.Content = $"Current position : {Math.Round(value, 2)}";
        }



        #endregion

    }
}
