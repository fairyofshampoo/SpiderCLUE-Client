using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;    

namespace Spider_Clue.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : Page
    {
        public MainMenuView()
        {
            InitializeComponent();
            List<Persona> personas = new List<Persona>
            {
                new Persona { Nombre = "Juan", Edad = 30 },
                new Persona { Nombre = "María", Edad = 25 },
                new Persona { Nombre = "Luis", Edad = 35 }
            };
            FriendsTable.ItemsSource = personas;
        }
    }

    public class Persona
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
    }
}
