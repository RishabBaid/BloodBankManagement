using System;

namespace BloodBankAPI.Models
{
    public class BloodBankEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Auto-generated unique identifier
        public string DonorName { get; set; } // Name of the donor
        public int Age { get; set; } // Donor's age
        public string BloodType { get; set; } // Blood group (e.g., A+, O-)
        public string ContactInfo { get; set; } // Contact details (phone/email)
        public int Quantity { get; set; } // Quantity of blood donated (in ml)
        public DateTime CollectionDate { get; set; } // Date when blood was collected
        public DateTime ExpirationDate { get; set; } // Expiration date of the blood unit
        public string Status { get; set; } // Status (e.g., "Available", "Requested", "Expired")
    }
}
