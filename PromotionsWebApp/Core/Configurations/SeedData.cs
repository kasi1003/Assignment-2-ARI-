using PromotionsWebApp.Domain.Abstract;
using PromotionsWebApp.Domain.Entities;
using PromotionsWebApp.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Core.Configurations
{
    public static class SeedData
    {
        public static List<DefaultUser> DefaultUserSeed()
        {
            return new List<DefaultUser>
            {
                new DefaultUser(TitleEnum.Mr,"System","Admin",UserRoleEnum.Admin,DepartmentEnum.ComputingAndInformatics,"admin@email.com","iAmSystemAdmin"),
                new DefaultUser(TitleEnum.Mr,"John","Snow",UserRoleEnum.HOD,DepartmentEnum.ComputingAndInformatics,"johnsnow@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"John","Wick",UserRoleEnum.Dean,DepartmentEnum.ComputingAndInformatics,"johnwick@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"Clark","Kent",UserRoleEnum.IFEC,DepartmentEnum.ComputingAndInformatics,"clarkkent@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr,"Barry","Allen",UserRoleEnum.PSPC,DepartmentEnum.ComputingAndInformatics,"barryallen@email.com","promotion1"),
                new DefaultUser(TitleEnum.Mr, "Bruce", "Wayne", UserRoleEnum.VC, DepartmentEnum.ComputingAndInformatics, "brucewayne@email.com", "promotion1")
            }; 
        }
        public static List<Rank> RankSeed()
        {
            return new List<Rank>
            {
                new Rank("Junior Lecturer",
                "Have a relevant Honours degree (NQF Level 8) " +
                "with at least two years of lecturing experience at tertiary education " +
                "level and/or industry experience or an equivalent combination of relevant " +
                "professional experience.  Excellent English communication skills (oral and written). " +
                "Registered for a relevant Master’s degree (NQF Level 9).",
                8),
                new Rank("Lecturer",
                "A relevant Master’s degree (NQF Level 9) with at least " +
                "four years of lecturing experience at tertiary education " +
                "level and/or industry experience or an equivalent combination " +
                "of relevant professional experience.  " +
                "Excellent English communication skills (oral and written)." +
                " Competence to establish professional networks and to maintain " +
                "links with the industry, as well as experience in developing " +
                "undergraduate programs. Demonstrated potential in conducting " +
                "research and the supervision of undergraduate students. " +
                "Governance and leadership potential at faculty and/or departmental " +
                "level. Proven competence in and involvement in community engagement " +
                "initiatives.",
                9),
                new Rank("Senior Lecturer",
                "A relevant Doctorate’s degree (NQF Level 10) with at least " +
                "six years of lecturing experience at tertiary education level " +
                "and/or industry experience or an equivalent combination of " +
                "relevant professional experience.  Excellent English " +
                "communication skills (oral and written). " +
                "Proven competence in successful sourcing of research or " +
                "project funding from third-party sources, successful " +
                "initiation, and management of research projects, " +
                "curriculum development, and strong management/organizational " +
                "and mentorship skills. Competence to establish professional " +
                "networks and to maintain links with the industry, as well as " +
                "experience in developing postgraduate programs, strong " +
                "management skills and a proven record of raising substantial " +
                "research funding. Proven governance and leadership capability " +
                "at faculty and/or departmental level. Proven competence and " +
                "involvement in community engagement initiatives. " +
                "Good research profile including three peer-reviewed " +
                "journals/books/conference proceedings and successful " +
                "supervision of at least one Master’s (research) student)",
                10),
                new Rank("Associate Professor",
                "A relevant Doctorate (NQF Level 10) with at least seven years " +
                "of lecturing experience at tertiary education level and/or " +
                "industry experience or an equivalent combination of relevant " +
                "professional experience.  Excellent English communication skills " +
                "(oral and written). Proven competence in successful sourcing of " +
                "research or project funding from third-party sources, successful " +
                "initiation, and management of research projects, curriculum " +
                "development, and strong management/organizational and mentorship " +
                "skills. Competence to establish professional networks and to " +
                "maintain links with the industry, as well as experience in " +
                "developing postgraduate programs, strong management skills " +
                "and a proven record of raising substantial research funding. " +
                "Sound research profile including ten peer-reviewed j" +
                "ournals/books/conference proceedings with an H-Index of 4 " +
                "and successful supervision of at least three Master’s and/or " +
                "Doctoral (research) students. To be appointed the rank of " +
                "Associate Professor the candidate requires a minimum total " +
                "score of 55 points or above.",
                10),
                new Rank("Professor",
                "A relevant Doctorate (NQF Level 10) with at least seven years " +
                "of lecturing experience at tertiary education level and/or industry " +
                "experience or an equivalent combination of relevant professional experience. " +
                "Excellent English communication skills (oral and written). " +
                "Proven competence in successful sourcing of research or project funding " +
                "from third-party sources, successful initiation, and management of research " +
                "projects, curriculum development, and strong management/organizational and " +
                "mentorship skills. Competence to establish professional networks and to " +
                "maintain links with the industry, as well as experience in developing " +
                "postgraduate programs, strong management skills and a proven record of raising " +
                "substantial research funding.  Sound research profile including twenty " +
                "peer-reviewed journals/books/conference proceedings with an H-Index of 8 " +
                "and successful supervision of at least three Master’s (research) and one " +
                "Doctoral student. To be appointed the rank of Associate Professor the " +
                "candidate requires a minimum total score of 85 points or above.",
                10)
            };
        }
        //public static List<Vendor> VendorSeed()
        //{
        //    return new List<Vendor>
        //    {
        //        new Vendor("Hishiko Technologies", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840"),
        //        new Vendor("Tuliza Inc", "dhishiko@gmail.com", "Tulonga Hishiko","0815542840")
        //    };
        //}
       
    }
}
