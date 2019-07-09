using BibliotecaDominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaDominio
{
    public class Bibliotecario
    {
        public const string EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE = "El libro no se encuentra disponible";
        private  IRepositorioLibro libroRepositorio;
        private  IRepositorioPrestamo prestamoRepositorio;

        public Bibliotecario(IRepositorioLibro libroRepositorio, IRepositorioPrestamo prestamoRepositorio)
        {
            this.libroRepositorio = libroRepositorio;
            this.prestamoRepositorio = prestamoRepositorio;
        }

        public void Prestar(string isbn, string nombreUsuario)
        {
            if (EsPalindromo(isbn))
            {
                throw new Exception("Los libros palindromos solo se pueden utilizar en la biblioteca");
            }

            if (EsPrestado(isbn))
            {
                throw new Exception("El libro no se encuentra disponible");
            }

            DateTime? fechaEntrega = ObtenerFechaEntregaPrestamo(isbn,DateTime.Now);

            prestamoRepositorio.Agregar(new Prestamo(DateTime.Now, libroRepositorio.ObtenerPorIsbn(isbn), fechaEntrega, nombreUsuario));
            
        }

        public DateTime? ObtenerFechaEntregaPrestamo(string isbn,DateTime fechaPrestamo)
        {
            int sumaIsbn = 0;
            int valorCaracter;
            for (int i = 0; i < isbn.Length; i++)
            {
                valorCaracter = 0;
                int.TryParse(isbn.Substring(i, 1), out valorCaracter);
                sumaIsbn += valorCaracter;
            }

            if (sumaIsbn > 30)
            {
                DateTime fechaEntrega = SumarDiasSinContarDomingos(fechaPrestamo,15);
                return fechaEntrega;
            }

            return null;

        }

        private DateTime SumarDiasSinContarDomingos(DateTime fechaASumar, int diasASumar)
        {
            int diasAOperar = diasASumar - 1;
            for (int i = 0; i < diasAOperar; i++)
            {
                if (EsDomingo(fechaASumar))
                {
                    fechaASumar = fechaASumar.AddDays(1);
                }
                fechaASumar = fechaASumar.AddDays(1);
            }
            return fechaASumar;
        }

        private bool EsDomingo(DateTime fecha)
        {
            return fecha.DayOfWeek.Equals(DayOfWeek.Sunday);
        }

        public bool EsPrestado(string isbn)
        {
            try
            {
                Prestamo prestamo = prestamoRepositorio.Obtener(isbn);
                return prestamo != null;
            }catch(Exception e)
            {
                return false;
            }
        }

        private bool EsPalindromo(string isbn)
        {
            string isbnInverso = "";
            for (int i = (isbn.Length-1); i >= 0; i--)
            {
                isbnInverso += isbn.Substring(i, 1);
            }

            if (isbn.Equals(isbnInverso))
            {
                return true;
            }

            return false;
        }
    }
}
