using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeTable.Masters
{
    public class MasterModel
    {
        private BD db;
        public string FIO;
        private int id;
        private List<string> relatedClasses;

        public MasterModel( BD dataBase )
        {
            this.db = dataBase;
        }

        public List<string> GetRelatedClassesByMasteName(string masterName)
        {
            relatedClasses = BD.getListFromBD("SELECT Class.Name " +
            " FROM Class INNER JOIN(master INNER JOIN master_skills ON master.id = master_skills.id_master) ON Class.Код = master_skills.id_class" +
            " WHERE(((master.FIO) = \"" + masterName + "\"));");
            return relatedClasses;
             
        }

        public void AddClassToMaster(string masterName, string className)
        {            
            string idMaster = db.SelectFirstQery("SELECT master.id FROM master Where (master.FIO = \"" + masterName + "\")");
            string idClass = db.SelectFirstQery("SELECT Class.Код FROM Class Where (Class.Name = \"" + className + "\")");
            BD.AddQuery("INSERT INTO master_skills VALUES (" + idMaster + ", " + idClass + ")");
        }
        public void RemoveClassFromMaster(string masterName, string className)
        {
            string idMaster = db.SelectFirstQery("SELECT master.id FROM master Where (master.FIO = \"" + masterName + "\")");
            string idClass = db.SelectFirstQery("SELECT Class.Код FROM Class Where (Class.Name = \"" + className + "\")");
            BD.RemoveQuery("DELETE master_skills.id_master, master_skills.id_class FROM master_skills WHERE ((id_master = " + idMaster + ") and (id_class = " + idClass + "));");
        }

        public List<string> Masters
        {
            get
            {
                return BD.getListFromBD("SELECT master.FIO FROM master;");                
            }
        }
        public List<string> Classes
        {
            get
            {
                return BD.getListFromBD("SELECT Class.Name FROM Class;");
            }
        }        
    }
}
