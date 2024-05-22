using Microsoft.AspNetCore.Mvc;
using PhoneNumberDetectionWeb.Models;
using PhoneNumberDetectionWeb.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PhoneNumberDetectionWeb.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new PhoneNumberViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PhoneNumberViewModel model)
        {
            string inputText = model.InputText;
            model.Errors = new List<string>(); // Initialize error list

            if (model.UploadedFile != null && model.UploadedFile.Length > 0)
            {
                if (model.UploadedFile.ContentType != "text/plain")
                {
                    model.Errors.Add("Only text files are supported.");
                }
                else
                {
                    try
                    {
                        using (var reader = new StreamReader(model.UploadedFile.OpenReadStream()))
                        {
                            inputText = await reader.ReadToEndAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        model.Errors.Add($"An error occurred while reading the file: {ex.Message}");
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(inputText))
            {
                try
                {
                    model.DetectedNumbers = PhoneNumberDetector.DetectPhoneNumbers(inputText);
                }
                catch (Exception ex)
                {
                    model.Errors.Add($"An error occurred while detecting phone numbers: {ex.Message}");
                }
            }
            else if (model.UploadedFile == null || model.UploadedFile.Length == 0)
            {
                model.Errors.Add("Please enter text or upload a text file.");
            }

            if (model.DetectedNumbers == null)
            {
                model.DetectedNumbers = new List<PhoneNumberInfo>();
            }

            return View(model);
        }
    }
}
