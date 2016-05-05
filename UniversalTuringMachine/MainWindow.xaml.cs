using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using UniversalTuringMachine.Models;

namespace UniversalTuringMachine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string machineFileText = "";
        string inputFileText = "";
        string output = "";
        List<TransitionRule> transitionRules;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void chooseMachineFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                machineFileTextBlock.Text = filename;
                System.IO.Stream fileStream = dlg.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    machineFileText = reader.ReadLine();
                }
                fileStream.Close();
            }
        }

        private void chooseInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                inputFileTextBlock.Text = filename;
                System.IO.Stream fileStream = dlg.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    inputFileText = reader.ReadLine();
                    output = inputFileText;
                }
                fileStream.Close();
            }
        }

        private void runButton_Click(object sender, RoutedEventArgs e)
        {
            if (machineFileText == "" || inputFileText == "")
            {
                MessageBox.Show("Invalid Input");
                return;
            }
            //First We Separate Each Rule (Each Rule is Separated by 11)
            string[] rules = machineFileText.Split(new string[] { "11" }, StringSplitOptions.None);
            transitionRules = new List<TransitionRule>();
            //Then We Make a new Transition Rule Object for each rule
            for (int i = 0; i < rules.Length - 1; i++)
            {
                TransitionRule tr = new TransitionRule();
                string[] components = rules[i].Split('1');
                tr.currentState = components[0];
                tr.currentChar = components[1];
                tr.nextState = components[2];
                tr.nextChar = components[3];
                tr.direction = components[4];
                transitionRules.Add(tr);
            }
            //Then we make string object for each input character
            string[] inputs = inputFileText.Split('1');
            string[] outputs = new string[inputs.Length];
            inputs.CopyTo(outputs, 0);
            //Now All Configurations are set, we begin now to execute the Machine on the Given Input

            bool isFinished = false;
            //Assume the first input is $ so we start at index 0
            int currentIndex = 0;
            //We assume we start at q0 that equals 00
            string currentState = "00";
            TransitionRule currentRule = new TransitionRule();
            for (int i = 0; i < transitionRules.Count; i++)
            {
                if(transitionRules[i].currentChar==inputs[currentIndex] && transitionRules[i].currentState==currentState)
                {
                    currentRule = transitionRules[i];
                    break;
                }
            }
            if (currentRule.currentChar == null || currentRule.currentChar == "")
                isFinished = true;
            if (!isFinished)
            {
                outputs[currentIndex] = currentRule.nextChar;
                if (currentRule.direction == "000")
                {
                    currentIndex++;
                }
                else if (currentRule.direction == "00")
                {
                    currentIndex--;
                }
                currentState = currentRule.nextState;
                if (currentState == "0")
                    isFinished = true;
            }
            while (!isFinished)
            {
                bool isFound = true;
                currentRule = new TransitionRule();
                for (int i = 0; i < transitionRules.Count; i++)
                {
                    if (transitionRules[i].currentChar == inputs[currentIndex] && transitionRules[i].currentState == currentState)
                    {
                        currentRule = transitionRules[i];
                        break;
                    }
                    
                }
                if (currentRule.currentChar == null || currentRule.currentChar == "")
                    isFinished = true;
                if (!isFinished)
                {
                    outputs[currentIndex] = currentRule.nextChar;
                    if (currentRule.direction == "000")
                    {
                        currentIndex++;
                    }
                    else if (currentRule.direction == "00")
                    {
                        currentIndex--;
                    }
                    currentState = currentRule.nextState;
                    if (currentState == "0")
                        isFinished = true;
                }
            }
            outputTextBlock.Text = "";
            for(int i=0;i<outputs.Length;i++)
            {
                if(i!=outputs.Length-1)
                    outputTextBlock.Text += outputs[i] + "1";
                else
                    outputTextBlock.Text += outputs[i];
            }
        }
    }
}
