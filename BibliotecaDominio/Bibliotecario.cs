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
            //throw new Exception("se debe implementar este método");
            int cantDiasPrestamo = 0;
            if (palindromo(isbn) == true)
            {
                throw new Exception("Los libros palíndromos solo se pueden utilizar en la biblioteca");
            }
            else
            {
                if (EsPrestado(isbn) == true)
                {
                    throw new Exception(EL_LIBRO_NO_SE_ENCUENTRA_DISPONIBLE);
                }
                else
                {
                    DateTime fechaSolicitud = DateTime.Now;
                    DateTime fechaEntregaMaxima = new DateTime();
                    if (SumaNumerosISBN(isbn) >= 30)
                    {
                        cantDiasPrestamo = 15;
                        fechaEntregaMaxima = CalcularFechaEntregaMaxima(fechaSolicitud, cantDiasPrestamo);
                    }
                    else
                    {
                        cantDiasPrestamo = 16;
                        fechaEntregaMaxima = CalcularFechaEntregaMaxima(fechaSolicitud, cantDiasPrestamo);
                    }             
                    libroRepositorio.ObtenerPorIsbn(isbn);
                    Prestamo oPrestamo = new Prestamo(fechaSolicitud, libroRepositorio.ObtenerPorIsbn(isbn), fechaEntregaMaxima, nombreUsuario);
                    prestamoRepositorio.Agregar(oPrestamo);
                }
            }
        }


        public bool EsPrestado(string isbn)
        {
            try
            {
                prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn);
                string isbnPrestado = prestamoRepositorio.ObtenerLibroPrestadoPorIsbn(isbn).Isbn;
                if (isbnPrestado == isbn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool palindromo(string texto)
        {
            String textoInv = "", caracter;
            int textoTamano = texto.Length;
            for (int i = textoTamano - 1; i >= 0; i--)
            {
                caracter = texto.Substring(i, 1);
                textoInv = textoInv + caracter;
            }
            if (texto == textoInv)
                return true;
            else
                return false;
        }


        public int SumaNumerosISBN(string texto)
        {
            char caracter;
            int textoTamano = texto.Length, acumNumeros=0, caracterNumerico=0;
            for (int i = textoTamano - 1; i >= 0; i--)
            {
                caracter = Convert.ToChar(texto.Substring(i, 1));
                if (char.IsNumber(caracter))
                {
                    caracterNumerico = Convert.ToInt32(caracter.ToString());
                    acumNumeros = acumNumeros + caracterNumerico;
                }
            }
            return acumNumeros;
        }

        public DateTime CalcularFechaEntregaMaxima(DateTime fechaSolicitud, int cantDias)
        {
            DateTime fechaEntregaMaxima = new DateTime();
            for (int i = 0; i < cantDias-1; i++)
            {
                if (fechaSolicitud.DayOfWeek.ToString() == "Saturday")
                {
                    fechaEntregaMaxima = fechaSolicitud.AddDays(2);
                }
                else
                {
                    fechaEntregaMaxima = fechaSolicitud.AddDays(1);
                }
                fechaSolicitud = fechaEntregaMaxima;
            }
            return fechaEntregaMaxima;
        }
    }
}
