using System;
using System.IO;
using QRCoder;
using static QRCoder.PayloadGenerator;

namespace QrCodeGeneratorFileToQr
{
	class Program
	{
		private static byte[] png { get; set; }

		static void Main(string[] args)
		{
			Console.WriteLine(args[0]);
			bool isText = false;
			Payload payload = null;
			string filePath = "";
			string file = "";
			string dataType = "";
			if (args.Length != 1)
			{
				return;
			}
			else
			{
				filePath = args[0];
				dataType = filePath.Split('.')[filePath.Split('.').Length - 1];
				file = File.ReadAllText(filePath);
			}

			switch (dataType)
			{
				case "txt":
					isText = true;
					break;
				case "sms":
					break;
				default:
					break;
			}

			QRCodeGenerator generator = new QRCodeGenerator();
			QRCodeData data;
			if (!isText)
				data = generator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
			else
				data = generator.CreateQrCode(file, QRCodeGenerator.ECCLevel.Q);
			PngByteQRCode pngQrCode = new PngByteQRCode(data);

			File.WriteAllBytes($"{filePath}.png", pngQrCode.GetGraphic(20));
		}
	}
}
