using Microsoft.Win32;
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
using QRCoder;
using Newtonsoft.Json;
using static QRCoder.PayloadGenerator;
using System.IO;

namespace QrCodeGenerator
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private byte[] png { get; set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		private void EncodeBtn_Click(object sender, RoutedEventArgs e)
		{
			bool isText = false;

			Payload payload = null;
			if ((bool)PayLoadTypeUrlRad.IsChecked)
				payload = new Url(DataTb.Text);

			else if ((bool)PayLoadTypeSmsRad.IsChecked)
			{
				Dictionary<string, string> smsArgs =
					JsonConvert.DeserializeObject<Dictionary<string, string>>(DataTb.Text);
				payload = new SMS(smsArgs["num"], smsArgs["msg"]);
			}

			else if ((bool)PayLoadTypeWifiRad.IsChecked)
			{
				Dictionary<string, string> wifiArgs = 
					JsonConvert.DeserializeObject<Dictionary<string, string>>(DataTb.Text);
				WiFi.Authentication auth;

				switch (wifiArgs["auth"])
				{
					case "wpa":
						auth = WiFi.Authentication.WPA;
						break;
					case "wep":
						auth = WiFi.Authentication.WEP;
						break;
					case "nopass":
					default:
						auth = WiFi.Authentication.nopass;
						break;
				}
				payload = new WiFi(wifiArgs["ssid"], wifiArgs["pass"], auth);
			}
			else if ((bool)PayLoadTypeWathsappMessageRad.IsChecked)
			{
				Dictionary<string, string> whatsappArgs =
					JsonConvert.DeserializeObject<Dictionary<string, string>>(DataTb.Text);
				payload = new WhatsAppMessage(whatsappArgs["num"], whatsappArgs["msg"]);
			}
			else
			{
				isText = true;
			}

			QRCodeGenerator generator = new QRCodeGenerator();
			QRCodeData data;
			if (!isText)
				data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
			else 
				data = generator.CreateQrCode(DataTb.Text, QRCodeGenerator.ECCLevel.Q);
			PngByteQRCode pngQrCode = new PngByteQRCode(data);
			png = pngQrCode.GetGraphic(20);
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			if (dialog.ShowDialog() == true)
				File.WriteAllBytes(dialog.FileName, png);
		}
	}
}
