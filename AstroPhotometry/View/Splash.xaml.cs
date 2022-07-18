using AstroPhotometry.ViewModels;
using System.Windows;


namespace AstroPhotometry.View
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash(CmdStringVM output)
        {
            DataContext = output;
            InitializeComponent();
        }

    }
}
