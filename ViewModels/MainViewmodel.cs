using CommunityToolkit.Mvvm.Input;
using MultasTransito2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MultasTransito2.ViewModels
{
    public enum Vistas { Ciudadanos, Multas, AgregarCiudadano, EditarCiudadano, AgregarMulta, EditarMulta, Eliminar }

    public class MainViewmodel : INotifyPropertyChanged
    {
        public Vistas Vista { get; set; }

        private string _vistaSeleccionada;
        private UserControl vistaActual;

        public MultasView Multasv = new();
        public UserControl VistaActual { get => vistaActual; set { vistaActual = value; VistaSeleccionada = value?.GetType().Name; } }

        public ICommand IrCiudadanosCommand { get; set; }
        public ICommand IrMultasCommand { get; set; }
        public MainViewmodel()
        {
            IrCiudadanosCommand = new RelayCommand(IrCiudadanos);
            IrMultasCommand = new RelayCommand(IrMultas);

            IrCiudadanos();
        }

        public string VistaSeleccionada
        {
            get => _vistaSeleccionada;
            set
            {
                _vistaSeleccionada = value;
                PropertyChanged?.Invoke(this, new(null));
            }
        }

        private void IrMultas()
        {
            Vista = Vistas.Multas;
            vistaActual = new MultasView();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VistaActual)));

        }
        private void IrCiudadanos()
        {
            Vista= Vistas.Ciudadanos;
            VistaActual = new CiudadanoView();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VistaActual)));
        }

      
        public void Actualizar()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
