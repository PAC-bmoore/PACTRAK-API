using PAC.Common.Sage;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Reflection;
using System.Text;

namespace PAC.DataLayer.Sage
{
    public class CustomerDL : BaseDL
    {
        #region GetCustomer
        public static Customer GetCustomer(string custNum)
        {
            Customer item = new Customer();
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "SELECT custno,company,contact,title,address1,address2,city,addrstate,zip,country,phone," +
                    "currency,faxno,email,addrstate,custstat,webpwd,id_col,username,webpwdhash " +
                    "FROM arcust01 WHERE custno = '" + custNum + "'";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                OleDbDataReader reader = cmd.ExecuteReader();

                if (reader != null)
                {
                    if (reader.Read())
                    {
                        item = new Customer();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            PropertyInfo prop = item.GetType().GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance);

                            if (prop != null)
                            {
                                if (reader.GetValue(i).GetType().Name == "String")
                                    prop.SetValue(item, reader.GetValue(i).ToString().Trim());
                                else
                                    prop.SetValue(item, reader.GetValue(i));
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return item;
        }
        #endregion

        #region UpdateCustomer
        public static bool UpdateCustomer(Customer cust)
        {
            bool retVal = true;
            OleDbConnection conn = null;

            try
            {
                conn = new OleDbConnection(SageConn);
                conn.Open();

                string query = "UPDATE arcust01 SET " +
                    "company = ?," +
                    "contact = ?," +
                    "title = ?," +
                    "address1 = ?," +
                    "address2 = ?," +
                    "city = ?," +
                    "zip = ?," +
                    "country = ?," +
                    "phone = ?," +
                    "faxno = ?," +
                    "addrstate = ?," +
                    "email = ?" +
                    "WHERE id_col = ?";

                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("company", cust.company == null ? "" : cust.company);
                cmd.Parameters.AddWithValue("contact", cust.contact == null ? "" : cust.contact);
                cmd.Parameters.AddWithValue("title", cust.title == null ? "" : cust.title);
                cmd.Parameters.AddWithValue("address1", cust.address1 == null ? "" : cust.address1);
                cmd.Parameters.AddWithValue("address2", cust.address2 == null ? "" : cust.address2);
                cmd.Parameters.AddWithValue("city", cust.city == null ? "" : cust.city);
                cmd.Parameters.AddWithValue("zip", cust.zip == null ? "" : cust.zip);
                cmd.Parameters.AddWithValue("country", cust.country == null ? "" : cust.country);
                cmd.Parameters.AddWithValue("phone", cust.phone == null ? "" : cust.phone);
                cmd.Parameters.AddWithValue("faxno", cust.faxno == null ? "" : cust.faxno);
                cmd.Parameters.AddWithValue("addrstate", cust.addrstate == null ? "" : cust.addrstate);
                cmd.Parameters.AddWithValue("email", cust.email == null ? "" : cust.email);
                cmd.Parameters.AddWithValue("id", cust.id_col);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                conn.Close();
            }

            return retVal;
        }
        #endregion
    }
}
