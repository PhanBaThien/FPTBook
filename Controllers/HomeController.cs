
using FPTBook_v3.Data;
using FPTBook_v3.Models;
using FPTBook_v3.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FPTBook_v3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        [Route("Home/ShowBook")]
        public async Task<IActionResult> ShowBook(string sterm = "", int genreId = 0)
        {

            IEnumerable<Book> books = await GetBooks(sterm, genreId);
            IEnumerable<Category> categorys = await _db.Categorys.ToListAsync(); ;
            Models.BookDisplayModel bookModel = new Models.BookDisplayModel
            {
                Books = books,
                Categorys = categorys,
                STerm = sterm,
                GenreId = genreId
            };
            return View(bookModel);
        }


        [Route("/Book/Detail")]
        public async Task<IActionResult> BookDetail(int id, string cate)
        {
            if (id == null || _db.Books == null)
            {
                return NotFound();
            }
            else
            {
                var book = _db.Books.Include(x => x.category).FirstOrDefault(b => b.book_Id == id);
                if (book == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(book);
                }
            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IEnumerable<Category>> Category()
        {
            return await _db.Categorys.ToListAsync();
        }


        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            IEnumerable<Book> books = await (from book in _db.Books
                                             join genre in _db.Categorys
                                             on book.cate_Id equals genre.cate_Id
                                             where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.book_Title.ToLower().StartsWith(sTerm))
                                             select new Book
                                             {
                                                 book_Id = book.book_Id,
                                                 book_ImagURL = book.book_ImagURL,
                                                 category = book.category,
                                                 book_Title = book.book_Title,
                                                 cate_Id = book.cate_Id,
                                                 book_Price = book.book_Price,
                                                 book_Description = book.book_Description
                                             }
                         ).ToListAsync();
            if (genreId > 0)
            {

                books = books.Where(a => a.book_Id == genreId).ToList();
            }
            return books;

        }


    }
}