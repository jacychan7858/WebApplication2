using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {
        private NorthwindEntities2 db = new NorthwindEntities2();


        // GET: Category
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }



        //讀取單筆資料
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            return View(db.Categories.Find(id));
        }



        [HttpPost]
        //修改資料
        public ActionResult Edit(Category category, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                if (ProductImage != null)
                {
                    using (var tempdb = new NorthwindEntities2())
                    {

                        var c = tempdb.Categories.Find(category.CategoryID);
                        c.CategoryName = category.CategoryName;
                        c.Description = category.Description;

                        //檔案上傳
                        string strPath = Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName;
                        ProductImage.SaveAs(strPath);

                        //要將資料寫進資料庫
                        c.ProductImage = ProductImage.FileName;
                        //將圖轉成二進位
                        var imgSize = ProductImage.ContentLength;
                        byte[] imgByte = new byte[imgSize];
                        ProductImage.InputStream.Read(imgByte, 0, imgSize);
                        c.Picture = imgByte;

                        tempdb.SaveChanges();

                    }
                }
            }
            return RedirectToAction("Index");
        }



        //新增
        //public ActionResult Insert()
        //{
        //    //判斷使否有表單的資料傳過來
        //    if (Request.Form.Count > 0)
        //    {
        //        //接收表單傳過來的資料
        //        Categories _category = new Categories();
        //        _category.CategoryName = Request.Form["CategoryName"];
        //        _category.Description = Request.Form["Description"];

        //        //將資料傳給Model
        //        db.Categories.Add(_category);
        //        db.SaveChanges();

        //        //轉到Index Action，檢視新增完成後的結果
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}


        //刪除
        public ActionResult Delete(int id = 0) //用id參數接收browser傳過來的資料
        {
            db.Categories.Remove(db.Categories.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Index圖片顯示
        public ActionResult GetImageByte(int id = 1)
        {
            Category _category = db.Categories.Find(id);
            byte[] img = _category.Picture;
            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            else
                return null;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.datas = db.Categories;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category, HttpPostedFileBase ProductImage)
        {
            if (ModelState.IsValid)
            {
                //檔案上傳注意事項
                //1. <form enctype="multipart/form-data"><inpu type="file" name="ProductImage"
                //2. action(HttpPostFileBase ProductImage)
                //3. 知道資料夾的實際路徑 D:\Share\StoreApp\StoreApp\ProductImages\
                //ViewBag.realPath = Request.PhysicalApplicationPath;  //回傳專案目錄的實際路徑
                //ViewBag.realPath = Server.MapPath(".");   //回傳程式執行所在的實際路徑
                //ViewBag.realPath = Request.PhysicalApplicationPath + @"ProductImages\";
                //4. 足夠的權限  給everyone寫入的權限
                //執行上傳程式
                //ProductImage.SaveAs(Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName);

                if (ProductImage != null)
                {
                    //檔案上傳
                    string strPath = Request.PhysicalApplicationPath + @"ProductImages\" + ProductImage.FileName;
                    ProductImage.SaveAs(strPath);

                    //要將資料寫進資料庫
                    category.ProductImage = ProductImage.FileName;
                    //將圖轉成二進位
                    var imgSize = ProductImage.ContentLength;
                    byte[] imgByte = new byte[imgSize];
                    ProductImage.InputStream.Read(imgByte, 0, imgSize);
                    category.Picture = imgByte;

                    db.Categories.Add(category);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "請選擇檔案!!";

                }


            }


            ViewBag.datas = db.Categories;
            return View();

        }


    }


}
