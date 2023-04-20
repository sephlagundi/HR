using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRWeb.Data;
using HRWeb.Models;

namespace HRWeb.Controllers
{
    public class LeavesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeavesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Leaves
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Leaves.Include(l => l.Employee).Include(l => l.LeaveType);

            // Check for TempData messages and add them to the ViewBag
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Leaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leaves/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveType, "Id", "Name");
            return View();
        }

        // POST: Leaves/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LeaveStartDate,LeaveEndDate,Reason,Status,EmployeeId,LeaveTypeId")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                leave.Status = "Pending";

                _context.Add(leave);
                await _context.SaveChangesAsync();

                // Add success message to TempData
                TempData["SuccessMessage"] = "Leave request has been created successfully.";

                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", leave.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveType, "Id", "Name", leave.LeaveTypeId);

            // Add error message to TempData
            TempData["ErrorMessage"] = "Failed to create leave request.";

            return View(leave);
        }

        // GET: Leaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", leave.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveType, "Id", "Name", leave.LeaveTypeId);
            return View(leave);
        }

        // POST: Leaves/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LeaveStartDate,LeaveEndDate,Reason,EmployeeId,LeaveTypeId,Status")] Leave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the status property
                    // Set the status to "Pending" or update to the desired status
                    _context.Update(leave);
                    await _context.SaveChangesAsync();

                    // Add success message to TempData
                    TempData["SuccessMessage"] = "Leave request has been updated successfully.";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", leave.EmployeeId);
            ViewData["LeaveTypeId"] = new SelectList(_context.LeaveType, "Id", "Name", leave.LeaveTypeId);

            // Add error message to TempData
            TempData["ErrorMessage"] = "Failed to update leave request.";

            return View(leave);
        }


        // GET: Leaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leaves == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leaves == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Leaves'  is null.");
            }
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveExists(int id)
        {
          return (_context.Leaves?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
