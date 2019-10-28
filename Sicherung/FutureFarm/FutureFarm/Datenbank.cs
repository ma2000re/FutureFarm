using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace FutureFarm
{
    class Datenbank
    {
        private OleDbConnection verbindung;
        private OleDbCommand cmd;
        private string cn;

        public Datenbank()
        {
            cn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=DatenbankFutureFarm.accdb";
            verbindung = new OleDbConnection(cn);
            verbindung.Open();
        }

        public OleDbDataReader Einlesen(string sql)
        {
            try
            {
                verbindung.Close();
                verbindung.Open();
                cmd = new OleDbCommand(sql, verbindung);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Einlesen:\n" + ex.Message);
            }
            //finally
            //{
            //    verbindung.Close();
            //}
        }

        public void Ausfuehren(string sql)
        {
            try
            {
                verbindung.Close();
                verbindung.Open();
                cmd = new OleDbCommand(sql, verbindung);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Ausführen" + ex.Message);
            }
            finally
            {
                verbindung.Close();
            }
        }

        public Int32 BerechnenInt(string sql)
        {
            try
            {
                verbindung.Close();
                verbindung.Open();
                cmd = new OleDbCommand(sql, verbindung);
                Int32 x = Convert.ToInt32(cmd.ExecuteScalar());
                return x;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Berechnen mit Int:\n " + ex.Message);
            }
            finally
            {
                verbindung.Close();
            }

        }

        public double berechnenDouble(string sql)
        {
            try
            {
                verbindung.Close();
                verbindung.Open();
                cmd = new OleDbCommand(sql, verbindung);
                double x = Convert.ToDouble(cmd.ExecuteScalar());
                return x;
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Berechnen für Double" + ex.Message);
            }
            finally
            {
                verbindung.Close();
            }

        }
    }
}
