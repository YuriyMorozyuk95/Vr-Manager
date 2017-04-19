using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace VrManager.Controls
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        private TimeSpan? _selectedTime;
        public TimeSpan? SelectedTime
        {
            get
            {
                return _selectedTime;
            }
            set
            {
                _selectedTime = value;

                if (value == null)
                {
                    //HourBox.SelectedIndex = -1;
                    MinuteBox.SelectedIndex = -1;
                    SecondBox.SelectedIndex = -1;
                }
                else
                {
                    string hour = convetToString(value.Value.Hours);
                    string minutes = convetToString(value.Value.Minutes);
                    string second = convetToString(value.Value.Seconds);

                    //HourBox.SelectedItem = hour;
                    MinuteBox.SelectedItem = minutes;
                    SecondBox.SelectedItem = second;
                }
            }
        }


        public TimePicker()
        {
            InitializeComponent();

            //setTimeSourse(0, 23, 1, HourBox);
            setTimeSourse(0, 59, 1, MinuteBox);
            setTimeSourse(0, 59, 5, SecondBox);

            //HourBox.SelectionChanged += TimeChanged;
            MinuteBox.SelectionChanged += TimeChanged;
            SecondBox.SelectionChanged += TimeChanged;
        }


        private void setTimeSourse(int minValue, int maxValue, int step, ComboBox receiver)
        {
            List<string> timeSourse = new List<string>();

            for (int i = minValue; i <= maxValue; i+= step)
            {
                string number = convetToString(i);
                timeSourse.Add(number);
            }

            receiver.ItemsSource = timeSourse;
            receiver.SelectedIndex = 0;
        }

        private void TimeChanged(object sender, SelectionChangedEventArgs e)
        {
            //int hours = Int32.Parse(HourBox.SelectedItem as string);
            int minutes = Int32.Parse(MinuteBox.SelectedItem as string);
            int second = Int32.Parse(SecondBox.SelectedItem as string);

            SelectedTime = new TimeSpan(/*hours*/ 0 , minutes, second);
        }

        private string convetToString(int i)
        {
            string number = i.ToString();

            if (number.Count() == 1)
            {
                number = "0" + number;
            }

            return number;
        }
    }

}
