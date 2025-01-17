using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using System.Diagnostics;

namespace Image_Annotation_Tool
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
            //LinkLabel.Link link = new LinkLabel.Link();
            //link.LinkData = "https://github.com/serhataydinxd";
            //linkLabel1.Links.Add(link);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var url = "https://github.com/serhataydinxd";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Varsayılan tarayıcıda açmayı sağlar.
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var url = "https://github.com/Mu5enna";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Varsayılan tarayıcıda açmayı sağlar.
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var url = "https://github.com/Mu5enna/Easier_Image_Annotation_Tool";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Varsayılan tarayıcıda açmayı sağlar.
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var url = "";
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Varsayılan tarayıcıda açmayı sağlar.
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }
    }
}
