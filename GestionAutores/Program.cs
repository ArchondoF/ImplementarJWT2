using BusinessLogic.DataModel;
using DataAccess.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAutores
{
    class Program
    {
        static void Main(string[] args)
        {
        IngresoOpcionMenu:
            DibujarMenuPrincipal();
            int opcionMenuPrinciapal = ProcesarOpcionMenuPrincipal();

            switch (opcionMenuPrinciapal)
            {
                case 0:
                    CrearNuevoAutor();
                    goto IngresoOpcionMenu;
                case 1:
                    CrearNuevoLibro();
                    goto IngresoOpcionMenu;
                case 2:
                    ModificarCantidadVentasLibro();
                    goto IngresoOpcionMenu;
                case 3:
                    ConsultarLibros();
                    goto IngresoOpcionMenu;
                case 4:
                    ConsultarAutores();
                    goto IngresoOpcionMenu;
                case 5:
                    EliminarLibro();
                    goto IngresoOpcionMenu;
                default:
                    goto IngresoOpcionMenu;
            }

        }

        private static void DibujarMenuPrincipal()
        {
            Console.WriteLine("0.Crear nuevo Autor");
            Console.WriteLine("1.Crear Nuevo Libro");
            Console.WriteLine("2.Modificar cantidad ventas de un Libro");
            Console.WriteLine("3.Consultar Libros");
            Console.WriteLine("4.Consultar autores");
            Console.WriteLine("5.Eliminar Libro");

            Console.WriteLine("");
        }
        private static int ProcesarOpcionMenuPrincipal()
        {
            int opcion = -1;
            string inputUsuario = string.Empty;

            do
            {
                Console.WriteLine("Ingrese la opcion deseada");

                inputUsuario = Console.ReadLine();

                if (int.TryParse(inputUsuario, out opcion) && opcion >= 0 && opcion <= 5)
                {
                    break;
                }
                else
                {
                    Console.Clear();
                    DibujarMenuPrincipal();
                }

            } while (true);

            return opcion;
        }
        private static void CrearNuevoAutor()
        {
            Console.Clear();
            Console.WriteLine("Ingrese el nombre del autor a crear");

            string nombreAutor = Console.ReadLine();

            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    Autores autor = new Autores()
                    {
                        Nombre = nombreAutor
                    };

                    uow.AutorRepository.AddAutor(autor);

                    uow.SaveChanges();

                    uow.Commit();

                    Console.Clear();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw;
                }

            }

        }
        private static void CrearNuevoLibro()
        {
        CrearNuevoLibro:
            Console.Clear();
            Console.WriteLine("Ingrese el nombre del autor al que desea agregar un libro");

            string nombreAutor = Console.ReadLine();

            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var autor = uow.AutorRepository.GetAutorByName(nombreAutor);

                    if (autor == null)
                        goto CrearNuevoLibro;

                    string tituloLibro = string.Empty;
                    int cantidadVenta, anioPublicacion;

                    Console.WriteLine("Ingrese el titulo del libro");
                    tituloLibro = Console.ReadLine();
                    Console.WriteLine("Ingrese la cantidad de ventas del libro");
                    cantidadVenta = int.Parse(Console.ReadLine());
                    Console.WriteLine("Ingrese el anio de publicacion libro");
                    anioPublicacion = int.Parse(Console.ReadLine());


                    Libros libro = new Libros()
                    {
                        AnioPublicacion = anioPublicacion,
                        CantidadDeVentas = cantidadVenta,
                        Titulo = tituloLibro,
                        IdAutor = autor.Id
                    };

                    uow.LibroRepository.AddLibro(libro);

                    uow.SaveChanges();

                    uow.Commit();

                    Console.Clear();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw;
                }

            }

        }
        private static void ModificarCantidadVentasLibro()
        {
        ModificarCantidadVentas:
            Console.Clear();
            Console.WriteLine("Ingrese el nombre del Libro al cual desea modificar la cantidad de ventas");

            string nombreLibro = Console.ReadLine();

            using (var uow = new UnitOfWork())
            {
                try
                {
                    var libro = uow.LibroRepository.GetLibroByTitle(nombreLibro);

                    if (libro == null)
                        goto ModificarCantidadVentas;

                    int cantidadVentas;
                    Console.WriteLine("Ingrese la nueva cantidad de ventas");
                    cantidadVentas = int.Parse(Console.ReadLine());

                    libro.CantidadDeVentas = cantidadVentas;

                    uow.SaveChanges();

                    uow.Commit();

                    Console.Clear();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw;
                }
            }

        }
        private static void ConsultarLibros()
        {
            Console.Clear();

            using (var uow = new UnitOfWork())
            {
                var libros = uow.LibroRepository.GetLibrosWithAutorOrderBySales();

                foreach (var libro in libros)
                {
                    Console.WriteLine($"{libro.Titulo} - {libro.Autores.Nombre}");
                }

                Console.ReadLine();
                Console.Clear();
            }
        }
        private static void ConsultarAutores()
        {
            Console.Clear();

            using (var uow = new UnitOfWork())
            {
                var autores = uow.AutorRepository.GetAutoresOrderByName();

                foreach (var autor in autores)
                {
                    Console.WriteLine(autor.Nombre);
                }

                Console.ReadLine();
                Console.Clear();
            }
        }
        private static void EliminarLibro()
        {
        EliminarLibro:
            Console.Clear();
            Console.WriteLine("Ingrese el identificador del Libro que desea eliminar");

            long idLibro = long.Parse(Console.ReadLine());

            using (var uow = new UnitOfWork())
            {
                uow.BeginTransaction();

                try
                {
                    var libro = uow.LibroRepository.GetLibroById(idLibro);

                    if (libro == null)
                        goto EliminarLibro;

                    uow.LibroRepository.RemoveLibro(libro);

                    uow.SaveChanges();

                    uow.Commit();

                    Console.Clear();
                }
                catch (Exception ex)
                {
                    uow.Rollback();
                    throw;
                }
            }

        }

    }
}
