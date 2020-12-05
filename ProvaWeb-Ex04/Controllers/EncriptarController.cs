using ProvaWeb_Ex04.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProvaWeb_Ex04.Controllers
{
    public class EncriptarController : Controller
    {
        private DataContext db = new DataContext();
        private static string AesIV256BD = @"%j?TmFP6$BbMnY$@";//16 caracteres
        private static string AesKey256BD = @"rxmBUJy]&,;3jKwDTzf(cui$<nc2EQr)";


        // GET: Encriptar
        public ActionResult Index()
        {
            return View(db.Encriptars.ToList());
        }

        #region Create - GET
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Models.Encriptar encriptar)
        {
            if (ModelState.IsValid)
            {

                //AesCryptoServiceProvider
                AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
                aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                //Convertendo string para byte array
                byte[] src = Encoding.Unicode.GetBytes(encriptar.Txt);


                //Encriptação
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    //Converte byte array para string de base 64
                    encriptar.Txt = Convert.ToBase64String(dest);
                }
                db.Encriptars.Add(encriptar);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(encriptar);
        }
        #endregion

        #region Details - GET
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Encriptar encriptar = db.Encriptars.Find(id);
            if (encriptar == null)
            {
                return HttpNotFound();
            }

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256BD);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256BD);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Converter a String para um byte array 64 bists
            byte[] src = Convert.FromBase64String(encriptar.Txt);

            // Decriptar
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                encriptar.Txt = Encoding.Unicode.GetString(dest);
            }
            return View(encriptar);
        }
        #endregion

    }
}