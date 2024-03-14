using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebsiteBanHang.Models; // Thay thế bằng namespace thực tế của bạn
using WebsiteBanHang.Repositories; // Thay thế bằng namespace thực tế của bạn

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        //[HttpPost]
        //public IActionResult Add(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _productRepository.Add(product);
        //        return RedirectToAction("Index"); // Chuyển hướng tới trang danhsách sản phẩm
        //    }
        //    return View(product);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (imageUrl != null)
        //        {
        //            Lưu hình ảnh đại diện
        //            product.ImageUrl = await SaveImage(imageUrl);
        //        }
        //        if (imageUrls != null)
        //        {
        //            product.ImageUrls = new List<string>();
        //            foreach (var file in imageUrls)
        //            {
        //                Lưu các hình ảnh khác
        //                product.ImageUrls.Add(await SaveImage(file));
        //            }
        //        }

        //        _productRepository.Add(product);
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null && IsImageValid(imageUrl))
                {
                    // Lưu hình ảnh đại diện
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                else if (imageUrl != null)
                {
                    ModelState.AddModelError("ImageUrl", "Vui lòng tải lên một tệp hình ảnh hợp lệ.");
                }

                if (imageUrls != null)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        if (IsImageValid(file))
                        {
                            // Lưu các hình ảnh khác
                            product.ImageUrls.Add(await SaveImage(file));
                        }
                        else
                        {
                            ModelState.AddModelError("ImageUrls", "Một hoặc nhiều tệp ảnh không hợp lệ.");
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    _productRepository.Add(product);
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        private bool IsImageValid(IFormFile file)
        {
            // Kiểm tra tệp có phải là hình ảnh hợp lệ không
            if (file.ContentType.ToLower() != "image/jpg" &&
                file.ContentType.ToLower() != "image/jpeg" &&
                file.ContentType.ToLower() != "image/pjpeg" &&
                file.ContentType.ToLower() != "image/gif" &&
                file.ContentType.ToLower() != "image/x-png" &&
                file.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            // Kiểm tra kích thước tệp không vượt quá giới hạn
            if (file.Length > 5 * 1024 * 1024) // 5MB
            {
                return false;
            }

            return true;
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // Các actions khác như Display, Update, Delete

        // Display a list of products
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }


        // Display a single product
        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Show the product update form
        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Process the product update
        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // Show the product delete confirmation
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Process the product deletion
        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
