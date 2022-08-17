using System;
using NPoco;

namespace Visitor.Entities
{
    [TableName("REF_BRANCH")]
    public class BranchEntity
    {

        [Column("BRANCH_NUMBER")]
        public int Branch_Number { get; set; }

        [Column("BRANCH_NAME")]
        public string Branch_Name { get; set; }

        [Column("COMPANY_NUMBER")]
        public int Company_Number { get; set; }

        [Column("STOCK_COMPANY_NUMBER")]
        public int Stock_Company_Number { get; set; }

        [Column("AREA_CODE")]
        public string Area_Code { get; set; }

        [Column("BRANCH_ADDRESS_1")]
        public string Branch_Address_1 { get; set; }

        [Column("BRANCH_ADDRESS_2")]
        public string Branch_Address_2 { get; set; }

        [Column("BRANCH_ADDRESS_3")]
        public string Branch_Address_3 { get; set; }

        [Column("BRANCH_ADDRESS_4")]
        public string Branch_Address_4 { get; set; }

        [Column("BRANCH_STATUS")]
        public char Branch_Status { get; set; }

        [Column("EFFECTIVE_DATE")]
        public DateTime Effective_Date { get; set; }

        [Column("MANAGER_NAME")]
        public string Manager_Name { get; set; }

        [Column("TELEPHONE_NUMBER")]
        public string Telephone_Number { get; set; }

        [Column("NUMBER_OF_TILLS")]
        public int Number_Of_Tills { get; set; }

        [Column("EPOS_SOFTWARE_RELEASE")]
        public int Epos_Software_Release { get; set; }

        [Column("WAGE_COST")]
        public int Wage_Cost { get; set; }

        [Column("STAFF_DISCOUNT_PCENT")]
        public int Staff_Discount_Pcent { get; set; }

        [Column("COMMISSION_RATE")]
        public string Commission_Rate { get; set; }

        [Column("BRANCH_TYPE_ID")]
        public int Branch_Type_Id { get; set; }

        [Column("COUNTRY_CODE")]
        public string Country_Code { get; set; }

        [Column("MARKET_TYPE_ID")]
        public int Market_Type_Id { get; set; }

        [Column("NUMBER_OF_MOBILE_TILLS")]
        public int Number_Of_Mobile_Tills { get; set; }
    }
}