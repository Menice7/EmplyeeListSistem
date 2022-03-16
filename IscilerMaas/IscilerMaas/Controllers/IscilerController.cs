using IscilerMaas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IscilerMaas.Controllers
{
    public class IscilerController : Controller
    {
       
        string CONNECTIONSTRING = "Server = localhost; Database =CompanyDB; Integrated Security = True;";
        #region Login
        public int loginrole;
        public IActionResult Index()
        {

            return View();

        }
        public IActionResult Login(Isciler isciler)
        {
            List<Isciler> lisciler = LIscilerAllFields();
            int say = lisciler.Count();
        
            string str="";
    
            foreach (var i in lisciler)
            {
             
              
                if (isciler.IsciName == i.IsciName.Trim() & isciler.Sifre == i.Sifre.Trim())
                {
                    if (i.RoleId == 1)
                    {
                        str = "HomePage";
                        return View(str, lisciler);
                    }
                    else if (i.RoleId == 2)
                    {
                        str = "HomePageUser";
                        isciler.DepName = i.DepName;
                        return View(str, isciler);

                    }
                    else { RedirectToAction("Index"); }
                }
            }
           return View(str,lisciler);
        }

        public IActionResult Registration()
        {
            return View();
        }
        #endregion
        #region Maas
        public List<Odenis> LOdenis()
        {
            DataTable dataTableOdenis = new DataTable();
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = @"select i.IsciId, i.IsciName,il.Il,a.AyId,a.AyName,m.Miqdar from Maas m, 
Isciler i,Iller il,Ay a where i.IsciId=m.IsciId and il.IlId=m.IlId
and a.AyId=m.AyId order by il.IlId desc,a.AyId desc";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTableOdenis);
            con.Close();

            List<Odenis> odenisler = new List<Odenis>();
            odenisler = (from DataRow dr in dataTableOdenis.Rows
                         select new Odenis()
                         {
                             IsciId= Convert.ToInt32(dr["IsciId"]),
                             IsciName = dr["IsciName"].ToString(),
                             Il = Convert.ToInt32(dr["Il"]),
                             AyId= Convert.ToInt32(dr["AyId"]),
                             Ay = dr["AyName"].ToString(),
                             OdenisMiqdari = Convert.ToInt32(dr["Miqdar"])
                            
                         }).ToList();
            return odenisler;

        }
      
        public IActionResult Maas(int isciId)
        {
            List<Odenis> odenis = new List<Odenis>();
            List<Odenis> odenisler = LOdenis();
            foreach(var i in odenisler)
            {
                if (i.IsciId == isciId)
                {
                    odenis.Add(i);
                }
            }
            return View(odenis);
        }

        #endregion
        #region Odenis
        public IActionResult MaasOde(int isciId)
        {

            List<Isciler> isciler = LIscilerAllFields();
            Odenis odenis = new Odenis();
            List<Iller> iller = LIller();
            List<Aylar> aylar = LAylar();

            string str = isciler.FirstOrDefault(m => m.IsciId == isciId).IsciName;
            odenis.Aylar = aylar;
            odenis.Iller = iller;
            odenis.IsciName = str;
            return View(odenis);

        }
        public IActionResult AddOdenis(Odenis odenis)
        {
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = $"insert into Maas (IsciId,IlId,AyId,Miqdar) values ({odenis.IsciId},{odenis.IlId},{odenis.AyId},{odenis.OdenisMiqdari})";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("HomePage");
        }
        public IActionResult CariAyUcun(int isciId)
        {

            List<Isciler> isciler = LIscilerAllFields();
            Odenis odenis = new Odenis();

            string str = isciler.FirstOrDefault(m => m.IsciId == isciId).IsciName;
           
            odenis.IsciName = str;
            return View(odenis);

        }
        public IActionResult CariAyUcun1(Odenis odenis)
        {
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = $"insert into Maas (IsciId,IlId,AyId,Miqdar) values ({odenis.IsciId},{3},{DateTime.Now.Month},{odenis.OdenisMiqdari})";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("HomePage");
        }
        #endregion
        #region Isciler
        public List<Isciler> LIscilerForAdmin()
        {
            DataTable dataTableIsciler = new DataTable();
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = @"select i.IsciName,d.DepName from Isciler i, Department d
                             where i.DepId = d.DepId ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTableIsciler);
            con.Close();

            List<Isciler> isciler = new List<Isciler>();
            isciler = (from DataRow dr in dataTableIsciler.Rows
                       select new Isciler()
                       {

                           IsciName = dr["IsciName"].ToString(),
                           DepName = dr["DepName"].ToString()
                       }).ToList();
            return isciler;
        }

        public List<Isciler> LIscilerAllFields()
        {
            DataTable dataTableIsciler = new DataTable();
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = @"
select i.IsciId,i.IsciName,i.DepId,i.RoleId,i.Sifre, d.DepName from Isciler i, Department d
                             where i.DepId = d.DepId ";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTableIsciler);
            con.Close();

            List<Isciler> isciler = new List<Isciler>();
            isciler = (from DataRow dr in dataTableIsciler.Rows
                       select new Isciler()
                       {
                           IsciId = Convert.ToInt32(dr["IsciId"]),
                           IsciName = dr["IsciName"].ToString(),
                           DepId = Convert.ToInt32(dr["DepId"]),
                           RoleId = Convert.ToInt32(dr["RoleId"]),
                           Sifre = dr["Sifre"].ToString(),
                           DepName = dr["DepName"].ToString(),
                           TrueFalse = true
                       }).ToList();
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            
            List<Odenis> odenisler = LOdenis();
            foreach (var it in odenisler)
            {
                if (it.AyId == month & it.Il == year)
                {
                    
                    foreach (var i in isciler)
                    {
                        
                        if (it.IsciId == i.IsciId)
                        {
                            i.TrueFalse = false;
                        }

                       
                    }
                }

            }
            Console.WriteLine(DateTime.Now);
            return isciler;
        }

        public List<Department> LDepForAdmin()
        {
            DataTable dataTableDep = new DataTable();
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = @"select * from Department";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTableDep);
            con.Close();

            List<Department> departments = new List<Department>();
            departments = (from DataRow dr in dataTableDep.Rows
                           select new Department()
                           {
                               DepId = Convert.ToInt32(dr["DepId"]),
                               DepName = dr["DepName"].ToString(),

                           }).ToList();
            return departments;
        }
        public List<Role> LRoleForAdmin()
        {
            DataTable dataTableRole = new DataTable();
            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = @"select * from [Role]";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dataTableRole);
            con.Close();

            List<Role> roles = new List<Role>();
            roles = (from DataRow dr in dataTableRole.Rows
                     select new Role()
                     {
                         RoleId = Convert.ToInt32(dr["RoleId"]),
                         RoleName = dr["RoleName"].ToString(),

                     }).ToList();
            return roles;
        }


        public IActionResult HomePage()
        {
           
            List<Isciler> isciler = LIscilerAllFields();
           
          
          
            return View(isciler);

        }
        public IActionResult HomePageUser()
        {
            List<Isciler> lisciler = LIscilerAllFields();
            return View(lisciler);
        }
        public IActionResult CreateIsci(int? isciId)
        {
            List<Isciler> isciler = LIscilerAllFields();
            #region get
            List<Role> roles = LRoleForAdmin();
            List<Department> departments = LDepForAdmin();
            #endregion
            if (isciId == null)
            {
                Isciler isci = new Isciler();
                isci.Departments = departments;
                isci.Roles = roles;
                return View(isci);
            }

            else
            {
                Isciler isci = isciler.FirstOrDefault(m => m.IsciId == isciId);
                isci.Departments = departments;
                isci.Roles = roles;
                return View(isci);
            }
        }

        public IActionResult AddOrUpdateIsci(Isciler isciler)
        {

            if (ModelState.IsValid)
            {
                if (isciler.IsciId == null)
                {

                    SqlConnection con = new SqlConnection(CONNECTIONSTRING);
                    con.Open();
                    string query = $"insert into Isciler values ('{isciler.IsciName}',{isciler.DepId},{isciler.RoleId}," +
                        $"'{isciler.Sifre}')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {

                    SqlConnection con = new SqlConnection(CONNECTIONSTRING);
                    con.Open();
                    string query = $"update Isciler set IsciName='{isciler.IsciName}',DepId = {isciler.DepId}," +
                        $"RoleId = {isciler.RoleId}, Sifre={isciler.Sifre}" +
                        $" where IsciId = {isciler.IsciId}";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                return RedirectToAction("HomePage");
            }
            else
            {
                return View("CreateIsci", isciler);
            }



        }

        public IActionResult DeleteIsci(int isciId)
        {


            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = $"delete from Isciler where IsciId={isciId}";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            return RedirectToAction("HomePage");
        }

        #endregion
        #region Export

        public ActionResult Export()
        {
            List<Isciler> lisciler = LIscilerAllFields();
            return View(lisciler);
        }

        public ActionResult GetData()
        {
            List<Isciler> isciler = LIscilerAllFields();
            return View(isciler);
        }

        #endregion
        #region Hesabat

        public ActionResult Hesabat()
        {
           
            List<Odenis> maas = LOdenis();
            return View(maas);
        }

        public ActionResult GetHesabat()
        {
            List<Isciler> isciler = LIscilerAllFields();
            return View(isciler);
        }

        #endregion
        
        #region Sobeler
        public IActionResult Sobeler()
        {
            List<Department> LDep = LDepForAdmin();
            return View(LDep);
        }
        public IActionResult CreateSobe(int? depId)
        {
            
            List<Department> departments = LDepForAdmin();
       
            if (depId == null)
            {
               
                
                return View();
            }

            else
            {
                Department dep = departments.FirstOrDefault(m => m.DepId == depId);
               
                return View(dep);
            }
        }

        public IActionResult AddOrUpdateDep(Department dep)
        {

            if (ModelState.IsValid)
            {
                if (dep.DepId == null)
                {

                    SqlConnection con = new SqlConnection(CONNECTIONSTRING);
                    con.Open();
                    string query = $"insert into Department(DepName) values ('{dep.DepName}')";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {

                    SqlConnection con = new SqlConnection(CONNECTIONSTRING);
                    con.Open();
                    string query = $"update Department set DepName='{dep.DepName}'  where depId={dep.DepId}";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    con.Close();

                }
                return RedirectToAction("Sobeler");
            }
            else
            {
                return View("CreateSobe", dep);
            }



        }

        public IActionResult DeleteDep(int depId)
        {


            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = $"delete from Department where DepId={depId}";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
            return RedirectToAction("HomePage");
        }

        #endregion
        #region Il
        public DataTable GetIller()
        {

            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = "select * from Iller";
            DataTable dataTableIl = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTableIl);
            con.Close();
            return dataTableIl;

        }
        public List<Iller> LIller()
        {
            DataTable dataTable = GetIller();
            List<Iller> iller = new List<Iller>();
            iller = (from DataRow dr in dataTable.Rows
                     select new Iller()
                     {
                         IlId = Convert.ToInt32(dr["IlId"]),
                         Il = Convert.ToInt32(dr["Il"])
                     }).ToList();
            return iller;
        }
        #endregion
        #region Ay
        public DataTable GetAy()
        {

            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = "select * from Ay";
            DataTable dataTableAy = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTableAy);
            con.Close();
            return dataTableAy;

        }

        public List<Aylar> LAylar()
        {
            DataTable dataTable = GetAy();
            List<Aylar> aylar = new List<Aylar>();
            aylar = (from DataRow dr in dataTable.Rows
                     select new Aylar()
                     {
                         AyId = Convert.ToInt32(dr["AyId"]),
                         Ay = dr["AyName"].ToString()
                     }).ToList();
            return aylar;
        }

        #endregion
        #region Roles

        public DataTable GetRole()
        {

            SqlConnection con = new SqlConnection(CONNECTIONSTRING);
            con.Open();
            string query = "select * from [Role]";
            DataTable dataTableRole = new DataTable();
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTableRole);
            con.Close();
            return dataTableRole;

        }
        public List<Role> LRole()
        {
            DataTable dataTable = GetRole();
            List<Role> roles = new List<Role>();
            roles = (from DataRow dr in dataTable.Rows
                      select new Role()
                      {
                          RoleId = Convert.ToInt32(dr["RoleId"]),
                          RoleName = dr["RoleName"].ToString()
                      }).ToList();
            return roles;
        }
        //public IActionResult HomePage(int roleId)
        //{

        //    bool bl = CheckRole(roleId);
        //    if (bl == true)
        //    {
        //        return View();
        //    }
        //    return View();
        //}
        public bool CheckRole(int roleId)
        {
            if(roleId == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public IActionResult Roles()
        {
            List<Role> roles = LRole();
            return View(roles);
        }
       
        #endregion
    }
}
