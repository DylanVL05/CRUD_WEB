using CRUD_WEB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CRUD_WEB.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()

        {

            using (WEBCRUDEntities context = new WEBCRUDEntities())
            {
                return View(context.Usuarios.ToList());

            }




        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)

        {

            using (WEBCRUDEntities context = new WEBCRUDEntities())
            {
                return View(context.Usuarios.Where(x => x.id == id));

            }


                
        }





        // GET: Usuario/Create
        public ActionResult Create()


        


        {
            return View();
        }







        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Create(Usuarios usuarios)
        {
            try
            {
                using (WEBCRUDEntities context = new WEBCRUDEntities())
                {
                    // Generar salt único para el usuario
                    string salt = GenerateSalt();

                    // Calcular el hash de la contraseña combinando con el salt
                    string hashedPassword = CalculateHash(usuarios.contrasena + salt);

                    // Asignar el hash y el salt al modelo de usuario
                    usuarios.HashContrasena = hashedPassword;
                    usuarios.Salt = salt;

                    // Agregar usuario a la base de datos
                    context.Usuarios.Add(usuarios);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            using (WEBCRUDEntities context = new WEBCRUDEntities())
            {
                return View(context.Usuarios.Where(x => x.id == id).FirstOrDefault());
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Usuarios usuarios)
        {
            try
            {
                using (WEBCRUDEntities context = new WEBCRUDEntities())
                {
                    // Obtener el usuario existente de la base de datos
                    Usuarios existingUser = context.Usuarios.Find(id);

                    if (existingUser != null)
                    {
                        // Generar un nuevo salt
                        string salt = GenerateSalt();

                        // Calcular el nuevo hash de la contraseña combinando con el nuevo salt
                        string hashedPassword = CalculateHash(usuarios.contrasena + salt);

                        // Actualizar las propiedades del usuario con los nuevos valores
                        existingUser.nombre = usuarios.nombre;
                        existingUser.correo = usuarios.correo;
                        existingUser.contrasena = usuarios.contrasena; 
                        existingUser.HashContrasena = hashedPassword;
                        existingUser.Salt = salt;

                        context.Entry(existingUser).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // Función para generar un salt único
        private string GenerateSalt()
        {
            // Lógica para generar un salt único (puedes personalizar esta función según tus necesidades)
            // Ejemplo simple: Devolver un valor de tiempo actual o un valor aleatorio único
            return Guid.NewGuid().ToString();
        }

        // Función para calcular el hash de la contraseña
        private string CalculateHash(string value)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            using (WEBCRUDEntities context = new WEBCRUDEntities())
            {
                return View(context.Usuarios.Where(x => x.id == id).FirstOrDefault());

            }
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                using (WEBCRUDEntities context = new WEBCRUDEntities())
                {

                    Usuarios usuarios = context.Usuarios.Where(x => x.id == id).FirstOrDefault();
                    context.Usuarios.Remove(usuarios);
                    context.SaveChanges();
                }



                    return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
