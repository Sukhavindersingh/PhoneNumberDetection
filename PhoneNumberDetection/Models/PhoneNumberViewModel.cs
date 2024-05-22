using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PhoneNumberDetectionWeb.Models
{
    public class PhoneNumberViewModel
    {
        public string InputText { get; set; }
        public IFormFile UploadedFile { get; set; }
        public List<PhoneNumberInfo> DetectedNumbers { get; set; } = new List<PhoneNumberInfo>();
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class PhoneNumberInfo
    {
        public string Number { get; set; }
        public string Format { get; set; }
    }
}


