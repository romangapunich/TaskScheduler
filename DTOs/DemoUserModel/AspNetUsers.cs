using System;

namespace TaskScheduler.DTOs.DemoUserModel
{
    public class AspNetUsers
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int? SymptomSinglePassword { get; set; }
        public DateTime? PasswordExpirationDate { get; set; }
        public string IdMentor { get; set; }
        public bool CheckPassword { get; set; }
        public DateTime? DateEndPassword { get; set; }
        public int? IdSateUser { get; set; }
        public DateTime? DateState { get; set; }
        public string IndeficatorAgenta { get; set; }
        public bool? StateAgentCandidate { get; set; }
        public bool Anketa { get; set; }
        public bool LandingAuth { get; set; }
        public int AgentId { get; set; }
        public bool IsCerteficateAgent { get; set; }
        public DateTime? DateMentor { get; set; }
        public bool? IsVisibleallContract { get; set; }
        public DateTime? DateRoles { get; set; }
        public bool FirstChangePass { get; set; }
        public decimal? Balance { get; set; }
        public decimal? CashBalance { get; set; }
        public bool? IsLockedPayment { get; set; }

       
    }
}
