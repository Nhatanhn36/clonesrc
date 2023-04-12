using HealthInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthInsurance.Controllers
{
    public class PoliciesEmpController : Controller
    {
        private readonly HealthInsuranceContext _context;

        public PoliciesEmpController(HealthInsuranceContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Policies != null ?
                        View(await _context.Policies.ToListAsync()) :
                        Problem("Entity set 'HealthInsuranceContext.Policies'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Policies == null)
            {
                return NotFound();
            }

            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == id);

            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        private bool PolicyExists(int id)
        {
            return (_context.Policies?.Any(e => e.PolicyId == id)).GetValueOrDefault();
        }










        public async Task<IActionResult> Buy(int id)
        {
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == id);

            var employee = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == policy.CompanyId);

            if (policy == null || employee == null)
            {
                return NotFound();
            }

            var model = new PoliciesReqDetail
            {

                PolicyId = id,
                PolicyName = policy.PolicyName,
                CompanyId = policy.CompanyId,
                PolicyAmount = policy.Amount,
                CompanyName = employee.CompanyName,
                Emi = policy.Emi
            };

            return View("Buy", model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(PoliciesReqDetail model)
        {
            if (model == null || model.PolicyId == null)
            {
                return View(model);
            }

            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == model.PolicyId);
            var employee = await _context.Companies.FirstOrDefaultAsync(p => p.CompanyId == model.CompanyId);


            if (policy == null)
            {
                return NotFound();
            }

            try
            {

                //ghi de
                var model1 = new PoliciesReqDetail
                {
                    PolicyId = policy.PolicyId,
                    PolicyName = policy.PolicyName,
                    CompanyId = policy.CompanyId,
                    PolicyAmount = policy.Amount,
                    RequestDate = DateTime.Now,
                    Status = "Waiting",
                    EmployeeId = model.EmployeeId,
                    CompanyName = employee.CompanyName,
                    Emi = policy.Emi
                };

                _context.Add(model1);
                await _context.SaveChangesAsync();


                // Kiểm tra điều kiện policyid của policyreq và employee có bằng nhau hay không
                var emp = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == model.EmployeeId);
                if (emp != null && model.EmployeeId == model.EmployeeId)
                {
                    _context.Update(policy);
                    await _context.SaveChangesAsync();


                    // Cập nhật trường PolicyId của Employee
                    emp.PolicyId = model.PolicyId;
                    emp.PolicyStatus = "Waiting"; // Cập nhật giá trị của trường PolicyStatus
                    _context.Update(emp);
                    await _context.SaveChangesAsync();




                    return RedirectToAction("RequestDetails", "Policies", new { id = model.RequestId });
                }

                return RedirectToAction("RequestDetails", "Policies", new { id = model.RequestId });

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PolicyExists(model.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


        }




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










        
    } 
}


