using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesManagment.Data;
using EmployeesManagment.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeesManagment.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var pendingstatus = _context.SystemCodeDetails.
               Include(x => x.systemCode).
               Where(t => t.Code == "Pending" && t.systemCode.Code == "LeaveApprovalStatus")
               .FirstOrDefault();
            var applicationDbContext = _context.LeaveApplications.
                Include(l => l.Duration).
                Include(l => l.Employee).
                Include(l => l.Status).
                Include(l => l.leaveType).Where(x => x.StatusId == pendingstatus.Id);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> ApprovedLeaveApplications()
        {
            var Approvedstatus = _context.SystemCodeDetails.
                Include(x => x.systemCode)
                .Where(t => t.Code == "Approved" && t.systemCode.Code == "LeaveApprovalStatus")
                .FirstOrDefault();
            var applicationDbContext = _context.LeaveApplications.
                Include(l => l.Duration).
                Include(l => l.Employee).
                Include(l => l.Status).
                Include(l => l.leaveType).Where(x => x.StatusId == Approvedstatus.Id);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> RejectedLeaveApplications()
        {
            var Rejectedstatus = _context.SystemCodeDetails.
                Include(x => x.systemCode)
                .Where(t => t.Code == "Rejected" && t.systemCode.Code == "LeaveApprovalStatus")
                .FirstOrDefault();
            var applicationDbContext = _context.LeaveApplications.
                Include(l => l.Duration).
                Include(l => l.Employee).
                Include(l => l.Status).
                Include(l => l.leaveType).Where(x => x.StatusId == Rejectedstatus.Id);
            return View(await applicationDbContext.ToListAsync());
        }
       

        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {

            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(c => c.systemCode).Where(t => t.systemCode.Code == "Leave Duration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Emplyees, "Id", "FullName");
            ViewData["leaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            var model = new LeaveApplication
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(15)
            };
            return View(model);
            // return View();
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {

            var pendingstatus = _context.SystemCodeDetails.
                Include(x => x.systemCode).
                Where(t => t.Code == "Pending" && t.systemCode.Code == "LeaveApprovalStatus")
                .FirstOrDefault();
            ModelState.Remove("Status");
            ModelState.Remove("Duration");
            ModelState.Remove("Employee");
            ModelState.Remove("leaveType");

            TryValidateModel(ModelState);

            if (ModelState.IsValid)
            {
                leaveApplication.CreatedOn = DateTime.Now;
                leaveApplication.CreatedById = "Alaa";
                leaveApplication.StatusId = pendingstatus.Id;
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(c => c.systemCode).Where(t => t.systemCode.Code == "Leave Duration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Emplyees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["leaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.leaveTypeId);

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(c => c.systemCode).Where(t => t.systemCode.Code == "Leave Duration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Emplyees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["leaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.leaveTypeId);

            return View(leaveApplication);
        }
        [HttpGet]
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }
        


        [HttpPost]
        public async Task<IActionResult> ApproveLeave(int? id,LeaveApplication leave)
        {
            var Approvedstatus = _context.SystemCodeDetails.
                Include(x => x.systemCode)
                .Where(t => t.Code == "Approved" && t.systemCode.Code == "LeaveApprovalStatus")
                .FirstOrDefault();
            

            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            leaveApplication.StatusId = Approvedstatus!.Id;
            if (leaveApplication == null)
            {
                return NotFound();
            }
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Alaa";
            leaveApplication.ApprovalNotes= leave.ApprovalNotes;
            _context.Update(leaveApplication);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> RejectLeave(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }
        [HttpPost]
        public async Task<IActionResult> RejectLeave(int? id, LeaveApplication leave)
        {
            var Rejectedstatus = _context.SystemCodeDetails.
                Include(x => x.systemCode)
                .Where(t => t.Code == "Rejected" && t.systemCode.Code == "LeaveApprovalStatus")
                .FirstOrDefault();

            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            leaveApplication.StatusId = Rejectedstatus.Id;
            if (leaveApplication == null)
            {
                return NotFound();
            }
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Alaa";
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;

            _context.Update(leaveApplication);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Status");
            ModelState.Remove("Duration");
            ModelState.Remove("leaveType");
            ModelState.Remove("Employee");


            TryValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var pendingstatus = _context.SystemCodeDetails.
               Include(x => x.systemCode).
               Where(t => t.Code == "Pending" && t.systemCode.Code == "LeaveApprovalStatus")
               .FirstOrDefault();

                try
                {
                  
                    
                    leaveApplication.StatusId = pendingstatus.Id;
                    leaveApplication.ModifiedById = "Alaa";
                    leaveApplication.ModifiedOn = DateTime.Now;

                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationId"] = new SelectList(_context.SystemCodeDetails.Include(c => c.systemCode).Where(t => t.systemCode.Code == "Leave Duration"), "Id", "Description", leaveApplication.DurationId);
            ViewData["EmployeeId"] = new SelectList(_context.Emplyees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["leaveTypeId"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.leaveTypeId);

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Include(l => l.leaveType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
