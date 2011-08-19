using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components {

	/// <summary>
	/// AutoComplite.xaml の相互作用ロジック
	/// </summary>
	public partial class AutoCompleteTextBox : UserControl {

		public AutoCompleteTextBox() {
			InitializeComponent();
		}

		#region Dependency Property
		#region Text
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(AutoCompleteTextBox),
			new PropertyMetadata(string.Empty)
		);

		public string Text {
			get {
				return (string)GetValue(TextProperty);
			}
			set {
				SetValue(TextProperty, value);
			}
		}
		#endregion
		#endregion

		#region Method
		private static string GetCurrentWord(TextBox textBox) {
			if (string.IsNullOrEmpty(textBox.Text)) {
				return string.Empty;
			}
			if (textBox.CaretIndex == 0) {
				return string.Empty;
			}

			int index = textBox.CaretIndex - 1;
			int space = textBox.Text.LastIndexOfAny(new[] { ' ', '\r', '\n', '\t' }, index) + 1;

			return textBox.Text.Substring(space, textBox.CaretIndex - space);
		}
		#endregion

		#region Event Handler
		private void TextBox_KeyDown(object sender, KeyEventArgs e) {
			var textBox = sender as TextBox;
			string word = GetCurrentWord(textBox);

			if (e.IsDown) {
				bool isPopupOpenKey = Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Space;
				bool selectKey = e.Key == Key.Tab;
				if (isPopupOpenKey && listBoxTarget.Items.Count > 0) {
					popup.IsOpen = true;
					listBoxTarget.Focus();
					listBoxTarget.SelectedIndex = 0;
					e.Handled = true;
				}
				if (selectKey && listBoxTarget.Items.Count > 0) {
					listBoxTarget_KeyDown(textBox, e);
				}
			} else if (e.IsUp) {
				var provider = FindResource("itemsProvider") as ObjectDataProvider;
				provider.MethodParameters.Clear();
				provider.MethodParameters.Add(word);
				provider.Refresh();
				popup.PlacementTarget = textBox;
				popup.PlacementRectangle = textBox.GetRectFromCharacterIndex(textBox.CaretIndex);
				popup.IsOpen = word.Length > 0 && listBoxTarget.Items.Count > 0;
				if (popup.IsOpen) {
					listBoxTarget.SelectedIndex = 0;
				}
			}
		}

		private void listBoxTarget_KeyDown(object sender, KeyEventArgs e) {
			if (e.Key == Key.Escape || e.Key == Key.Back) {
				popup.IsOpen = false;
				textBox1.Focus();
			} else if (e.Key == Key.Enter || e.Key == Key.Tab || e.Key == Key.Return) {
				if (listBoxTarget.SelectedItem == null) {
					return;
				}
				var textBox = popup.PlacementTarget as TextBox;
				var caretIndex = textBox.CaretIndex;
				var currentWord = GetCurrentWord(textBox);
				var selectedText = listBoxTarget.SelectedItem as string + " ";

				var tmpText = textBox.Text.Remove(caretIndex - currentWord.Length, currentWord.Length);
				textBox.Text = tmpText.Insert(caretIndex - currentWord.Length, selectedText);
				textBox.CaretIndex = caretIndex + selectedText.Length;
				popup.IsOpen = false;
				textBox.Focus();
			}
			e.Handled = true;
		}
		#endregion

	}

}
