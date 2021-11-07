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
        public static List<Faculty> FacultySeed()
        {
            return new List<Faculty>
            { 
                new Faculty
                {
                    Name = "Faculty of Computer and Informatics",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Informatics"
                        },
                        new Department
                        {
                            Name = "Computer Science"
                        }
                    }
                },
                 new Faculty
                {
                    Name = "Faculty of Engineering",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Mechanical and Marine Engineering"
                        },
                        new Department
                        {
                            Name = "Electrical and Computer Engineering"
                        },
                        new Department
                        {
                            Name = "Civil and Environmental Engineering"
                        },
                        new Department
                        {
                            Name = "Mining and Process Engineering"
                        }
                    }
                },
                  new Faculty
                {
                    Name = "Health and Applied Sciences",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Natural and Applied Sciences"
                        },
                        new Department
                        {
                            Name = "Health Sciences"
                        },
                        new Department
                        {
                            Name = "Mathematics and Statistics"
                        }
                    }
                },
                   new Faculty
                {
                    Name = "Faculty of Human Sciences",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Communication"
                        },
                        new Department
                        {
                            Name = "Technical and Vocational Education and Training"
                        },
                        new Department
                        {
                            Name = "Social Sciences"
                        }
                    }
                },
                    new Faculty
                {
                    Name = "Faculty of Management Sciences",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Management"
                        },
                        new Department
                        {
                            Name = "Marketing and Logistics"
                        },
                        new Department
                        {
                            Name = "Accounting, Economics and Finance"
                        }
                    }
                },
                     new Faculty
                {
                    Name = "Faculty of Natural Resources and Spatial Sciences",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Architecture and Spatial Planning"
                        },
                        new Department
                        {
                            Name = "Agriculture and Natural Resources Sciences"
                        },
                        new Department
                        {
                            Name= "Geo-Spatial Sciences and Technology"
                        },
                        new Department
                        {
                            Name = "Land and Property Sciences"
                        }
                    }
                },
                       new Faculty
                {
                    Name = "Faculty of Accounting and Economics",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Accounting and Economics"
                        },
                    }
                },
                        new Faculty
                {
                    Name = "Harold Pupkewitz Graduate School of Business",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Harold Pupkewitz Graduate School of Business"
                        },
                    }
                },
                         new Faculty
                {
                    Name = "Centre for Open and Lifelong Learning",
                    Departments = new List<Department>
                    {
                        new Department
                        {
                            Name= "Centre for Open and Lifelong Learning"
                        },
                    }
                },
            };
        }
        public static List<DefaultUser> DefaultUserSeed()
        {
            return new List<DefaultUser>
            {
                new DefaultUser(TitleEnum.Mr,"System","Admin",UserRoleEnum.Admin,"admin@email.com","iAmSystemAdmin")
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

       
    }
}
