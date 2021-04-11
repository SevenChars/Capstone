using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContractMaster.models;
using System.IO;
using LINQtoCSV;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;

namespace ContractMaster.Controllers
{
    [Authorize]
    public class ContractController : Controller
    {
        private readonly ContractMasterContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ContractController(ContractMasterContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public string GenerateContractId()
        {
            int numberOfRows = _context.Contract.Count();

            string autoConId = "CON" + numberOfRows;
            return autoConId;
        }

        public string GenerateComId()
        {
            int numberOfRows = _context.Company.Count();

            string autoComId = "COM" + numberOfRows;
            return autoComId;
        }

        public string GenerateCategoryId()
        {
            int numberOfRows = _context.MainCategory.Count();

            string autoCatId = "CAT" + numberOfRows;
            return autoCatId;
        }

        public string GenerateSubcategoryId()
        {
            int numberOfRows = _context.SubCategory.Count();

            string autoSubId = "SUB" + numberOfRows;
            return autoSubId;
        }

        [AllowAnonymous]
        public IActionResult Search()
        {
            ViewData["ClientId"] = new SelectList(_context.Client.Include(c => c.ClientNavigation).OrderBy(c => c.ClientNavigation.CompanyName), "ClientId", "ClientNavigation.CompanyName");
            ViewData["BuyerId"] = new SelectList(_context.Buyer.OrderBy(c => c.BuyerName), "BuyerId", "BuyerName");
            return View();
        }

        // GET: Contract
        [AllowAnonymous]
        public async Task<IActionResult> Index(string contractNumber, string clientId, string buyerId, string contractStatus, string keyword, DateTime? startDate, DateTime? endDate, string stimeDivider, string etimeDivider)
        {
            //SendMail();

            if (contractNumber == null)
                contractNumber = "";
            if (clientId == null)
                clientId = "";
            if (buyerId == null)
                buyerId = "";
            if (keyword == null)
                keyword = "";

            var displayList = _context.Contract.Include(c => c.ContractDetail).Include(a => a.ContractDetail.Client.ClientNavigation)
                                                    .Where(c => c.ContractDetail.ContractName.Contains(contractNumber) &&
                                                                c.ContractDetail.ClientId.Contains(clientId) &&
                                                                c.ContractDetail.BuyerId.Contains(buyerId) &&
                                                               (c.ContractDetail.Client.ClientNavigation.CompanyName.Contains(keyword) ||
                                                                c.ContractDetail.Client.ClientNavigation.ContactPerson.Contains(keyword) ||
                                                                c.ContractDetail.Buyer.BuyerName.Contains(keyword) ||
                                                                c.ContractDetail.Buyer.BuyerEmail.Contains(keyword))
                                                                );
            if (contractStatus == "Current")
                displayList = displayList.Where(c => c.EndDate >= DateTime.Now.Date);
            if (contractStatus == "Expired")
                displayList = displayList.Where(c => c.EndDate < DateTime.Now.Date);

            if (startDate != null)
            {
                if (stimeDivider == "greater")
                    displayList = displayList.Where(c => c.StartDate >= startDate);
                if (stimeDivider == "less")
                    displayList = displayList.Where(c => c.StartDate <= startDate);
            }
            if (endDate != null)
            {
                if (etimeDivider == "greater")
                    displayList = displayList.Where(c => c.EndDate >= endDate);
                if (etimeDivider == "less")
                    displayList = displayList.Where(c => c.EndDate <= endDate);
            }

            return View(await displayList.ToListAsync());
        }

        public FileContentResult ExportToCsv()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"ContractId\",\"ContractName\",\"StartDate\",\"EndDate\",\"AddDate\",\"CategoryName\",\"SubcategoryName\",\"ClientName\",\"ContractorName\",\"Description\",\"BidDate\",\"BidNumber\"");

            var contractList = _context.Contract.Include(a => a.ContractDetail.Category)
                .Include(a => a.ContractDetail.Client)
                .Include(a => a.ContractDetail.SubCategory)
                .Include(a => a.ContractDetail.Client.ClientNavigation)
                .Include(a => a.ContractDetail.Contractor.ContractorNavigation)
                .Include(a => a.ContractDetail.Buyer)
                .Where(a => a.ContractId == a.ContractDetail.ContractId &&
            a.ContractDetail.CategoryId == a.ContractDetail.Category.CategoryId &&
            a.ContractDetail.SubCategoryId == a.ContractDetail.SubCategory.SubcategoryId &&
            a.ContractDetail.Client.ClientId == a.ContractDetail.Client.ClientNavigation.CompanyId &&
            a.ContractDetail.ContractorId == a.ContractDetail.Contractor.ContractorNavigation.CompanyId &&
            a.ContractDetail.BuyerId == a.ContractDetail.Buyer.BuyerId).ToList();
            foreach (var item in contractList)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\"", item.ContractId
                    , item.ContractDetail.ContractName, item.StartDate, item.EndDate, item.AddDate, item.ContractDetail.Category.CategoryName,
                    item.ContractDetail.SubCategory.SubcategoryName, item.ContractDetail.Client.ClientNavigation.CompanyName, item.ContractDetail.Contractor.ContractorNavigation.CompanyName,
                    item.ContractDetail.Description, item.ContractDetail.BidDate, item.ContractDetail.BidNumber));
            }
            var fileName = "ContractList" + DateTime.Now.ToString() + ".csv";
            return File(new System.Text.UTF8Encoding().GetBytes(sw.ToString()), "text/csv", fileName);
        }


        //Sending Email
        //public void SendMail()
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Test Project", "capstoneprojecttest222@gmail.com"));
        //    message.To.Add(new MailboxAddress("client", "15195715372@163.com"));
        //    message.Subject = "A new record has been created";
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = "A new record has been created"
        //    };
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 80, false);
        //        client.Authenticate("capstoneprojecttest222@gmail.com", "LOL12345hh");
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }
        //}



        // present form to upload a file
        public IActionResult Upload()
        {
            return View();
        }

        // read in uploaded file and copy its contents to a folder on the server
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadedFile, string targetFileName)
        {
            string pathAndFile = @"Upload/";

            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                TempData["message"] = "file is empty or null";
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(targetFileName))
            {
                // get original file's name
                Int32 lastSlash = uploadedFile.FileName.LastIndexOf(@"\");
                pathAndFile += uploadedFile.FileName.Substring(lastSlash + 1);
            }
            else
                pathAndFile += targetFileName;

            FileStream fileStream = new FileStream(pathAndFile, FileMode.Create);
            try
            {
                await uploadedFile.CopyToAsync(fileStream);
                TempData["message"] = $"file '{uploadedFile.FileName}' uploaded to '{pathAndFile}'";
            }
            catch (Exception ex)
            {
                TempData["message"] = $"exception on upload: {ex.GetBaseException().Message}";
            }
            fileStream.Close();

            ImportIntoClass(fileStream, pathAndFile);
            return RedirectToAction(nameof(Index));
        }


        public void ImportIntoClass(FileStream fileStream, string pathAndFile)
        {
            string[] contractProps = new string[6];
            var companyNames = _context.Company.Select(a => a.CompanyName);
            var categories = _context.MainCategory.Select(a => a.CategoryName);
            var subcategories = _context.SubCategory.Select(a => a.SubcategoryName);

            fileStream = new FileStream(pathAndFile, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);
            string headLine = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                bool clientIsExist = false;
                bool contractorIsExist = false;
                bool categoryIsExist = false;
                bool subcategoryIsExist = false;

                var line = reader.ReadLine();
                contractProps = line.Split(',');

                foreach (var item in companyNames)
                {
                    if (item == contractProps[3])
                        clientIsExist = true;
                    if (item == contractProps[4])
                        contractorIsExist = true;
                }

                foreach (var item in categories)
                {
                    if (item == contractProps[1])
                        categoryIsExist = true;
                }

                if (categoryIsExist == false)
                {
                    MainCategory category = new MainCategory
                    {
                        CategoryId = GenerateCategoryId(),
                        CategoryName = contractProps[1]
                    };
                    _context.Add(category);
                    _context.SaveChanges();
                }

                foreach (var item in subcategories)
                {
                    if (item == contractProps[2])
                        subcategoryIsExist = true;
                }

                if (subcategoryIsExist == false)
                {
                    SubCategory subCategory = new SubCategory
                    {
                        SubcategoryId = GenerateSubcategoryId(),
                        CategoryId = _context.MainCategory.Where(a => a.CategoryName == contractProps[1]).Select(a => a.CategoryId).FirstOrDefault(),
                        SubcategoryName = contractProps[2]
                    };
                    _context.Add(subCategory);
                    _context.SaveChanges();
                }

                if (clientIsExist == false)
                {
                    Company clientCompany = new Company
                    {
                        CompanyId = GenerateComId(),
                        CompanyName = contractProps[3]
                    };

                    Client client = new Client
                    {
                        ClientId = clientCompany.CompanyId
                    };

                    _context.Add(clientCompany);
                    _context.Add(client);
                    _context.SaveChanges();
                }

                if (contractorIsExist == false)
                {
                    Company contractorCompany = new Company
                    {
                        CompanyId = GenerateComId(),
                        CompanyName = contractProps[4]
                    };
                    Contractor contractor = new Contractor
                    {
                        ContractorId = contractorCompany.CompanyId,
                    };

                    _context.Add(contractorCompany);
                    _context.Add(contractor);
                    _context.SaveChanges();
                }

                Contract contract = new Contract
                {
                    ContractId = GenerateContractId(),
                    StartDate = DateTime.Parse(contractProps[9]),
                    EndDate = DateTime.Parse(contractProps[10])
                };
                ContractDetail contractDetail = new ContractDetail
                {
                    ContractId = contract.ContractId,
                    ContractName = contractProps[0],
                    CategoryId = _context.MainCategory.Where(a => a.CategoryName == contractProps[1]).Select(a => a.CategoryId).FirstOrDefault(),
                    SubCategoryId = _context.SubCategory.Where(a => a.SubcategoryName == contractProps[2]).Select(a => a.SubcategoryId).FirstOrDefault(),
                    ClientId = _context.Company.Where(a => a.CompanyName == contractProps[3]).Select(a => a.CompanyId).FirstOrDefault(),
                    ContractorId = _context.Company.Where(a => a.CompanyName == contractProps[4]).Select(a => a.CompanyId).FirstOrDefault()
                };

                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(contract);
                        _context.Add(contractDetail);
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    TempData["message"] = $"exception import: {ex.GetBaseException().Message}";
                }
                //}
            }
            reader.Close();
            TempData["message"] = "imorted";

        }


        // GET: Contract/Details/5
        public async Task<IActionResult> Details(string id, ContractDetail contractDetail, Client client, Contractor contractor, Company company)
        {
            if (id == null)
            {
                TempData["message"] = "Can not find contract: ID is null";
                return Redirect("/Contract/Index");
            }

            var contract = await _context.Contract
                .Include(a => a.ContractDetail.SubCategory)
                .Include(a => a.ContractDetail.Category)
                .Include(a => a.ContractDetail.Client.ClientNavigation)
                .Include(a => a.ContractDetail.Contractor.ContractorNavigation)
                .FirstOrDefaultAsync(m => m.ContractId == id);
            if (contract == null)
            {
                TempData["message"] = "Can not find contract: No contract match with ID.";
                return Redirect("/Contract/Index");
            }

            return View(contract);
        }

        // GET: Contract/Create
        public IActionResult Create()
        {
            Contract contract = new Contract
            {
                ContractId = GenerateContractId()
            };
            var categories = _context.SubCategory.Include(s => s.Category).OrderBy(c => c.Category.CategoryName);
            foreach (var item in categories)
            {
                item.SubcategoryName = item.Category.CategoryName + " - " + item.SubcategoryName;
            }
            ViewData["SubcategoryId"] = new SelectList(categories, "SubcategoryId", "SubcategoryName");
            ViewData["ClientId"] = new SelectList(_context.Client.Include(a => a.ClientNavigation).OrderBy(a => a.ClientNavigation.CompanyName), "ClientId", "ClientNavigation.CompanyName");
            ViewData["ContractorId"] = new SelectList(_context.Contractor.Include(a => a.ContractorNavigation).OrderBy(a => a.ContractorNavigation.CompanyName), "ContractorId", "ContractorNavigation.CompanyName");
            ViewData["BuyerId"] = new SelectList(_context.Buyer.OrderBy(a => a.BuyerName), "BuyerId", "BuyerName");
            return View(contract);
        }

        // POST: Contract/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContractId,StartDate,EndDate,AddDate")] Contract contract,
                                                [Bind("ContractId,ContractName,CategoryId, ClientId, ContractorId, SubCategoryId,Description, BuyerId, BidDate,BidNumber")] ContractDetail contractDetail,
                                                [Bind("ContractId")] ProjectData projectData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contract);
                contractDetail.ContractId = contract.ContractId;
                projectData.ContractId = contract.ContractId;
                contractDetail.CategoryId = (_context.SubCategory.Where(a => a.SubcategoryId == contractDetail.SubCategoryId)
                                          .Select(a => a.CategoryId)).FirstOrDefault();
                _context.Add(projectData);
                _context.Add(contractDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            Create();

            return View(contract);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                TempData["message"] = "Can not find contract: ID is null";
                return Redirect("/Contract/Index");
            }
            var contract = await _context.Contract.Include(a => a.ContractDetail).Include(a => a.ProjectData).Include(a => a.ContractDetail.Contractor.ContractorNavigation.ContactPersonNavigation).FirstOrDefaultAsync(a => a.ContractId == id);
            if (contract == null)
            {
                TempData["message"] = "Can not find contract: No contract match with ID.";
                return Redirect("/Contract/Index");
            }
            Create();
            return View(contract);
        }

        // POST: Contract/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ContractId,StartDate,EndDate,AddDate")] Contract contract,
                                                         [Bind("ContractId,ContractName, Description, CategoryId, SubCategoryId, ClientId, ContractorId,Contractor, BuyerId, BidDate,BidNumber,Name")] ContractDetail contractDetail,
                                                         [Bind("TenderTitle,Description,PromisedDays,ActualStartDate,ActualCompeletionDate,ContractAwardAmount,ContractCompletionAmount")] ProjectData projectData)
        {
            if (id != contract.ContractId)
            {
                TempData["message"] = "Can not edit contract: ID is changed.";
                return Redirect($"/Contract/Index/{id}");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    contractDetail.Contractor.ContractorId = contractDetail.ContractorId;
                    contractDetail.Contractor.ContractorNavigation.CompanyId = contractDetail.ContractorId;
                    contractDetail.Contractor.ContractorNavigation.CompanyName = _context.Company.Where(a => a.CompanyId == contractDetail.ContractorId).FirstOrDefault().CompanyName;

                    var contactPerson = (from record in _context.ContactPerson
                                         join aContract in _context.ContractDetail on record.CompanyId equals aContract.ContractorId
                                         where record.CompanyId == contractDetail.ContractorId
                                         select new ContactPerson
                                         {
                                             Id = _context.ContactPerson.Where(a => a.CompanyId == contractDetail.ContractorId).FirstOrDefault().Id,
                                             Name = _context.ContactPerson.Where(a => a.CompanyId == contractDetail.ContractorId).FirstOrDefault().Name,
                                             Phone = contractDetail.Contractor.ContractorNavigation.ContactPersonNavigation.Phone,
                                             Email = contractDetail.Contractor.ContractorNavigation.ContactPersonNavigation.Email,
                                             CompanyId = _context.Company.Where(a => a.CompanyId == contractDetail.ContractorId).FirstOrDefault().CompanyId,
                                             Company = _context.Company.Where(a => a.CompanyId == contractDetail.ContractorId).FirstOrDefault()
                                         }).SingleOrDefault();
                                        
                                        


                        //_context.ContactPerson
                        //.Where(a => a.CompanyId == contractDetail.ContractorId).Select()


                    contractDetail.ContractId = contract.ContractId;
                    contractDetail.CategoryId = (_context.SubCategory.Where(a => a.SubcategoryId == contractDetail.SubCategoryId)
                                          .Select(a => a.CategoryId)).FirstOrDefault();
                    _context.Update(contract);
                    projectData.ContractId = contract.ContractId;
                    _context.Update(projectData);
                    _context.Update(contractDetail);
                    _context.Update(contactPerson);

                await _context.SaveChangesAsync();
            }
                catch (Exception ex)
            {
                if (!ContractExists(contract.ContractId))
                {
                    TempData["message"] = "Can not edit contract: Contract is not exist.";
                    return Redirect("/Contract/Index");
                }
                else
                {
                    TempData["message"] = "Error editing contract: " + ex.GetBaseException().Message;
                    return Redirect("/Contract/Index");
                }
            }
            return RedirectToAction(nameof(Index));
        }
        Create();
            return View(contract);
    }

    // GET: Contract/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            TempData["message"] = "Can not find contract: ID is null";
            return Redirect("/Contract/Index");
        }

        var contract = await _context.Contract
            .FirstOrDefaultAsync(m => m.ContractId == id);
        var contractDetail = await _context.ContractDetail
            .FirstOrDefaultAsync(m => m.ContractId == id);
        if (contract == null)
        {
            TempData["message"] = "Can not find contract: No contract match with ID.";
            return Redirect("/Contract/Index");
        }

        return View(contract);
    }

    // POST: Contract/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var projectData = await _context.ProjectData.FindAsync(id);
        var contractDetail = await _context.ContractDetail.FindAsync(id);
        var contract = await _context.Contract.FindAsync(id);
        _context.ProjectData.Remove(projectData);
        _context.ContractDetail.Remove(contractDetail);
        _context.Contract.Remove(contract);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ContractExists(string id)
    {
        return _context.Contract.Any(e => e.ContractId == id);
    }
}
}
