using HealthInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HealthInsuranceContext _context;

        public EmployeeController(HealthInsuranceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Employees != null ?
                View(await _context.Employees.ToListAsync()) : 
                Problem("Entity set 'HealthInsuranceContext.Employee is null'");
        }

   

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }

        public IActionResult Create()
        {
            var model = new Employee(); // Tạo một đối tượng Employee mới để truyền vào view
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {


            if (ModelState.IsValid)
            {

                var model1 = new Employee
                {
                    ContactNo = employee.ContactNo,
                    Firstname = employee.Firstname,
                    Lastname = employee.Lastname,
                    Address = employee.Address,
                    PolicyId = employee.PolicyId,
                    Joindate = DateTime.Now,
                    Designation = "No Job",
                    PolicyStatus = employee.PolicyStatus


                };
                _context.Employees.Add(model1);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Employee");
            }
            return View(employee);
        }



        // update emp details
        public async Task<IActionResult> UpdateEmployeeDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null || id != employee.EmployeeId)
            {
                return NotFound();
            }

            ViewBag.Employee = employee; // truyền đối tượng emp vào ViewBag

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmployeeDetails(int? id, [Bind("EmployeeId,Designation,Firstname,Lastname,ContactNo,Joindate,Address,IdAccount,PolicyId,PolicyStatus")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), "Employee");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(employee);
        }
    }









}
