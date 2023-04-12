using HealthInsurance.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HealthInsurance.Controllers
{
    public class PoliciesController : Controller
    {
        private readonly HealthInsuranceContext _context;

        public PoliciesController(HealthInsuranceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Policies != null ?
                    View(await _context.Policies.ToListAsync()) :
                    Problem("Entity set 'HealthInsuranceContext.Policies is null'");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == id);

            if(policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyId,PolicyName,PolicyDescription,Amount,Emi,CompanyId")] Policy policy)
        {
            _context.Add(policy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyName");
            return View(policy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PolicyId, PolicyName,PolicyDescription , Amount, Emi, CompanyId")] Policy policy)
        {
            if (id != policy.PolicyId)
            {
                return NotFound();
            }

            
                try
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.PolicyId))
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


        // GET: Policies/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies
                .FirstOrDefaultAsync(m => m.PolicyId == id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        // POST: Policies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Policies == null)
            {
                return Problem("Entity set 'HealthInsuranceContext.Policies'  is null.");
            }
            var policy = await _context.Policies.FindAsync(id);
            if (policy != null)
            {
                _context.Policies.Remove(policy);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RequestDetail()
        {
            var policiesReqDetails = await _context.PoliciesReqDetails.ToListAsync();
            if (policiesReqDetails == null)
            {
                return Problem("Entity set 'HealthInsuranceContext.PoliciesReqDetails' is null.");
            }

            return View("RequestDetails", policiesReqDetails);
        }

        public IActionResult RequestDetails()
        {
            return View(_context.PoliciesReqDetails);
        }




        // GET: Policies/Adjust/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Adjust(int? id)
        {
            if (id == null || _context.PoliciesReqDetails == null)
            {
                return NotFound();
            }

            var policy = await _context.PoliciesReqDetails.FindAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            ViewBag.Policy = policy; // truyền đối tượng policy vào ViewBag

            return View(policy);
        }
        private bool PolicyExists(int id)
        {
            return (_context.Policies?.Any(e => e.PolicyId == id)).GetValueOrDefault();
        }

        // POST: Policies/Adjust/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adjust(int? id, [Bind("RequestId,RequestDate,EmployeeId,Status,Emi,PolicyId,PolicyName,PolicyAmount,CompanyId,CompanyName")] PoliciesReqDetail policy)
        {
            if (id != policy.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra điều kiện EmployeeId của policy và employee có bằng nhau hay không
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == policy.EmployeeId);
                    if (employee != null && policy.EmployeeId == employee.EmployeeId && policy.PolicyId == employee.PolicyId)
                    {
                        _context.Update(policy);
                        await _context.SaveChangesAsync();

                        // Cập nhật trường PolicyStatus của Employee
                        employee.PolicyStatus = policy.Status;
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(RequestDetails), new { id = policy.RequestId });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PolicyExists(policy.RequestId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(policy);
        }

        public IActionResult PoliciesEmpList()
        {
            return View();
        }
    } 
}
