using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Collections;
using System.Data.SqlClient;

namespace EMR_LISTENER
{
    public partial class Form1 : Form
    {
        int i_time = 60;
        OrclDbConn orclIcareConn = null;
        OracleDataAdapter oracleDataAdapter = null;
        SqlDataAdapter sqlDataAdapter = null;
        SqlDataReader sqlDataReader = null;
        DataSet dataSet = null;
        String Strsql = "";
        ArrayList al_sql = null;
        ArrayList al_orc = null;
        DbConn dbConn = null;
        ArrayList alist;
        ArrayList al_jsbr;
        int int_maxTimes =0;
        public Form1()
        {
            InitializeComponent();
            orclIcareConn = new OrclDbConn();
            dbConn = new DbConn();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            button1.Text = i_time--.ToString() ;
            if (i_time < 1)
            {
                button1_Click(sender, e);
                i_time = 60;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = i_time.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            al_sql = new ArrayList();
            al_orc = new ArrayList();
            alist = OrclDbConn.arraylist;
            al_jsbr = new ArrayList();
            //查询新入院的病人插入到tblpatient_base
            Strsql = "SELECT a.inpatientid_chr as case_no, a.inpatientid_chr as case_no,a.inpatientcount_int as admiss_times,1 as mz_times,b.lastname_vchr as name,b.birth_dat as birth_date,b.sex_chr as sex,"
                + " b.race_vchr as nation_code ,b.bloodtype_chr as blood_type,b.idcard_chr as social_no,b.birthplace_vchr as birth_place,b.nationality_vchr as country,'' as d_code,'' delete_flag,null as other1,"
                + " null as other1,null as other2,null as other3,null as other4,null as other5,null as other6,null as other7,null as other8,null as other9,null as other10,null as other11,null as other12,null as other13,"
                + " null as other14,null as other15,null as other16,null as other17,null as other18,null as other19 "
                + " FROM t_opr_bih_register_new A,t_opr_bih_registerdetail b "
                + "  where a.registerid_chr=b.registerid_chr and a.isPost=0";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table1");
            for (int i = 0; i < dataSet.Tables["table1"].Rows.Count; i++)
            {
                Strsql = "insert into tblpatient_base values('";
                for (int j = 0; j < dataSet.Tables["table1"].Columns.Count - 1; j++)
                {
                    Strsql = Strsql + dataSet.Tables["table1"].Rows[i].ItemArray[j].ToString() + "','";
                }
                Strsql = Strsql + dataSet.Tables["table1"].Rows[i].ItemArray[dataSet.Tables["table1"].Columns.Count - 1].ToString() + "')";
                al_sql.Add(Strsql);
            }
            //查询入院的病人把数据插入到tblzy_actpatient1
            Strsql = "select substr(a.registerid_chr,2) as patient_id,a.inpatientid_chr as case_no,a.inpatientcount_int as admiss_times,b.lastname_vchr as n,"
                + "b.sex_chr as sex,'' as charge_type,(select i.empno_chr from t_bse_employee i where A.MZDOCTOR_CHR=i.empid_chr AND 1=2) AS refer_physician,"
                + "'' AS director_physician,(select i.empno_chr from t_bse_employee i where  A.CASEDOCTOR_CHR=i.empid_chr) AS consult_physician,"
                + "A.INPATIENT_DAT as admiss_date,(select j.code_vchr from t_bse_deptdesc j where j.deptid_chr=a.deptid_chr) as admiss_dept_code,"
                + "(select j.code_vchr from t_bse_deptdesc j where j.deptid_chr=a.deptid_chr) as admiss_ward_code,f.bed_no as bed_no,"
                + "'' as dis_date,0 as dis_status,null as dis_dept_code,null as dis_ward_code,g.paytypename_vchr as response_type,"
                + "null as max_ledger_sn,null as balance,null as total_charge,0 as infant_flag,null as max_depo_times,null as bed_charge_date,"
                + "null as owe_limit,null as hic_no,null as other_hicno,null as total_insu_acc,NULL AS op_id_code,7 as grade_code,"
                + "0 as delete_flag,null as age,'' as age_unit,null as ht_jzh,'' as owner_temp,0 as case_flag,null as inpatientid,null as indays,null as admiss_code,0 "
                + " from  T_OPR_BIH_REGISTER_new A left join T_OPR_BIH_REGISTERDETAIL B on a.registerid_chr=b.registerid_chr "
                + " left join t_bse_patientpaytype C on a.paytypeid_chr=c.paytypeid_chr "
                + " left join t_bse_deptdesc E on a.deptid_chr=e.deptid_chr "
                + " left join t_bse_bed f on a.bedid_chr=f.bedid_chr and a.areaid_chr=f.areaid_chr"
                + " left join t_bse_patientpaytype g on a.paytypeid_chr=g.paytypeid_chr "
                + " where A.isPost=0";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table2");
            for (int i = 0; i < dataSet.Tables["table2"].Rows.Count; i++)
            {
                Strsql = "insert into tblzy_actpatient1 values(";
                for (int j = 0; j < dataSet.Tables["table2"].Columns.Count - 1; j++)
                {
                    if (dataSet.Tables["table2"].Rows[i].ItemArray[j].ToString() != "")
                    {
                        Strsql = Strsql + "'" + dataSet.Tables["table2"].Rows[i].ItemArray[j].ToString() + "',";
                    }
                    else
                    {

                        Strsql = Strsql + "null,";
                    }

                }
                Strsql = Strsql + "'" + dataSet.Tables["table2"].Rows[i].ItemArray[dataSet.Tables["table2"].Columns.Count - 1].ToString() + "')";
                al_sql.Add(Strsql);
            }

           
            for (int i = 0; i < dataSet.Tables["table2"].Rows.Count; i++)
            {
                Strsql = "insert into dr_emr_headpage(patient_id) values('"+dataSet.Tables["table2"].Rows[i].ItemArray[0].ToString()+"')";
                al_sql.Add(Strsql);

            }
            //查询新入院的病人把数据插入tblzy_actpatient2
            Strsql = "select substr(a.registerid_chr,2) as patient_id,"
                +"null,null,null,null,null,null,null,null,null,null,null,null,null,"
                +"null,null,null,null,null,null,null,"
                +"null,null,null,null,null,null,null,null,null,null,null,null"
                + ",(select  i.empno_chr  from t_bse_employee i where a.mzdoctor_chr=i.empid_chr) ,"
                +"  (select  i.empno_chr  from t_bse_employee i where  a.casedoctor_chr=i.empid_chr),"
                + "0,null,null,null,null,null,null,null,null,null,null,null,null,null,null,"
                +"null,null,null,null,null,null,null,null,null,null"
                + "  from t_opr_bih_register_new a where a.isPost=0";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table3");
            for (int i = 0; i < dataSet.Tables["table3"].Rows.Count; i++)
            {
                Strsql = "insert into tblzy_actpatient2 values(";
                for (int j = 0; j < dataSet.Tables["table3"].Columns.Count - 1; j++)
                {
                    if (dataSet.Tables["table3"].Rows[i].ItemArray[j].ToString() != "")
                    {
                        Strsql = Strsql + "'" + dataSet.Tables["table3"].Rows[i].ItemArray[j].ToString() + "',";
                    }
                    else
                    {

                        Strsql = Strsql + "null,";
                    }

                }
                Strsql = Strsql + "'" + dataSet.Tables["table3"].Rows[i].ItemArray[dataSet.Tables["table3"].Columns.Count - 1].ToString() + "')";
                al_sql.Add(Strsql);
            }
            //查询注销的病人删除tblpatient_base,tblzy_actpatient1,dr_emr_headpage
            Strsql = "select substr(registerid_chr,2) from t_opr_bih_register_zhuxiao where isPost=0";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table4");
            for (int i = 0; i < dataSet.Tables["table4"].Rows.Count; i++)
            {
                Strsql = "delete from tblpatient_base  where case_no =(select case_no from tblzy_actpatient1 b where b.patient_id= ";
                for (int j = 0; j < dataSet.Tables["table4"].Columns.Count - 1; j++)
                {

                    Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[j].ToString() + "'";


                }
                Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[0].ToString() + "')";
                al_sql.Add(Strsql);
            }
            for (int i = 0; i < dataSet.Tables["table4"].Rows.Count; i++)
            {
                Strsql = "delete from tblzy_actpatient1 where patient_id =";
                for (int j = 0; j < dataSet.Tables["table4"].Columns.Count - 1; j++)
                {

                    Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[j].ToString() + "'";


                }
                Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[0].ToString() + "'";
                al_sql.Add(Strsql);
            }
            for (int i = 0; i < dataSet.Tables["table4"].Rows.Count; i++)
            {
                Strsql = "delete from dr_emr_headpage where patient_id =";
                for (int j = 0; j < dataSet.Tables["table4"].Columns.Count - 1; j++)
                {

                    Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[j].ToString() + "'";


                }
                Strsql = Strsql + "'" + dataSet.Tables["table4"].Rows[i].ItemArray[0].ToString() + "'";
                al_sql.Add(Strsql);
            }

            //查询出院的病人改变tblzy_actpatient1的状态
            Strsql = "select substr(a.registerid_chr,2),b.outhospital_dat,(select j.code_vchr from t_bse_deptdesc j where j.deptid_chr=b.OUTDEPTID_CHR) as OUTDEPTID_CHR,"
                + "(select j.code_vchr from t_bse_deptdesc j where j.deptid_chr=b.OUTDEPTID_CHR) as OUTDEPTID_CHR"
                + " from t_opr_bih_register_chuyuan a,t_opr_bih_leave b"
                + " where a.registerid_chr=b.registerid_chr and a.ispost=0 ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table5");
            for (int i = 0; i < dataSet.Tables["table5"].Rows.Count; i++)
            {
                Strsql = "update  tblzy_actpatient1 set dis_status  ='1',dis_date=";
                Strsql = Strsql + "'" + dataSet.Tables["table5"].Rows[i].ItemArray[1].ToString() + "'";
                Strsql = Strsql + ",dis_dept_code='" + dataSet.Tables["table5"].Rows[i].ItemArray[2].ToString() + "'";
                Strsql = Strsql + ",dis_ward_code='" + dataSet.Tables["table5"].Rows[i].ItemArray[3].ToString() + "'";
                Strsql = Strsql + " where patient_id='" + dataSet.Tables["table5"].Rows[i].ItemArray[0].ToString() + "' ";
                al_sql.Add(Strsql);
            }
            //查询取消出院病人
            Strsql = "select substr(a.registerid_chr,2) from t_opr_bih_register_qxcy a"
                + " where a.ispost=0 ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table5");
            for (int i = 0; i < dataSet.Tables["table5"].Rows.Count; i++)
            {
                Strsql = "update  tblzy_actpatient1 set dis_status  ='0',dis_date=null,dis_dept_code=null,dis_ward_code=null"
                + " where patient_id='" + dataSet.Tables["table5"].Rows[i].ItemArray[0].ToString() + "' ";
                al_sql.Add(Strsql);
            }
            Strsql = "delete from table_bafy";

            orclIcareConn.GetSqlCmd(Strsql);
            //查询费用插到一张临时表中

            Strsql = "insert into table_bafy select 住院号 as zyh,patientid_chr ,入院日期 as ryrq,出院日期 as cyrq,sum(总费用) as zfy,sum(自付金额) as zfje,";
            for (int ii = 0; ii < alist.Count - 1; ii++)
            {
                Strsql = Strsql + "max(c" + ii.ToString() + ") as c" + ii.ToString() + ",";
            }
            Strsql = Strsql + "max(c" + (alist.Count - 1).ToString() + ") as c" + (alist.Count - 1).ToString() + " from ";
            Strsql = Strsql + "(select a.inpatientid_chr as 住院号,substr(a.registerid_chr,2) as patientid_chr,a.inpatient_dat as 入院日期,d.outhospital_dat as 出院日期,case when b.totalsum_mny is null then sum(c.totalmoney_dec) else  b.totalsum_mny end  as 总费用, "
                   + " case when b.sbsum_mny is null then sum(c.totalmoney_dec) else  b.sbsum_mny  end as 自付金额,";

            for (int ii = 0; ii < alist.Count - 1; ii++)
            {
                Strsql = Strsql + "sum(case when (c.invcateid_chr in (" + getTypeString(alist[ii].ToString()) + ") ) then c.totalmoney_dec else 0 end) as c" + ii.ToString() + ", ";
            }
            Strsql = Strsql + "sum(case when (c.invcateid_chr in (" + getTypeString(alist[alist.Count - 1].ToString()) + ") ) then c.totalmoney_dec else 0 end) as c" + (alist.Count - 1).ToString();

            Strsql = Strsql + " from t_opr_bih_charge b,t_opr_bih_patientcharge c,t_opr_bih_register a "
                //     + " left join t_opr_bih_patientcharge c on c.registerid_chr=a.registerid_chr "
                + " left join t_opr_bih_leave d on d.registerid_chr=a.registerid_chr "
                + " left join t_opr_bih_charge_new e on a.registerid_chr=e.registerid_chr"
                + " where  a.pstatus_int <> 0  and a.status_int = 1 and a.feestatus_int=3 and d.pstatus_int=1 and d.status_int=1 and a.registerid_chr=b.registerid_chr "
                + " and b.chargeno_chr=c.paymoneyid_chr"
                + " and e.ispass_int=0 and e.type_int=1"
                // + " group by substr(a.registerid_chr,2) ,b.totalsum_mny,b.sbsum_mny,a.inpatient_dat,d.outhospital_dat order by a.inpatientid_chr";
                + " group by b.totalsum_mny, b.sbsum_mny,a.inpatientid_chr ,a.inpatient_dat,d.outhospital_dat,substr(a.registerid_chr,2)   order by a.inpatientid_chr)"
                + " group by 住院号,入院日期,出院日期,patientid_chr ";
            orclIcareConn.GetSqlCmd(Strsql);
            Strsql = "select zyh,substr(c.registerid_chr,2),zfy,zfje,c0,c1,c2,c3,c4,c5,c6,c7,c8,c9,c10,c11,c12,c13,c14,c15,c16,c17,c18,c19,c20,c21,c22,c23,c24,c25,c26,c27"
            + " from table_bafy a,t_opr_bih_charge_new b,t_opr_bih_register c where b.registerid_chr=c.registerid_chr and    a.registerid_chr=substr(b.registerid_chr,2)"
            +"  and b.ispass_int=0 and b.type_int=1 ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table7");
            if (dataSet.Tables["table7"].Rows.Count != 0)//判断是否有结算单
            {

                for (int ii = 0; ii < dataSet.Tables["table7"].Rows.Count ; ii++)
                {
                  
                    Strsql = "update dr_emr_headpage set fy_sum=" + dataSet.Tables["table7"].Rows[ii].ItemArray[2].ToString()
                        + ",other16=" + dataSet.Tables["table7"].Rows[ii].ItemArray[3].ToString()
                        + ",m_chuang=" + dataSet.Tables["table7"].Rows[ii].ItemArray[4].ToString()
                        + ",zl=" + dataSet.Tables["table7"].Rows[ii].ItemArray[5].ToString()
                        + ",m_huli=" + dataSet.Tables["table7"].Rows[ii].ItemArray[6].ToString()
                        + ",other3=" + dataSet.Tables["table7"].Rows[ii].ItemArray[7].ToString()
                        + ",m_check=" + dataSet.Tables["table7"].Rows[ii].ItemArray[8].ToString()
                        + ",hy=" + dataSet.Tables["table7"].Rows[ii].ItemArray[9].ToString()
                        + ",fs=" + dataSet.Tables["table7"].Rows[ii].ItemArray[10].ToString()
                        + ",qt=" + dataSet.Tables["table7"].Rows[ii].ItemArray[11].ToString()
                        + ",other4=" + dataSet.Tables["table7"].Rows[ii].ItemArray[12].ToString()
                        + ",other5=" + dataSet.Tables["table7"].Rows[ii].ItemArray[13].ToString()
                        + ",other6=" + dataSet.Tables["table7"].Rows[ii].ItemArray[14].ToString()
                        + ",operation=" + dataSet.Tables["table7"].Rows[ii].ItemArray[15].ToString()
                        + ",mazui=" + dataSet.Tables["table7"].Rows[ii].ItemArray[16].ToString()
                        + ",other7=" + dataSet.Tables["table7"].Rows[ii].ItemArray[17].ToString()
                        + ",other8=" + dataSet.Tables["table7"].Rows[ii].ItemArray[18].ToString()
                        + ",m_xiyao=" + dataSet.Tables["table7"].Rows[ii].ItemArray[19].ToString()
                        + ",other2=" + dataSet.Tables["table7"].Rows[ii].ItemArray[20].ToString()
                        + ",m_zy=" + dataSet.Tables["table7"].Rows[ii].ItemArray[21].ToString()
                        + ",m_cy=" + dataSet.Tables["table7"].Rows[ii].ItemArray[22].ToString()
                        + ",sx=" + dataSet.Tables["table7"].Rows[ii].ItemArray[23].ToString()
                        + ",other9=" + dataSet.Tables["table7"].Rows[ii].ItemArray[24].ToString()
                        + ",other10=" + dataSet.Tables["table7"].Rows[ii].ItemArray[25].ToString()
                        + ",other11=" + dataSet.Tables["table7"].Rows[ii].ItemArray[26].ToString()
                        + ",other12=" + dataSet.Tables["table7"].Rows[ii].ItemArray[27].ToString()
                        + ",other13=" + dataSet.Tables["table7"].Rows[ii].ItemArray[28].ToString()
                        + ",other14=" + dataSet.Tables["table7"].Rows[ii].ItemArray[29].ToString()
                        + ",other15=" + dataSet.Tables["table7"].Rows[ii].ItemArray[30].ToString()
                        + ",other1=" + dataSet.Tables["table7"].Rows[ii].ItemArray[31].ToString()
                        + " where patient_id=" + dataSet.Tables["table7"].Rows[ii].ItemArray[1].ToString() + "";
                    al_sql.Add(Strsql);
                }
            }
            Strsql = "select substr(c.registerid_chr,2)"
            + " from t_opr_bih_charge_new b,t_opr_bih_register c where b.registerid_chr=c.registerid_chr and b.ispass_int=0 and b.type_int=2 ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table8");
            if (dataSet.Tables["table8"].Rows.Count != 0)//判断是否有退费单
            {

                for (int ii = 0; ii < dataSet.Tables["table8"].Rows.Count; ii++)
                {
                    Strsql = "update dr_emr_headpage set fy_sum=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[2].ToString()
                        + ",other16=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[3].ToString()
                        + ",m_chuang=0" //+ dataSet.Tables["table87"].Rows[ii].ItemArray[4].ToString()
                        + ",zl=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[5].ToString()
                        + ",m_huli=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[6].ToString()
                        + ",other3=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[7].ToString()
                        + ",m_check=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[8].ToString()
                        + ",hy=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[9].ToString()
                        + ",fs=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[10].ToString()
                        + ",qt=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[11].ToString()
                        + ",other4=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[12].ToString()
                        + ",other5=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[13].ToString()
                        + ",other6=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[14].ToString()
                        + ",operation=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[15].ToString()
                        + ",mazui=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[16].ToString()
                        + ",other7=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[17].ToString()
                        + ",other8=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[18].ToString()
                        + ",m_xiyao=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[19].ToString()
                        + ",other2=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[20].ToString()
                        + ",m_zy=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[21].ToString()
                        + ",m_cy=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[22].ToString()
                        + ",sx=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[23].ToString()
                        + ",other9=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[24].ToString()
                        + ",other10=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[25].ToString()
                        + ",other11=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[26].ToString()
                        + ",other12=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[27].ToString()
                        + ",other13=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[28].ToString()
                        + ",other14=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[29].ToString()
                        + ",other15=0"// + dataSet.Tables["table8"].Rows[ii].ItemArray[30].ToString()
                        + ",other1=0" //+ dataSet.Tables["table8"].Rows[ii].ItemArray[31].ToString()
                        + " where patient_id=" + dataSet.Tables["table8"].Rows[ii].ItemArray[0].ToString() + "";
                    al_sql.Add(Strsql);
                }
            }
            //填错资料--科室
            Strsql = "select substr(a.REGISTERID_CHR,2) as REGISTERID_CHR,a.MODIFY_DAT,b.code_vchr  from t_opr_bih_register_new2 a,t_bse_deptdesc b "
                + " where a.deptid_chr=b.deptid_chr  and isPost=0";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table9");
            if (dataSet.Tables["table9"].Rows.Count != 0)
            {
                for (int ii = 0; ii < dataSet.Tables["table9"].Rows.Count; ii++)
                {
                    Strsql = "update tblzy_actpatient1 set admiss_ward_code='" + dataSet.Tables["table9"].Rows[ii].ItemArray[2].ToString() 
                        + "',admiss_dept_code='" + dataSet.Tables["table9"].Rows[ii].ItemArray[2].ToString()
                        + "' where patient_id='" + dataSet.Tables["table9"].Rows[ii].ItemArray[0].ToString() + "'";
                    al_sql.Add(Strsql);
                }
            }
            Strsql = "select substr(a.REGISTERID_CHR,2) as REGISTERID_CHR,a.lastname_vchr,b.inpatientid_chr from t_opr_bih_registerdetail_new a,t_opr_bih_register b where a.isPost=0 and a.registerid_chr=b.registerid_chr";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table12");
            if (dataSet.Tables["table12"].Rows.Count != 0)
            {
                for (int ii = 0; ii < dataSet.Tables["table12"].Rows.Count; ii++)
                {
                    Strsql = "update tblzy_actpatient1 set name='" + dataSet.Tables["table12"].Rows[ii].ItemArray[1].ToString()
                        + "' where patient_id='" + dataSet.Tables["table12"].Rows[ii].ItemArray[0].ToString() + "'";
                    al_sql.Add(Strsql);
                    Strsql = "update tblpatient_base set name='" + dataSet.Tables["table12"].Rows[ii].ItemArray[1].ToString()
                        + "' where case_no='" + dataSet.Tables["table12"].Rows[ii].ItemArray[2].ToString() + "'";
                    al_sql.Add(Strsql);
                }
            }
            //转科

           
            Strsql = "   select substr(a.REGISTERID_CHR,2),a.modify_dat,"
                +"  (select b.code_vchr from t_bse_deptdesc b where a.targetdeptid_chr=b.deptid_chr) as newdept,"
                + "  (select b.code_vchr from t_bse_deptdesc b where  a.targetdeptid_chr=b.deptid_chr ) as newared,"
                +"  (select b.code_vchr from t_bse_deptdesc b where  a.sourcedeptid_chr=b.deptid_chr ) as olddept,"
                + "  (select b.code_vchr from t_bse_deptdesc b where  a.sourcedeptid_chr=b.deptid_chr ) as oldared,"
                + " (select  b.deptname_vchr from t_bse_deptdesc b where  a.targetdeptid_chr=b.deptid_chr ) as newName"
                +"  from t_opr_bih_transfer_new a    where a.type_int=3  and a.ispost=0   ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table10");
            if (dataSet.Tables["table10"].Rows.Count != 0)
            {
                for (int ii = 0; ii < dataSet.Tables["table10"].Rows.Count; ii++)
                {
                    Strsql = "select max(times) from tblzy_change_dept where patient_id='" + dataSet.Tables["table10"].Rows[ii].ItemArray[0].ToString() + "'";
                    SqlDataReader sqlDataReader = dbConn.GetDataReader(Strsql);
                    if (sqlDataReader.Read())
                    {
                        if (sqlDataReader.GetValue(0).ToString() == null || sqlDataReader.GetValue(0).ToString() == "")
                        {
                            int_maxTimes = 1;
                        }
                        else
                        {
                            int_maxTimes = int.Parse(sqlDataReader.GetValue(0).ToString()) + 1;
                        }
                    }
                    //Strsql = "update tblzy_actpatient1 set admiss_dept_code='" + dataSet.Tables["table10"].Rows[ii].ItemArray[2].ToString()
                    //    + "', admiss_ward_code='" + dataSet.Tables["table10"].Rows[ii].ItemArray[2].ToString() + "' where patient_id='" + dataSet.Tables["table10"].Rows[ii].ItemArray[0].ToString() + "'";
                    Strsql = "update tblzy_actpatient1 set admiss_dept_code='" + dataSet.Tables["table10"].Rows[ii].ItemArray[2].ToString()
                        + "' where patient_id='" + dataSet.Tables["table10"].Rows[ii].ItemArray[0].ToString() + "'";
                    al_sql.Add(Strsql);
                    if (int_maxTimes == 1)
                    {
                        Strsql = "update dr_emr_headpage set zkkb='" + dataSet.Tables["table10"].Rows[ii].ItemArray[2].ToString()
                        + "',zkkb_date='" + DateTime.Parse(dataSet.Tables["table10"].Rows[ii].ItemArray[1].ToString()).ToString("yyyy-MM-dd HH:mm:dd") + "'"
                        + "where patient_id='" + dataSet.Tables["table10"].Rows[ii].ItemArray[0].ToString() + "'";
                        al_sql.Add(Strsql);
                    }
                    else
                    {


                        Strsql = "update dr_emr_headpage set zkkb1='" + dataSet.Tables["table10"].Rows[ii].ItemArray[6].ToString()
                        + "',zkkb_date1='" + DateTime.Parse(dataSet.Tables["table10"].Rows[ii].ItemArray[1].ToString()).ToString("yyyy-MM-dd HH:mm:dd") + "'"
                        + "where patient_id='" + dataSet.Tables["table10"].Rows[ii].ItemArray[0].ToString() + "'";
                        al_sql.Add(Strsql);
                    }
                }
            }
            Strsql = "  select substr(a.REGISTERID_CHR,2),1,a.modify_dat,null,null,  "
                + "  (select b.code_vchr from t_bse_deptdesc b where  a.sourcedeptid_chr=b.deptid_chr ) as olddept,"
                + "  (select b.code_vchr from t_bse_deptdesc b where  a.sourceareaid_chr=b.deptid_chr ) as oldared,null,"
                + "  (select b.code_vchr from t_bse_deptdesc b where a.targetdeptid_chr=b.deptid_chr) as newdept,"
                + "  (select b.code_vchr from t_bse_deptdesc b where  a.targetareaid_chr=b.deptid_chr ) as newared,null,"
                + "  (select c.code_chr from t_bse_bed c where a.targetbedid_chr=c.bedid_chr and a.targetareaid_chr=c.nurseunitid_vchr) "
            + " from t_opr_bih_transfer_new a where a.type_int=3  and a.ispost=0  ";
            oracleDataAdapter = orclIcareConn.GetDataAdapter(Strsql);
            dataSet = new DataSet();
            oracleDataAdapter.Fill(dataSet, "table11");
            if (dataSet.Tables["table11"].Rows.Count != 0)
            {
                for (int ii = 0; ii < dataSet.Tables["table11"].Rows.Count; ii++)
                {
                    Strsql = "insert into tblzy_change_dept values('" + dataSet.Tables["table11"].Rows[ii].ItemArray[0].ToString() + "',"
                        + int_maxTimes.ToString() + ",'"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[2].ToString() + "',null,null,'"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[5].ToString() + "','"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[5].ToString() + "',null,'"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[8].ToString() + "','"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[8].ToString() + "',null,'"
                        + dataSet.Tables["table11"].Rows[ii].ItemArray[11].ToString() + "')";
                    al_sql.Add(Strsql);
                }
            }
            Strsql = "update t_opr_bih_register_new set isPost=1 where isPost=0";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_register_zhuxiao set isPost=1 where isPost=0";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_register_chuyuan set isPost=1 where isPost=0";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_register_qxcy set isPost=1 where isPost=0";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_charge_new set isPass_int=1 where isPass_int=0 ";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_register_new2 set isPost=1 where isPost=0 ";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_transfer_new set isPost=1 where isPost=0 ";
            al_orc.Add(Strsql);
            Strsql = "update t_opr_bih_registerdetail_new set isPost=1 where isPost=0 ";
            al_orc.Add(Strsql);
            try
            {
                //MessageBox.Show("sql" + dbConn.GetTransaction(al_sql).ToString());
                //MessageBox.Show("oracle" + orclIcareConn.GetTransaction(al_orc));
                if (dbConn.GetTransaction(al_sql) && orclIcareConn.GetTransaction(al_orc))
                {
                    i_time = 60;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public String getJsbr(DataSet ds,String tableNmae)
        {
            String Str_jsbr = "";
            for (int ii = 0; ii < ds.Tables[tableNmae].Rows.Count - 1;ii++ )
            {
                Str_jsbr = Str_jsbr + "'" + ds.Tables[tableNmae].Rows[ii].ItemArray[0].ToString() + "',";
            }
            Str_jsbr = Str_jsbr + "'" + ds.Tables[tableNmae].Rows[ds.Tables[tableNmae].Rows.Count - 1].ItemArray[0].ToString() + "'";
            return Str_jsbr;
        }
        public String getTypeString(String str)
        {
            int length = 0;
            String Str1 = "";
            int i = 0;
            do
            {
                length = str.IndexOf(',');
                if (length > 0)
                {
                    if (i == 0)
                    {
                        Str1 = Str1 + "'" + str.Substring(0, length) + "'";
                        i++;
                    }
                    else
                    {
                        Str1 = Str1 + ",'" + str.Substring(0, length) + "'";
                        i++;
                    }
                    str = str.Substring(length + 1);
                }
                else if (length == -1)
                {
                    if (i == 0)
                    {
                        Str1 = "'" + str + "'";
                    }
                    else
                    {
                        Str1 = Str1 + ",'" + str + "'";
                    }
                }
            }
            while (length > 0);

            return Str1;
        }

    }
}
