using MyEvernote.BussinessLayer;
using MyEvernote.Entites;
using MyEvernote.Entites.Messages;
using MyEvernote.Entites.ValueObjects;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private UserManager userManager = new UserManager();

        // GET: Home
        public ActionResult Index()
        {
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category cat = categoryManager.Find(x => x.Id == id.Value);

            if (cat == null)
            {
                return HttpNotFound();
            }

            return View("Index", cat.Notes.Where(x=>x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(CurrentSession.User.Id); 
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştı!",
                    Items = res.Errors
                };
                return View("Error", errNotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            BusinessLayerResult<EvernoteUser> res = userManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştı!",
                    Items = res.Errors
                };
                return View("Error", errNotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EvernoteUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (
                    ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFileName = filename;
                }
                BusinessLayerResult<EvernoteUser> res = userManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("ShowProfile");
            }
            else
            {
                return View(model);
            }
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            BusinessLayerResult<EvernoteUser> res = userManager.RemoveUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil silinemedi!!!",
                    RedirectUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = userManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                CurrentSession.Set<EvernoteUser>("login", res.Result);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUser> res = userManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkViewModel notfyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectUrl = "/Home/Login"
                };
                notfyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak " +
                    "hesabınızı aktive ediniz.Hesabınızı aktive etmeden paylaşım yapamazsınız!!!");
                return View("Ok", notfyObj);
            }
            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EvernoteUser> res = userManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                TempData["errors"] = res.Errors;
                return RedirectToAction("UserActivateCancel");
            }

            return RedirectToAction("UserActivateOk");
        }
        public ActionResult UserActivateOk()
        {
            return View();
        }
        public ActionResult UserActivateCancel()
        {
            List<ErrorMessageObj> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObj>;
            }

            return View(errors);
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }


    }
}