using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using firstProjectWith_ASP.Data;
using firstProjectWith_ASP.Models;
using firstProjectWith_ASP.Models.ViewModels;

namespace firstProjectWith_ASP.Controllers
{ 
   
    public class ProductController : Controller
    {
        private readonly applicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(applicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
           _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType);

            //foreach(var obj in objList)//commenting the foreach as the include is doing the eagerlodaing of the values
            //{
            //    obj.Category = _db.category.FirstOrDefault(u => u.Id == obj.CategoryId);
           //     obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);//STEP 3:ADD TO THE PRODUCT 
          //  };

            return View(objList);
        }


        //GET - UPSERT
        public IActionResult Upsert(int? id) //common method for edit snd create so a id is managed to differentiate between them
        {
            
          //  IEnumerable<SelectListItem> CategoryDropDown = _db.category.Select(i => new SelectListItem
          //  {
          //     Text = i.Name,
          //     Value = i.Id.ToString()
          //  });
                                                                   //SENDING DATA FROM CONTROLLER TO VIEW METHODS
           // ViewBag.CategoryDropDown = CategoryDropDown;// Method 1 : Viewbag(1.is a dynamic property; 
                                                                //2.Any number of properties and values can be assigned to ViewBag;
                                                            // life only last during the current HTTP request
                                                            //4.Value will be null if redirection occurs) 
                                                            //5.rapper around view data
                                                               //transfers data from the controller to view, not vice-versa Ideal for solutions in which the temporaray data is not in a amodel
            ///ViewData["CategoryDropDown"] = CategoryDropDown;//Method 2://ViewData is a dictionary typr with ey and value ;
                                                            // transfers the data from conroller to view not vice-versa;
                                                            //when- ideal for situations in which the temporary data is not in a model
                                                            //is derived from ViewDataDictionary which is a dictionary  type
                                                            //ViewData value must be type cast before use
                                                            //life last only during the current HTTP request
                                                            //value null when redirection occurs
                                                            //Method 3: TempData-
                                                            

            //Product product = new Product();
             
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _db.category.Select(i => new SelectListItem
                {
                    Text = i.Name, 
                    Value = i.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            { //id not null then populated or the edit functionality
                productVM.Product = _db.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);//here the view knows what tyoe of model the controller uses
            }
        }


        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;//retrieve the host
                string webRootPath = _webHostEnvironment.WebRootPath;//path to a www root folder and access using the webhost environrent

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;//shows the browse for the image
                    string fileName = Guid.NewGuid().ToString();//name of the file
                    string extension = Path.GetExtension(files[0].FileName);//extension of the file uploaded

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create)) //upload the file to a new location where neede
                    {
                        files[0].CopyTo(fileStream);//file copied
                    }

                    productVM.Product.Image = fileName + extension;//update the image of the product with the new path

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);//retrieve from the Db
                    // AsNoTracking method ask not to track this entity as the entity cannot track same value'product ' two times(line 133, 159)
                    if (files.Count > 0)//New file has been updated with the existing name
                    {
                        string upload = webRootPath + WC.ImagePath;//new file
                        string fileName = Guid.NewGuid().ToString();//name
                        string extension = Path.GetExtension(files[0].FileName);//extension

                        var oldFile = Path.Combine(upload, objFromDb.Image);//replace the old image with the new

                        if (System.IO.File.Exists(oldFile))// check if the file exits old one
                        {
                            System.IO.File.Delete(oldFile);//if exists delete
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;//save the new image in the product folder
                    }
                    else//image not updated ut something else updated
                    {
                        productVM.Product.Image = objFromDb.Image;//keep the smae image since not modified
                    }
                    _db.Product.Update(productVM.Product);//update the product veiw model if something updated
                }


                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _db.category.Select(i => new SelectListItem//to show the drop down for the category list
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(i => new SelectListItem //to show thw dropdown for the application type
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);

        }



        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType).FirstOrDefault(u => u.Id == id);
           // include() to display category and application type using eager loading
         //   product.Category = _db.category.Find(product.CategoryId);//load the product category and we can display that 
         // due to the entity framework and eager losding approach using theinclude and lambda expresssion category get loadd
         //where clause compared to a group of records error removed
         //firstor defult --compares only onr record
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]//ActionName -used to define the custom action name which lets the.net core know this is the code for delete as well as here it is defined by the name of the deletepost
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;//update the image file
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))//if the old file exists
            {
                System.IO.File.Delete(oldFile);//delete
            }


            _db.Product.Remove(obj);//remove the product
            _db.SaveChanges();
            return RedirectToAction("Index");


        }

    }
}