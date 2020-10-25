using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using IT9000.Wpf.Repositories;
using System.Reflection;
using System.IO;
using System.Windows;

namespace IT9000.Wpf.Services
{
    public class WindowLocationDistributeService
    {
        private int _rowCount;
        private int _colCount;
        private double _cellWidth;
        private double _cellHeigth;
        public WindowLocationDistributeService(int rowCount, int colCount)
        {
            _rowCount = rowCount;
            _colCount = colCount;
            _cellWidth = Math.Floor(SystemParameters.WorkArea.Width / colCount);
            _cellHeigth = Math.Floor(SystemParameters.WorkArea.Height / rowCount);
        }

        private (int RowIndex,int ColIndex) GetCellIndex(int indexStartSinceOne)
        {
            int rowIndex = indexStartSinceOne % _colCount==0? indexStartSinceOne / _colCount-1:indexStartSinceOne/_colCount;
            int colIndex = (indexStartSinceOne-1) % _colCount;
            return (rowIndex,colIndex);
        }

        public WindowLocation GetWindowLocation(int deviceIndex)
        {
            var cellIndex = GetCellIndex(deviceIndex);
            return new WindowLocation(cellIndex.ColIndex * _cellWidth,cellIndex.RowIndex * _cellHeigth);
        }
        
    }
}