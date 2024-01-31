using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class CustomerController : Controller
    {
        
        private InsuranceDbContext dbContext;  
        public CustomerController()
        {
            dbContext = new InsuranceDbContext(); 
        }
      
        public ActionResult Dashboard()  
        {
            return View();  
        }
        public ActionResult GetAllCustomers() 
        {
            var customer = dbContext.Customers.ToList();  
            return View(customer);
        }
        
        public ActionResult GetAllUsers()    
        {
            var user = dbContext.Customers.ToList(); 
            return View(user);
        }
        public ActionResult ViewPoliciesToApply()  
        {
            List<Policy> policies = dbContext.Policies.ToList();  
            return View(policies);
        }
        public ActionResult Apply(int policyId)  
        {
            int customerId = 1;
           
            bool AppliedAlready = dbContext.AppliedPolicies
            .Any(ap => ap.CustomerId == customerId && ap.AppliedPolicyId == policyId);
            if (!AppliedAlready)
            {
              
                Policy policy = dbContext.Policies.FirstOrDefault(p => p.PolicyId == policyId);

                if (policy != null)
                {
                   
                    AppliedPolicy appliedPolicy = new AppliedPolicy
                    {
                        PolicyNumber = policy.PolicyNumber,
                        AppliedDate = DateTime.Now,
                        Category = policy.Category,
                        CustomerId = customerId
                    };
              
                    dbContext.AppliedPolicies.Add(appliedPolicy);
                    dbContext.SaveChanges();
                }
                else
                {
               
                }
            }
         
            return RedirectToAction("AppliedPolicies");
        }
        public ActionResult AppliedPolicies()   
        {
            List<AppliedPolicy> appliedPolicies;   

            using (var dbContext = new InsuranceDbContext())
            {
                
                appliedPolicies = dbContext.AppliedPolicies.ToList();
            }
            return View(appliedPolicies);
        }
        public ActionResult Categories()  
        {
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }
        //public ActionResult AskQuestion()   
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult AskQuestion(QuestionView questionview)
        //{
        //    var userId = Session["UserId"] as int?;

        //    if (!userId.HasValue)
        //    {
        //        // Handle the case where userId is null (optional)
        //        // For example, you might want to redirect to a login page or throw an exception
        //        return RedirectToAction("Login"); // Replace "Login" with the appropriate action/method
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        Questions newQuestion = new Questions
        //        {
        //            Question = questionview.Question,
        //            Date = questionview.Date,
        //            Answer = questionview.Answer,
        //            CustomerId = userId.Value
        //        };

        //        dbContext.Questions.Add(newQuestion);
        //        dbContext.SaveChanges();

        //        return RedirectToAction("Success");
        //    }

        //    return View(questionview);
        //}

        public ActionResult AskQuestion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AskQuestion(QuestionView questionview)
        {
            var userId = Session["UserId"] as int?;

            if (!userId.HasValue)
            {
                // Handle the case where userId is null (optional)
                // For example, you might want to redirect to a login page or throw an exception
                return RedirectToAction("Login"); // Replace "Login" with the appropriate action/method
            }

            if (ModelState.IsValid)
            {
                Questions newQuestion = new Questions
                {
                    Question = questionview.Question,
                    Date = questionview.Date,
                    Answer = questionview.Answer,
                    CustomerId = userId.Value
                };

                dbContext.Questions.Add(newQuestion);
                dbContext.SaveChanges();

                // Redirect to the action that displays questions based on the user's session ID
                return RedirectToAction("DisplayQuestionsByCustomerId", new { customerId = userId.Value });
            }

            return View(questionview);
        }


        public ActionResult Success() 
        {
            return View();
        }
        protected override void Dispose(bool disposing)   
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AskCustomerId()  
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult DisplayQuestionsByCustomerId(int? customerId)
        {
            var userId = Session["UserId"] as int?;

            if (!userId.HasValue)
            {
                // Handle the case where userId is null (optional)
                // For example, you might want to redirect to a login page or throw an exception
                return RedirectToAction("Login"); // Replace "Login" with the appropriate action/method
            }

            if (!customerId.HasValue || customerId.Value != userId.Value)
            {
                // Ensure that the requested customer ID matches the session user ID
                return RedirectToAction("Error");
            }

            var questions = dbContext.Questions.Where(q => q.CustomerId == customerId.Value).ToList();
            ViewBag.CustomerId = customerId.Value;
            return View(questions);
        }

    }
}