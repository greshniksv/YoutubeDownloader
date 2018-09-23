using System;
using System.Windows.Forms;
using System.IO;
using VideoLibrary;

namespace YouTubeExtractor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var url = "https://www.youtube.com/watch?v=KA7TTmrtU5g";

            try
            {
                var youTube = YouTube.Default; // starting point for YouTube actions
                var video = youTube.GetVideo(textBox1.Text); // gets a Video object with info about the video

                SaveFileDialog dialog = new SaveFileDialog
                {
                    Filter = $@"Video File (*{video.FileExtension})|*{video.FileExtension}",
                    DefaultExt = video.FileExtension.Replace(".", ""),
                    AddExtension = true
                };

                dialog.ShowDialog();

                if (string.IsNullOrEmpty(dialog.FileName))
                {
                    MessageBox.Show(@"File not selected");
                    return;
                }

                using (var @out = File.OpenWrite(dialog.FileName))
                {
                    var input = video.Stream();
                    CopyStream(input, @out);
                }
                
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            long totalRead = 0;
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalRead += read;
                output.Write(buffer, 0, read);

                Application.DoEvents();
                label1.Text = $@"{(int)(totalRead / 1048576)}Mb Downloaded";
                Application.DoEvents();
            }

            label1.Text = $@"{(int)(totalRead / 1048576)} Downloaded success";
        }
    }
}
