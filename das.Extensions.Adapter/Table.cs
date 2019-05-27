using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace das.Extensions.Adapter
{
    public class Table
    {
        private readonly DataTable _dataTable;

        public Table(DataTable dataTable, bool isFirstRowHeader = false)
        {
            _dataTable = dataTable;

            if (isFirstRowHeader)
            {
                for (int i = 0; i < _dataTable.Columns.Count; ++i)
                {
                    _dataTable.Columns[i].ColumnName = _dataTable.Rows[0][i].ToString();
                }

                _dataTable.Rows.RemoveAt(0);
            }
        }

        public IEnumerable<string> Fields
        {
            get { return _dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName); }
        }

        public IEnumerable<Row> Rows
        {
            get { return _dataTable.Rows.Cast<DataRow>().Select(r => new Row(r)); }
        }
    }

    public class Row : IEnumerable<Cell>
    {
        private readonly DataRow _dataRow;
        private readonly DataTable _dataTable;

        public Row(DataRow dataRow)
        {
            _dataRow = dataRow;
            _dataTable = _dataRow.Table;
        }

        public Cell this[string fieldName]
        {
            get
            {
                if (_dataTable.Columns.Contains(fieldName))
                    return new Cell(fieldName, _dataRow[fieldName]);

                return null;
            }
        }

        public Cell this[int fieldIndex]
        {
            get
            {
                if (_dataTable.Columns.Count >= fieldIndex)
                    return new Cell(_dataTable.Columns[fieldIndex].ColumnName, _dataRow[fieldIndex]);

                return null;
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            return _dataTable.Columns.Cast<DataColumn>().Select(c => new Cell(c.ColumnName, _dataRow[c.ColumnName])).GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            foreach (Cell c in this)
                result.Append(result.Length == 0 ? c.ToString() : $"; {c}");

            return result.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Cell
    {
        public string Name { get; protected set; }
        public object Value { get; protected set; }

        public Cell(string name, object value)
        {
            Name = name;
            Value = value is DBNull ? "" : value.ToString().Trim();
        }

        public override string ToString()
        {
            return $"[{Name}]={Value}";
        }
    }
}
