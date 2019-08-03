using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Biblioteka.Windows
{
	/// <summary>
	/// Interaction logic for CustomMessageBox.xaml
	/// </summary>
	public partial class CustomMessageBox : Window
	{
		//TODO: debug

		private static MessageBoxResult Result = MessageBoxResult.OK;
		private static CustomMessageBox MessageBox;

		public enum CustomMessageBoxIcon
		{
			Information,
			Warning,
			Error
		}

		private static void SetIconOfCustomMessageBox(CustomMessageBoxIcon icon)
		{
			switch (icon)
			{
				case CustomMessageBoxIcon.Information:
					MessageBox.MessageBoxPackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.InformationVariant;
					break;

				case CustomMessageBoxIcon.Warning:
					MessageBox.MessageBoxPackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WarningOutline;
					break;

				case CustomMessageBoxIcon.Error:
					MessageBox.MessageBoxPackIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Exclamation;
					break;
			}
		}

		public CustomMessageBox()
		{
			InitializeComponent();
		}

		public static MessageBoxResult Show(string title, string content, CustomMessageBoxIcon icon)
		{
			MessageBox = new CustomMessageBox()
			{
				TitleBox = { Text = title },
				ContentBox = { Text = content }
			};
			SetIconOfCustomMessageBox(icon);

			MessageBox.ShowDialog();

			return Result;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			Result = MessageBoxResult.OK;
			MessageBox.Close();
			MessageBox = null;
		}

		private void ExitButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.Black);
		}

		private void ExitButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
		{
			ExitButton.Foreground = new SolidColorBrush(Colors.DarkSlateGray);
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			Result = MessageBoxResult.OK;
			MessageBox.Close();
			MessageBox = null;
		}

		private void TopRectangle_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				this.DragMove();
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				OkButton.Click += OkButton_Click;
			}
		}
	}
}
