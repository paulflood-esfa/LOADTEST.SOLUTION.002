using Microsoft.VisualStudio.TestTools.WebTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DAS.QA.LOADTEST.PLUGINS
{
    [System.ComponentModel.DisplayName("Insert Unique Learner Number")]
    [System.ComponentModel.Description("Inserts a Unique Learner Number to DB with every iteration")]
    public class InsertULN : WebTestRequestPlugin
        {
            [DisplayName("Lastname Context Parameter")]
            [Description("This is the Lastname which will be used to search for an Apprentice and insert a Unique Learner Number in the database")]
            public string LastnameSearch { get; set; }

            //Get the Lastname Context Parameter Value

            [DisplayName("ULN Context Parameter")]
            [Description("Unique Learner Number to be inserted for the Apprentice")]
            public string ULNInsert { get; set; }


            [DisplayName("Database Connection Test Data File")]
            [Description("The Test Data File contains the Database connection Values to run SQL Updates. File Format: DataSource.Filename#csv.Column Heading")]
            public string DBConnectionDataFile { get; set; }


            public void DBUpdate(string LastnameSearch, string ULNInsert, string dbConnetionString)
            {

                SqlConnection connection;
                SqlDataAdapter adapter = new SqlDataAdapter();
                connection = new SqlConnection(dbConnetionString);

                string strSQL = "UPDATE [dbo].[Apprenticeship] SET ULN = '" + ULNInsert + "' WHERE lastname = '" + LastnameSearch + "'";
                try
                {
                    connection.Open();
                    adapter.InsertCommand = new SqlCommand(strSQL, connection);
                    adapter.InsertCommand.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                adapter.Dispose();
            }



            public override void PostRequest(object sender, PostRequestEventArgs e)
            {

                object datafileDBConnectionString;

                string DBConnectionValue = null;

                object LastnameSearchString;

                string LastnameSearchValue = null;

                object ULNInsertString;

                string ULNInsertValue = null;

                //Get DB Connection values from Test Data File
                if (e.WebTest.Context.TryGetValue(DBConnectionDataFile, out datafileDBConnectionString))
                {
                    DBConnectionValue = datafileDBConnectionString.ToString();

                    //e.WebTest.AddCommentToResult("DB Connection = " + DBConnectionValue);
                    //e.WebTest.AddCommentToResult("LastnameUpdate = " + e.WebTest.Context[LastnameSearch]);
                    //e.WebTest.AddCommentToResult("ULNInsert = " + e.WebTest.Context[ULNInsert]);

                    var webcontextparameters = e.WebTest.Context;
                    webcontextparameters.Add("dbconnectionstring", DBConnectionValue);
                }
                else
                {
                    throw new WebTestException(DBConnectionDataFile + "' - Test Data File' not found");
                }


                // Get LastnameSearch value
                if (e.WebTest.Context.TryGetValue(LastnameSearch, out LastnameSearchString))
                {
                    LastnameSearchValue = LastnameSearchString.ToString();

                    //e.WebTest.AddCommentToResult("Lastname Context Parameter = " + LastnameSearchValue);
                }
                else
                {
                    throw new WebTestException(LastnameSearch + " not found");
                }


                // Get ULNInsert Value
                if (e.WebTest.Context.TryGetValue(ULNInsert, out ULNInsertString))
                {
                    ULNInsertValue = ULNInsertString.ToString();

                    //e.WebTest.AddCommentToResult("ULN Context Parameter = " + ULNInsertValue);
                }
                else
                {
                    throw new WebTestException(ULNInsert + " not found");
                }

                //Insert ULN into database
                DBUpdate(LastnameSearchValue, ULNInsertValue, DBConnectionValue);

            }
        }

    [System.ComponentModel.DisplayName("Generate Unique First Name")]
    [System.ComponentModel.Description("Generates a unique first name with every iteration")]
    public class GenerateUniqueFirstName : WebTestPlugin
    {
        [System.ComponentModel.DisplayName("Target Context Parameter Name")]
        [System.ComponentModel.Description("Name of the context parameter that will receive the generated value.")]

        public string ContextParamTarget { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            // Generate new guid with specified output format
            string firstName = string.Empty;
            firstName = "First_" + System.DateTime.Now.ToString("ddMM") + System.DateTime.Now.ToString("HHmmss") + "_" + e.WebTest.Context.WebTestUserId.ToString() + "_" + e.WebTest.Context.WebTestIteration.ToString();

            // Set the context paramaeter with generated lastname
            e.WebTest.Context[ContextParamTarget] = firstName;
            base.PreWebTest(sender, e);
        }
    }

    [System.ComponentModel.DisplayName("Generate Unique Last Name")]
    [System.ComponentModel.Description("Generates a unique last name with every iteration")]
    public class GenerateUniqueLastName : WebTestPlugin
    {
        [System.ComponentModel.DisplayName("Target Context Parameter Name")]
        [System.ComponentModel.Description("Name of the context parameter that will receive the generated value.")]
        public string ContextParamTarget { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            // Generate new guid with specified output format
            string lastName = string.Empty;

            lastName = "Last_" + System.DateTime.Now.ToString("ddMM") + System.DateTime.Now.ToString("HHmmss") + "_" + e.WebTest.Context.WebTestUserId.ToString() + "_" + e.WebTest.Context.WebTestIteration.ToString();

            // Set the context paramaeter with generated lastname
            e.WebTest.Context[ContextParamTarget] = lastName;

            base.PreWebTest(sender, e);
        }
    }

    [System.ComponentModel.DisplayName("Generate Unique Email")]
    [System.ComponentModel.Description("Generates a unique for the new user")]
    public class GenerateUniqueEmail : WebTestPlugin
    {
        [System.ComponentModel.DisplayName("Target Context Parameter Name")]
        [System.ComponentModel.Description("Name of the context parameter that will receive the generated value.")]
        public string ContextParamTarget { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            // Generate new guid with specified output format
            string eMail = string.Empty;

            eMail = "First_" + System.DateTime.Now.ToString("ddMM") + System.DateTime.Now.ToString("HHmmss") + "_" + e.WebTest.Context.WebTestUserId.ToString() + "_" + e.WebTest.Context.WebTestIteration.ToString()
            + "@mailinator.com";

            // Set the context paramaeter with generated lastname
            e.WebTest.Context[ContextParamTarget] = eMail;

            base.PreWebTest(sender, e);
        }
    }

    [System.ComponentModel.DisplayName("Generate Unique Learner Number")]
    [System.ComponentModel.Description("Generates a unique for the new user with every iteration")]
    public class GenerateUniqueLearnerNumber : WebTestPlugin
    {
        [System.ComponentModel.DisplayName("Target Context Parameter Name")]
        [System.ComponentModel.Description("Name of the context parameter that will receive the generated value.")]
        public string ContextParamTarget { get; set; }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {
            String ULRN = GenerateRandomNumberBetweenTwoValues(10, 99).ToString() + DateTime.Now.ToString("ssffffff");

            for (int i = 1; i < 30; i++)
            {
                if (IsValidCheckSum(ULRN))
                {
                    break;
                }
                else
                {
                    ULRN = (long.Parse(ULRN) + 1).ToString();
                }
            }
            e.WebTest.Context[ContextParamTarget] = ULRN;
            base.PreWebTest(sender, e);
        }

        private static bool IsValidCheckSum(string uln)
        {
            var ulnCheckArray = uln.ToCharArray()
                                .Select(c => long.Parse(c.ToString()))
                                    .ToList();

            var multiplier = 10;
            long checkSumValue = 0;
            for (var i = 0; i < 10; i++)
            {
                checkSumValue += ulnCheckArray[i] * multiplier;
                multiplier--;
            }

            return checkSumValue % 11 == 10;
        }

        public static int GenerateRandomNumberBetweenTwoValues(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min, max);
        }
    }

    [System.ComponentModel.DisplayName("Update PAYE Removed Date")]
    [System.ComponentModel.Description("This plugin updates the PAYE Scheme Removed Date in the Database from NULL to Current DateTime as a Test Data Prerequisite for the Test Script to run")]
    public class UpdatePayeRemovedDate : WebTestPlugin
    {
        [DisplayName("PAYE Scheme Test Data File")]
        [Description("The Test Data File contains the PAYE Scheme Reference Values to update. File Format: DataSource.Filename#csv.Column Heading")]
        public string PayeSchemeDataFile { get; set; }

        [DisplayName("Database Connection Test Data File")]
        [Description("The Test Data File contains the Database connection Values to run SQL Updates. File Format: DataSource.Filename#csv.Column Heading")]
        public string DBConnectionDataFile { get; set; }

        public void DBReset(string PayeSchemeReference, string dbConnetionString)
        {

            SqlConnection connection;
            SqlDataAdapter adapter = new SqlDataAdapter();
            connection = new SqlConnection(dbConnetionString);

            string strSQL = "UPDATE employer_account.accounthistory SET removedDate = GetDATE() WHERE payeref = '" + PayeSchemeReference + "' and removeddate is null";
            try
            {
                connection.Open();
                adapter.InsertCommand = new SqlCommand(strSQL, connection);
                adapter.InsertCommand.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            adapter.Dispose();
        }

        public override void PreWebTest(object sender, PreWebTestEventArgs e)
        {

            //DB Connection values from Test Data File
            object datafileDBConnectionString;

            string DBConnectionValue = null;


            // Get DB Connection values from Test Data File
            if (e.WebTest.Context.TryGetValue(DBConnectionDataFile, out datafileDBConnectionString))
            {


                DBConnectionValue = datafileDBConnectionString.ToString();
                e.WebTest.AddCommentToResult("DB Connection = " + DBConnectionValue);

                // Debug
                //var webContextParameters = e.WebTest.Context;
                //webContextParameters.Add("DBConnectionString", DBConnectionValue);
                // Debug
            }
            else
            {
                throw new WebTestException(PayeSchemeDataFile + "' - Test Data File' not found");
            }

            // PAYE Scheme values from Test Data File
            object dataFilePayeSchemeValue;

            // Get PAYE Scheme values from Test Data File
            if (e.WebTest.Context.TryGetValue(PayeSchemeDataFile, out dataFilePayeSchemeValue))
            {
                string PayeSchemeReference = dataFilePayeSchemeValue.ToString();

                // Debug
                //string PayeSchemeValue = dataFilePayeSchemeValue.ToString();
                //e.WebTest.AddCommentToResult("PAYE Scheme = " + PayeSchemeValue);
                // Debug

                DBReset(PayeSchemeReference, DBConnectionValue);
            }
            else
            {
                throw new WebTestException(PayeSchemeDataFile + "' - Test Data File' not found");
            }



        }


        //public override void PostWebTest(object sender, PostWebTestEventArgs e)
        //{
        //    //Add code to execute after the test
        //}

        //public override void PreRequest(object sender, PreRequestEventArgs e)
        //{
        //    // Add code before the request
        //}
    }
}

