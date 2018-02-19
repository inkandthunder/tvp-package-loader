using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace package_loader
{

    class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                log.Info("No data file passed. Exiting Application.");
            }
            else
            {
                string xmlfile = args[0];	
                string extension = xmlfile.Substring(xmlfile.LastIndexOf('.') + 1);
                if (extension.ToLower() == "xml")
                {
                    log.Info("Begin processing on " + args[0]);
                    Job Job = DeserializeXMLFileToObject<Job>(xmlfile);
                    DataLoader(Job);
                }
                else
                {
                    log.Debug("Invalid filetype. Please pass an XML file");
                }
                Console.ReadKey();
            }
        }

        public static Job DeserializeXMLFileToObject<Job>(string path)
        {
            Job returnObject = default(Job);
            if (string.IsNullOrEmpty(path)) return default(Job);

            try
            {
                StreamReader xmlStream = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(Job));
                returnObject = (Job)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                log.Error("XML Deserialization failed", ex);
            }
            return returnObject;
        }

        static void DataLoader(Job job)
        {
            string cs = ReadSetting("cs");

            DataTable tvp_packageattributes = CreateTable();

            // Add New Rows to table
            try
            {
                for (var i = 0; i < job.Packages.Count; i++)
                {
                    tvp_packageattributes.Rows.Add(
                        int.Parse(job.Packages[i].NSLID),
                        int.Parse(job.Packages[i].Sequence),
                        int.Parse(job.Packages[i].LogicalPage),
                        int.Parse(job.Packages[i].DivertCode),
                        int.Parse(job.Packages[i].EnclosureCode)
                        );
                }
            }
            catch (Exception ex)
            {
                log.Error("Error parsing XML data", ex);
            }

            try
            {
                using (SqlConnection db = new SqlConnection(cs))
                {
                    db.Open();
                    SqlCommand cmd = new SqlCommand("dbo.usp_PackageAttributeUpdate", db);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Pass tbv to stored procedure
                    cmd.Parameters.Add("@updatedPacks", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlParameter sqlParam = cmd.Parameters.AddWithValue("@packages", tvp_packageattributes);
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    cmd.ExecuteNonQuery();
                    db.Close();
                    log.Info("Data Save Successfully.");
                }
            }
            catch (SqlException se)
            {
                log.Error("eFulfillment Database load failed", se);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("nslid", typeof(Int32));
            dt.Columns.Add("SequenceNumber", typeof(Int32));
            dt.Columns.Add("LogicalPage", typeof(Int32));
            dt.Columns.Add("DivertCode", typeof(Int32));
            dt.Columns.Add("EnclosureCode", typeof(Int32));
            return dt;
        }

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                return appSettings[key] ?? "Not Found";
            }
            catch (ConfigurationErrorsException ex)
            {
                log.Error("Error reading the app configuration", ex);
                return null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }



    }
}
