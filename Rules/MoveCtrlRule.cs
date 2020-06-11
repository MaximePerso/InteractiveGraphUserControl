using InteractiveGraphUserControl.MVVM;
using InteractiveGraphUserControl.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace InteractiveGraphUserControl.Rules
{
    class MoveCtrlRule : ValidationRule
    {
        //Makes sur that if "Relative" move position is selected, the previous command had the same MoveCtrl (otherwise relative is bullshit)
        //for the whole row but does not trigger firectly when the LimMode combobox is updated
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //triggers when the Combobox is updated
            if (value is BindingExpression be)
            {
                DoliInput input = be.DataItem as DoliInput;
                BindingGroup bg = be.BindingGroup;
                if (bg.Owner is DataGridRow dgrow)
                {
                    DataGrid dg = VisualHelper.FindParent<DataGrid>(dgrow);
                    int currItemKey = input.SequenceNumber - 1;
                    if(input.SequenceNumber < (((MVVM.ViewModel)dg.DataContext).DoliInputCollection).Count())
                    {
                        if (((((MVVM.ViewModel)dg.DataContext).DoliInputCollection)[currItemKey + 1]).MoveCtrl != input.MoveCtrl
                                     && ((((MVVM.ViewModel)dg.DataContext).DoliInputCollection)[currItemKey + 1]).LimMode == "Relative")
                        {
                            return new ValidationResult(false, $"To use the \"Relative\" option, the next command should have the same Move Ctrl as this one ");
                        }
                    }
                    
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
