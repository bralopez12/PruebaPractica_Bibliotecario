using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Prestamo
    {
        public DateTime FechaSolicitud { get;}
        public Libro Libro { get;}
        public DateTime? FechaEntregaMaxima { get; }
        public string NombreUsuario { get;}

       
        public Prestamo(DateTime fechaSolicitud, Libro libro, DateTime? fechaEntregaMaxima, string nombreUsuario)
        {
            this.FechaSolicitud = fechaSolicitud;
            this.Libro = libro;
            this.FechaEntregaMaxima = fechaEntregaMaxima;
            this.NombreUsuario = nombreUsuario;
        }
    }
}
