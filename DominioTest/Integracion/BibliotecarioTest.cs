using System;
using System.Collections.Generic;
using System.Text;
using BibliotecaDominio;
using BibliotecaRepositorio.Contexto;
using BibliotecaRepositorio.Repositorio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DominioTest.TestDataBuilders;
using Microsoft.EntityFrameworkCore;

namespace DominioTest.Integracion
{

    [TestClass]
    public class BibliotecarioTest
    {
        public const String CRONICA_UNA_MUERTE_ANUNCIADA = "Cronica de una muerte anunciada";
        public const String ISBN_PALINDROMO = "12345554321";
        public const String ISBN_NUMERICOS_MENOR_QUE_30 = "T8I8B8";
        public const String ISBN_NUMERICOS_MAYOR_QUE_30 = "A874B69Q";
        private  BibliotecaContexto contexto;
        private  RepositorioLibroEF repositorioLibro;
        private RepositorioPrestamoEF repositorioPrestamo;


        [TestInitialize]
        public void setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BibliotecaContexto>();
            contexto = new BibliotecaContexto(optionsBuilder.Options);
            repositorioLibro  = new RepositorioLibroEF(contexto);
            repositorioPrestamo = new RepositorioPrestamoEF(contexto, repositorioLibro);
        }

        [TestMethod]
        public void PrestarLibroTest()
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConTitulo(CRONICA_UNA_MUERTE_ANUNCIADA).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act
            bibliotecario.Prestar(libro.Isbn, "Juan");

            // Assert
            Assert.AreEqual(bibliotecario.EsPrestado(libro.Isbn), true);
            Assert.IsNotNull(repositorioPrestamo.ObtenerLibroPrestadoPorIsbn(libro.Isbn));

        }

        [TestMethod]
        public void EsIsbnPalindromoTest()
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConIsbn(ISBN_PALINDROMO).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            try
            {
                // Act
                bibliotecario.Prestar(libro.Isbn, "Juan");
                Assert.Fail();
            }
            catch (Exception err)
            {
                // Assert
                Assert.AreEqual("Los libros palindromos solo se pueden utilizar en la biblioteca", err.Message);
            }

        }

        [TestMethod]
        public void FechaEntregaPrestamoIsbnMayorQue30Test()
        {
            // Arrange
            
            Libro libro = new LibroTestDataBuilder().ConIsbn(ISBN_NUMERICOS_MAYOR_QUE_30).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            DateTime fechaPrestamo = new DateTime(2017, 5, 24);

            DateTime? fechaEntrega = bibliotecario.ObtenerFechaEntregaPrestamo(libro.Isbn, fechaPrestamo);

            // Assert
            Assert.AreEqual(new DateTime(2017, 06, 09), fechaEntrega);
        }

        [TestMethod]
        public void FechaEntregaPrestamoIsbnMenorQue30Test()
        {
            // Arrange

            Libro libro = new LibroTestDataBuilder().ConIsbn(ISBN_NUMERICOS_MENOR_QUE_30).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);
          
            DateTime fechaPrestamo = new DateTime(2017, 5, 24);

            DateTime? fechaEntrega = bibliotecario.ObtenerFechaEntregaPrestamo(libro.Isbn,fechaPrestamo);
    
            // Assert
            Assert.AreEqual(null,fechaEntrega);
        }


        [TestMethod]
        public void PrestarLibroNoDisponibleTest()
        {
            // Arrange
            Libro libro = new LibroTestDataBuilder().ConTitulo(CRONICA_UNA_MUERTE_ANUNCIADA).Build();
            repositorioLibro.Agregar(libro);
            Bibliotecario bibliotecario = new Bibliotecario(repositorioLibro, repositorioPrestamo);

            // Act
            bibliotecario.Prestar(libro.Isbn,"Juan");
            try
            {
                bibliotecario.Prestar(libro.Isbn, "Juan");
                Assert.Fail();
            }
            catch (Exception err)
            {
                // Assert
                Assert.AreEqual("El libro no se encuentra disponible", err.Message);
            }
        
        }

    }
}
