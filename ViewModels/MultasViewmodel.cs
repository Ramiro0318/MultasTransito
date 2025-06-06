using CommunityToolkit.Mvvm.Input;
using MultasTransito2.Models;
using MultasTransito2.Repositories;
using MultasTransito2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MultasTransito2.ViewModels
{
    public class MultasViewmodel : INotifyPropertyChanged
    {

        public Vistas Vista { get; set; }
        MultasRepository repoMulta { set; get; } = new();
        public ObservableCollection<Multa> ListaMultas { set; get; } = new();
        public Multa Multa { set; get; } = new();
        public string Modo { get; set; } = "Ver";

        private string error;
        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public ICommand IrAgregarMultaCommand { get; set; }
        public ICommand AgregarMultaCommand { get; set; }
        public ICommand IrEditarMultaCommand { set; get; }
        public ICommand EditarMultaCommand { set; get; }
        public ICommand IrEliminarMultaCommand { set; get; }
        public ICommand EliminarMultaCommand { set; get; }
        public ICommand ImprimirMultasCommand { set; get; }
        public ICommand CancelarCommand { set; get; }

        public ICommand OrdenarCommand { set; get; }

        public MultasViewmodel()
        {
            IrAgregarMultaCommand = new RelayCommand(IrAgregarMulta);
            AgregarMultaCommand = new RelayCommand(AgregarMulta);
            IrEditarMultaCommand = new RelayCommand<Multa>(IrEditarMulta);
            EditarMultaCommand = new RelayCommand(EditarMulta);
            IrEliminarMultaCommand = new RelayCommand<Multa>(IrEliminarMulta);
            EliminarMultaCommand = new RelayCommand(EliminarMulta);
            ImprimirMultasCommand = new RelayCommand(ImprimirMultas);
            CancelarCommand = new RelayCommand(Cancelar);

            OrdenarCommand = new RelayCommand<string>(CargarMultas);

            CargarMultas("Asc");

        }

        private void CargarMultas(string? orden)
        {
            if (orden == "Asc")
            {
                ListaMultas = new ObservableCollection<Multa>(repoMulta.GetMultas().OrderBy(x => x.Fecha));
            }
            else
            {
                ListaMultas = new ObservableCollection<Multa>(repoMulta.GetMultas().OrderByDescending(x => x.Fecha));
            }

            Actualizar(); // Asegurar que la UI se refresque
        }


        private void IrAgregarMulta()
        {
            //Vista = Vistas.AgregarMulta;
            Modo = "Agregar";
            Actualizar();
        }


        private void AgregarMulta()
        {
            Error = "";
            if (repoMulta.Validar(Multa, out error))
            {
                repoMulta.InsertarMulta(Multa);
                Cancelar();
            }
            Actualizar();
        }

        private void IrEditarMulta(Multa m)
        {
            Multa = m;

            Multa clon = new()
            {
                Id = Multa.Id,
                Fecha = Multa.Fecha,
                Motivo = Multa.Motivo,
                SancionPecuniaria = Multa.SancionPecuniaria,
                IdCiudadano = Multa.IdCiudadano,
                IdCiudadanoNavigation = Multa.IdCiudadanoNavigation
            };

            Multa = clon;
            Modo = "Editar";
            Actualizar();
        }

        private void EditarMulta()
        {
            Error = "";

            if (Multa != null)
            {
                Multa? m = repoMulta.GetMultaById(Multa.Id);

                if (m != null && repoMulta.Validar(Multa, out error))
                {
                    m.Id = Multa.Id;
                    m.Fecha = Multa.Fecha;
                    m.Motivo = Multa.Motivo;
                    m.SancionPecuniaria = Multa.SancionPecuniaria;
                    m.IdCiudadano = Multa.IdCiudadano;
                    m.IdCiudadanoNavigation = Multa.IdCiudadanoNavigation;

                    repoMulta.EditarMulta(m);
                    Cancelar();
                }
                Actualizar();
            }

        }


        private void IrEliminarMulta(Multa m)
        {
            Multa = m;
            Modo = "Eliminar";
            //Vista = Vistas.Eliminar;
            Actualizar();
        }

        private void EliminarMulta()
        {
            repoMulta.EliminarMulta(Multa);
            Cancelar();
        }

        private void ImprimirMultas()
        {
            ReporteMultas x = new ReporteMultas();
            byte[] pdfBytes = x.GetReporteMultas(repoMulta.GetMultas().ToList());

            File.WriteAllBytes("Secciones.pdf", pdfBytes);
            PropertyChanged?.Invoke(this, new(null));
        }

        private void Cancelar()
        {
            Error = "";
            Modo = "Ver";
            //Vista = Vistas.Multas;
            CargarMultas("Asc");
            Actualizar();
        }

        private void Actualizar()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;


    }
}
