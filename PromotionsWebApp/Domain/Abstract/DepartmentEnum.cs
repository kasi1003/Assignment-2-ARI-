using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PromotionsWebApp.Domain.Abstract
{
    public enum DepartmentEnum
    {
        [Description("Computing And Informatics")]
        ComputingAndInformatics = 1,
        Engineering,
        [Description("Health And Applied Services")]
        HealthAndAppliedServices,
        [Description("Human Sciences")]
        HumanScience,
        [Description("Harold Pupkewitz Graduat School of Business")]
        BusinessSchool,
        [Description("Management Sciences")]
        ManagementSciences,
        [Description("Natural Resources And Spatial Sciences")]
        NaturalResourcesAndSpatialSciences        
    }
}
