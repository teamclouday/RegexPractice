using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace RegexPractice
{
    public partial class MainWindow : Window
    {
        public static int maxLine = 1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            maxLine = 1;
            SCROLLNUM.Document.Blocks.Clear();
            SCROLLNUM.Document.Blocks.Add(new Paragraph(new Run("1")));
            Thickness tk = INPUTBOX.Margin;
            tk.Left = 30;
            INPUTBOX.Margin = tk;
            SCROLLNUM.Width = 20;
            //INPUTBOX.IsReadOnly = false;
            INPUTBOX.Document.Blocks.Clear();
            INPUTBOX.Document.Blocks.Add(new Paragraph(new Run("*Enter Your Data*")));
            INPUTBOX.SpellCheck.IsEnabled = false;
            EDITSTATUS.Visibility = Visibility.Hidden;
            CODEBOX.Clear();
            STATUS.Content = "Not Matching";
            STATUS.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void TryButton_Click(object sender, RoutedEventArgs e)
        {
            //INPUTBOX.IsReadOnly = true;
            STATUS.ClearValue(TextElement.ForegroundProperty);
            TextRange textrange = new TextRange(INPUTBOX.Document.ContentStart, INPUTBOX.Document.ContentEnd);
            textrange.ClearAllProperties();
            string pattern = CODEBOX.Text;
            int resultNum = 0;
            try
            {
                Regex reg = new Regex(pattern, RegexOptions.Compiled);
                var start = INPUTBOX.Document.ContentStart;
                while (start != null && start.CompareTo(INPUTBOX.Document.ContentEnd) < 0)
                {
                    if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));
                        if (match.Success)
                        {
                            var tr = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
                            tr.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                            tr.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
                            tr.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                            resultNum++;
                            start = tr.End;
                        }
                        else
                            break;
                    }
                    start = start.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Exception Occurred!!!\nCheck your code again!!!");
            }
            if (resultNum == 0)
            {
                STATUS.Content = "No Match!";
                STATUS.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else
            {
                STATUS.Content = resultNum.ToString() + " Results Found";
                STATUS.Foreground = new SolidColorBrush(Colors.ForestGreen);
            }
            //MatchCollection matches = Regex.Matches(data, pattern);
            //bool status = Regex.IsMatch(data, pattern);
            //string data = textrange.Text;
            //if(status == true && pattern != "")
            //{
            //    dynamic k = 0;
            //    resultNum = matches.Count;
            //    foreach (Match match in matches)
            //    {
            //        int head = match.Index + k*(match.Length);
            //        if (match.Length > 1)
            //        {
            //            int tail = head + match.Length;
            //            TextPointer start = textrange.Start.GetPositionAtOffset(head);
            //            TextPointer end = textrange.Start.GetPositionAtOffset(tail);
            //            TextRange tx = new TextRange(start, end);
            //            tx.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            //            tx.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            //        }
            //        else
            //        {

            //            TextPointer start = textrange.Start.GetPositionAtOffset(head);
            //            TextPointer end = textrange.Start.GetPositionAtOffset(head+1);
            //            INPUTBOX.Selection.Select(start, end);
            //            TextSelection tx = INPUTBOX.Selection;
            //            tx.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Red));
            //            tx.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            //        }
            //        k++;
            //    }
            //    STATUS.Content = resultNum.ToString() + " Results Found";
            //}
            //else
            //{
            //    STATUS.Content = "No Match!";
            //    STATUS.Foreground = new SolidColorBrush(Colors.Red);
            //}
        }

        private void EXIT_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OPEN_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.InitialDirectory = @"c:\";
            openfile.Filter = @"Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openfile.ShowDialog() == true)
            {
                string data = File.ReadAllText(openfile.FileName);
                if (data.Length >= 99900)
                {
                    MessageBox.Show("Cannot handle so much text!!");
                }
                else
                {
                    INPUTBOX.Document.Blocks.Clear();
                    INPUTBOX.Document.Blocks.Add(new Paragraph(new Run(data)));
                    INPUTBOX.CaretPosition = INPUTBOX.Document.ContentEnd;
                }
            }
        }

        private void SELECTALL_Click(object sender, RoutedEventArgs e)
        {
            INPUTBOX.SelectAll();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            string helpabout = "-Built by Sida Zhu-\n-Date : 2018-3-12-\n-Written In VS 2017-\n-For Regular Expression Practice-";
            MessageBox.Show(helpabout);
        }

        private void CONTACT_Click(object sender, RoutedEventArgs e)
        {
            string helpcontact = "-Email : zhustar1999@163.com-";
            MessageBox.Show(helpcontact);
        }

        private void SPELLCHECK_Click(object sender, RoutedEventArgs e)
        {
            INPUTBOX.SpellCheck.IsEnabled = true;
            EDITSTATUS.Content = "Spell Check Done!";
            EDITSTATUS.Foreground = new SolidColorBrush(Colors.ForestGreen);
            EDITSTATUS.Visibility = Visibility.Visible;
        }

        private void INPUTBOX_TextChanged(object sender, TextChangedEventArgs e)
        {
            INPUTBOX.ClearValue(TextElement.FontWeightProperty);
            INPUTBOX.ClearValue(TextElement.ForegroundProperty);
            TextRange tr = new TextRange(INPUTBOX.Document.ContentStart, INPUTBOX.Document.ContentEnd);
            string data = tr.Text;
            MatchCollection matches = Regex.Matches(data, @"[\w]+");
            long num = matches.Count;
            WORDCOUNT.Content = num.ToString() + " Words  ";
            //Get Line number in the richtextbox...
            //INPUTBOX.CaretPosition = INPUTBOX.CaretPosition.GetLineStartPosition(0);
            TextPointer caretStart = INPUTBOX.CaretPosition.GetLineStartPosition(0);
            TextPointer tp = INPUTBOX.Document.ContentStart.GetLineStartPosition(0);
            int linesCount = 1;
            while(true)
            {
                if (caretStart.CompareTo(tp) < 0)
                    break;
                int result;
                tp = tp.GetLineStartPosition(1, out result);
                if (result == 0)
                    break;
                linesCount++;
            }
            if (linesCount > 9 && linesCount <= 99)
            {
                Thickness tk = INPUTBOX.Margin;
                tk.Left = 40;
                INPUTBOX.Margin = tk;
                SCROLLNUM.Width = 30;

            }
            else if (linesCount > 99 && linesCount < 1000)
            {
                Thickness tk = INPUTBOX.Margin;
                tk.Left = 50;
                INPUTBOX.Margin = tk;
                SCROLLNUM.Width = 40;
            }
            else if (linesCount >= 1000)
            {
                MessageBox.Show("Sorry, I cannot deal with so much text!");
            }
            //Display in Another richtextbox
            while (maxLine < linesCount)
            {
                maxLine++;
                SCROLLNUM.AppendText(Environment.NewLine + maxLine.ToString());
            }
            while (maxLine > linesCount)
            {
                maxLine--;
                SCROLLNUM.Document.Blocks.Remove(SCROLLNUM.Document.Blocks.LastBlock);
            }
        }

        private void ScrollCHANGE_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var textToSync = (sender == INPUTBOX) ? SCROLLNUM : INPUTBOX;

            textToSync.ScrollToVerticalOffset(e.VerticalOffset);
            textToSync.ScrollToHorizontalOffset(e.HorizontalOffset);
        }

        private void Guide_Click(object sender, RoutedEventArgs e)
        {
            string tips = "1. Once click Try, no change can be made in the textbox." +
                "\n2. ClearAll button can reset everything." +
                "\n3. The regular expression input box does not support multiline." +
                "\n4. The SpellCheck is supported by windows itself, not my work." +
                "\n5. Press Enter key will trigger the Try button." +
                "\n6. The text box can only contain at most 999 lines of text. Be careful.";
            MessageBox.Show(tips);
        }

        private void INPUTBOX_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                INPUTBOX.AppendText("\r");
                INPUTBOX.CaretPosition = (INPUTBOX.CaretPosition.GetLineStartPosition(1) != null ? INPUTBOX.CaretPosition.GetLineStartPosition(1) : INPUTBOX.Document.ContentEnd);
                e.Handled = true;
            }
        }

        //private void INPUTBOX_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.SystemKey == Key.Enter)
        //    {
        //        //INPUTBOX.AppendText(Environment.NewLine);
        //        e.Handled = true;
        //    }
        //}

        //private void CODEBOX_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    ClearAllButton.IsEnabled = true;
        //}

        //private void INPUTBOX_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    ClearAllButton.IsEnabled = true;
        //}
    }
}
