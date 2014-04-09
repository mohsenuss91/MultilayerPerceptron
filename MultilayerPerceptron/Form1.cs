using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MultilayerPerceptron
{
	using System.Configuration;
	using System.Linq;

	public partial class Form1 : Form
	{
		private Bitmap originalImage;
		private Dictionary<string, Bitmap> _originalImages;
//		private NeuralNetwork _neuralNetwork;
	    private Perceptron _perceptron;
	    private readonly Dictionary<string, int> _imageClassDictionary; 

		public Form1()
		{
			InitializeComponent();
			_perceptron = new Perceptron(2, 3, 4);
			//originalImage = null;
			_originalImages = new Dictionary<string, Bitmap>();
            _imageClassDictionary = new Dictionary<string, int>();
			ImageInitialization();
			//_neuralNetwork = new NeuralNetwork();
			//_neuralNetwork.TeachingNeuralNetwork(_originalImages.Values.ToArray());

            foreach (var image in _originalImages)
            {
                _perceptron.TrainedNeural(new ImageMapper().ToDouble(image.Value),
                    _imageClassDictionary[image.Key]);
            }
		}

		private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void buttonGenerateNoise_Click(object sender, EventArgs e)
		{
			var appSetting = ConfigurationManager.AppSettings;
			var key = appSetting[comboBoxLetter.SelectedItem.ToString()];
			int percentOfNoise;
			int.TryParse(comboBoxPercentOfNoise.SelectedItem.ToString(), out percentOfNoise);
			originalImage = new Bitmap(_originalImages[key]);

			pictureBoxOriginal.Image = NoiseGenerator.MakeNoisy(originalImage, percentOfNoise);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			originalImage = new Bitmap(_originalImages[ConfigurationManager.AppSettings[comboBoxLetter.SelectedItem.ToString()]]);
			pictureBoxOriginal.Image = originalImage;
		}

		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBoxLetter.SelectedIndex != 0 && comboBoxPercentOfNoise.SelectedIndex != 0)
			{
				buttonGenerateNoise.Enabled = true;
			}
		}

		private void buttonStart_Click(object sender, EventArgs e)
		{
			//pictureBoxResult.Image = _neuralNetwork.RecognizeImage(originalImage);
		    var result = _perceptron.GetNeuronResult(new ImageMapper().ToDouble(originalImage));
            Console.WriteLine(result);
		}

		private void ImageInitialization()
		{
			var appSettings = ConfigurationManager.AppSettings;
		    int classIndex = 0;
			foreach (var letter in comboBoxLetter.Items)
			{
				var key = appSettings[letter.ToString()];
				_originalImages[key] = new Bitmap("../../Content/" + key + ".png");

                _imageClassDictionary.Add(key, classIndex++);
			}
		}
	}
}
