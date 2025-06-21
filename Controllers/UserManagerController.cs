using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CameraView.Models;
using CameraView.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CameraView.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly AppDbContext _context;

        public UserManagerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /UserManager
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: /UserManager/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /UserManager/Create
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                ModelState.AddModelError("", "Tên đăng nhập và mật khẩu không được để trống.");
                return View(user);
            }

            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                return View(user);
            }

            // ✅ Mã hóa mật khẩu
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Tạo người dùng thành công!";
            return RedirectToAction("Index");
        }

        // GET: /UserManager/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /UserManager/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id) return BadRequest();

            if (string.IsNullOrWhiteSpace(updatedUser.Username) || string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                ModelState.AddModelError("", "Tên đăng nhập và mật khẩu không được để trống.");
                return View(updatedUser);
            }

            if (await _context.Users.AnyAsync(u => u.Id != id && u.Username == updatedUser.Username))
            {
                ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                return View(updatedUser);
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null) return NotFound();

            existingUser.Username = updatedUser.Username;
            existingUser.Role = updatedUser.Role;

            // ✅ Chỉ mã hóa lại nếu mật khẩu có thay đổi
            var hasher = new PasswordHasher<User>();
            var passwordCheck = hasher.VerifyHashedPassword(existingUser, existingUser.Password, updatedUser.Password);
            if (passwordCheck != PasswordVerificationResult.Success)
            {
                existingUser.Password = hasher.HashPassword(existingUser, updatedUser.Password);
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật người dùng thành công!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /UserManager/DeleteConfirmed/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Đã xoá người dùng thành công.";
            return RedirectToAction("Index");
        }
    }
}
