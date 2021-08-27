using Psps.Core.JqGrid.Models;
using Psps.Core.Models;
using Psps.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psps.Services.Report
{
    public partial interface IReportService
    {
        System.IO.MemoryStream GenerateR1Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR27Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR2Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR3Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR4Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR5Excel(string templatePath, int year);

        System.IO.MemoryStream GenerateR6Excel(string templatePath, int disasterMasterId);

        System.IO.MemoryStream GenerateR7Excel(string templatePath, int? fromYear, int? toYear);

        System.IO.MemoryStream GenerateR8Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR9Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR10Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR11Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR12Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR13Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR14Excel(string templatePath, int? fromYear, int? toYear, string complaintSource);

        System.IO.MemoryStream GenerateR15Excel(string templatePath);

        System.IO.MemoryStream GenerateR16Excel(string templatePath, DateTime? criteriaDate);

        System.IO.MemoryStream GenerateR17Word(int compliantMasterId, string templatePath);

        System.IO.MemoryStream GenerateR18Excel(string templatePath);

        System.IO.MemoryStream GenerateR19Excel(string templatePath);

        System.IO.MemoryStream GenerateR20Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR21Excel(string templatePath, int? year);

        System.IO.MemoryStream GenerateR22Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream GenerateR23Excel(string templatePath, int? datefrom, int? dateto);

        System.IO.MemoryStream GenerateR24Excel(string templatePath, string from);

        System.IO.MemoryStream GenerateR25Excel(string templatePath, DateTime? sentDate, DateTime? toDate);

        System.IO.MemoryStream GenerateR26Excel(string templatePath, DateTime? datefrom, DateTime? dateto);

        System.IO.MemoryStream ExportTableToExcel(string strName, string strSql, Dictionary<string, string> columnName, string strWhere = null);

        System.IO.MemoryStream ExportToExcel<T>(IList<T> dataList, Dictionary<string, Func<object, object>> columnMappings = null, ColumnModel columnModel = null, List<string> filterCriterias = null, bool showHiddenCol = false);

        string ExportTablesToZipFile(string folderPath, string zFileName, List<string> tables);

        void ExportSqlToCsv(string strSql, string strDestFileName);

        void DocumentsMerge(object fileName, List<string> arrayList);
    }
}