using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Caching.Memory;
using MultasTransito2.Models;
using MultasTransito2.Repositories;
using MultasTransito2.Services;
using MultasTransito2.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MultasTransito2.ViewModels
{

    public class CiudadanosViewmodel : INotifyPropertyChanged
    {

        public Vistas Vista { get; set; } = Vistas.Ciudadanos;


        public CiudadanosRepository repoCiudadano { set; get; } = new();

        public List<Multasporciudadanoview> ListaMultasPorCiudadano { get; set; } = new();
        public ObservableCollection<Ciudadano> ListaCiudadanos { get; set; } = new();
        public Ciudadano Ciudadano { get; set; } = new();

        public string Modo { get; set; } = "Ver";


        public event PropertyChangedEventHandler? PropertyChanged;

        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; }
        }
        private string filtro;
        public string Filtro {
            get { return  filtro; } 
            set { filtro = value; }
        }


        public ICommand AgregarCommand { set; get; }
        public ICommand IrAgregarCommand { set; get; }
        public ICommand IrEditarCommand { set; get; }
        public ICommand EditarCommand { set; get; }
        public ICommand IrEliminarCommand { set; get; }
        public ICommand EliminarCommand { set; get; }
        public ICommand ImprimirCiudadanosCommand { set; get; }
        public ICommand CancelarCommand { set; get; }

        public ICommand FiltrarCommand { set; get; }


        public CiudadanosViewmodel()
        {

            IrAgregarCommand = new RelayCommand(IrAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            IrEditarCommand = new RelayCommand<Ciudadano>(IrEditar);
            EditarCommand = new RelayCommand(Editar);
            IrEliminarCommand = new RelayCommand<Ciudadano>(IrEliminar);
            EliminarCommand = new RelayCommand(Eliminar);
            ImprimirCiudadanosCommand = new RelayCommand(ImprimirCiudadanos);
            CancelarCommand = new RelayCommand(Cancelar);

            FiltrarCommand = new RelayCommand(Filtrar);

            CargarCiudadanos();
        }

        

        private void CargarCiudadanos()
        {
            ListaCiudadanos.Clear();
            var c = repoCiudadano.GetAll();
            foreach (var item in c)
            {
                ListaCiudadanos.Add(item);
            }
        }
        private void Filtrar()
        {
            
            ListaCiudadanos.Clear();
            var c = repoCiudadano.Filtrar(Filtro);
            foreach (var item in c)
            {
                ListaCiudadanos.Add(item);
            }
            Actualizar();
        }


        private void IrAgregar()
        {
            Modo = "Agregar";
            Ciudadano = new();
            Actualizar();
        }

        private void Agregar()
        {
            Error = "";
            if (repoCiudadano.Validar(Ciudadano, out error))
            {
                repoCiudadano.InsertarCiudadano(Ciudadano);
                Cancelar();
            }
            Actualizar();
        }

        private void IrEditar(Ciudadano c)
        {
            if (Ciudadano != null)
            {

                Ciudadano = c;
                Ciudadano clon = new()
                {
                    NumeroLicenca = Ciudadano.NumeroLicenca,
                    Nombre = Ciudadano.Nombre,
                    Edad = Ciudadano.Edad,
                    MultasAcargo = Ciudadano.MultasAcargo,
                    Multa = Ciudadano.Multa

                };

                Ciudadano = clon;
                Modo = "Editar";
                Actualizar();
            }
        }


        private void Editar()
        {
            Error = "";

            if (Ciudadano != null)
            {
                Ciudadano? c = repoCiudadano.GetCiudadanoByNumero(Ciudadano.NumeroLicenca);

                if (c != null && repoCiudadano.ValidarEditar(Ciudadano, out error))
                {
                    c.NumeroLicenca = Ciudadano.NumeroLicenca;
                    c.Nombre = Ciudadano.Nombre;
                    c.Edad = Ciudadano.Edad;
                    c.MultasAcargo = Ciudadano.MultasAcargo;
                    c.Multa = Ciudadano.Multa;

                    repoCiudadano.EditarCiudadano(c);
                    Cancelar();
                }
                Actualizar();
            }
        }

        private void IrEliminar(Ciudadano c)
        {
            Ciudadano = c;
            Modo = "Eliminar";
            //Vista = Vistas.Eliminar;
            Actualizar();
        }

        private void Eliminar()
        {
            Error = "";
            if (repoCiudadano.ValidarEliminar(Ciudadano, out error))
            {
                repoCiudadano.EliminarCiudadano(Ciudadano);
                Cancelar();

            }
            Actualizar();
        }

        private void ImprimirCiudadanos()
        {
            ReporteCiudadanos x = new ReporteCiudadanos();
            byte[] pdfBytes = x.GetReporteSecciones(repoCiudadano.GetMultasPorCiudadanoView().ToList());

            File.WriteAllBytes("Secciones.pdf", pdfBytes);
            PropertyChanged?.Invoke(this, new(null));
            Cancelar();
        }

        private void Cancelar()
        {
            //((MainViewmodel)Application.Current.MainWindow.DataContext).CambiarVista(new CiudadanoView());
            Modo = "Ver";
            Error = "";
            Filtro = "";
            CargarCiudadanos();
            Actualizar();
        }


        private void Actualizar()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }


        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private string multa;

        public string Multa { get => multa; set => SetProperty(ref multa, value); }



    }
}
