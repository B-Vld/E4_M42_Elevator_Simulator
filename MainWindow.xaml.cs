using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            Slider.IsEnabled = false;

            MyElevator = new Elevator { CurrentPosition = 0d, State = (ElevatorStates)2, ElevatorSpeed = 0.01d };

        }

        List<Task> TaskList = new List<Task>();

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

        private void FloorButtonStop_Click(object sender, RoutedEventArgs e)
        {
            StopElevator();
        }

        #endregion

        #region EventHandlers

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            MyElevator.State = (ElevatorStates)3;
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
            switch (ECurrentState)
            {
                case ElevatorStates.Idle:
                    await Task.Delay(500);
                    EnableUI();
                    return;
                case ElevatorStates.GoingUp:
                    await MoveUp(elevatorSpeed, destinationPos);
                    break;
                case ElevatorStates.GoingDown:
                    await MoveDown(elevatorSpeed, destinationPos);
                    break;
            }
            EnableUI();
            return;
        }

        private async Task MoveUp(double speed, int destinationPos)
        {
            while (Slider.Value < destinationPos)
            {
                if (MyElevator.State == ElevatorStates.Stopped || MyElevator.State==ElevatorStates.Idle)
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
        }


        private async Task MoveDown(double speed, int destinationPos)
        {
            while (Slider.Value > destinationPos)
            {
                if (MyElevator.State == ElevatorStates.Stopped || MyElevator.State == ElevatorStates.Idle)
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
        }

        private void StopElevator()
        {
            MyElevator.State = (ElevatorStates)3;
        }

        private void UpdateCurrentPositionLabel(double value)
        {
            CurrentPositionLabel.Content = $"Current position : {Math.Round(value, 2)}";
        }

        #endregion

    }
}
