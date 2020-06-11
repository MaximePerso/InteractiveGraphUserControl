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
    class RelativeLimitModeRule : ValidationRule
    {
        //Makes sur that if "Relative" move position is selected, the previous command had the same MoveCtrl (otherwise relative is bullshit)
        //for the whole row but does not trigger firectly when the LimMode combobox is updated
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is BindingGroup bg && bg.Items.Count > 0)
            {
                if ((bg.Items[0] as DoliInput).LimMode == "Relative")
                {
                    if (bg.Owner is DataGridRow dgrow)
                    {
                        DataGrid dg = VisualHelper.FindParent<DataGrid>(dgrow);
                        int currItemKey = ((bg.Items[0]) as DoliInput).SequenceNumber - 1;
                        if (currItemKey == 0)
                        {
                            return new ValidationResult(false, $"First command cannot be \"Relative\" ");
                        }
                        else
                        {
                            if (((((InteractiveGraphUserControl.MVVM.ViewModel)dg.DataContext).DoliInputCollection)[currItemKey - 1]).MoveCtrl != (bg.Items[0] as DoliInput).MoveCtrl)
                            {
                                return new ValidationResult(false, $"To use the \"Relative\" option, the previous command should have the same Move Ctrl as this one ");
                            }
                        }
                    }
                }
            }
            //triggers when the Combobox is updated
            if (value is BindingExpression be)
            {
                DoliInput input = be.DataItem as DoliInput;

                if (input.LimMode == "Relative")
                {
                    if (input.SequenceNumber == 1)
                    {
                        return new ValidationResult(false, $"First command cannot be \"Relative\" ");
                    }
                    else
                    {
                        BindingGroup bg2 = be.BindingGroup;
                        if (bg2.Owner is DataGridRow dgrow)
                        {
                            DataGrid dg = VisualHelper.FindParent<DataGrid>(dgrow);
                            int currItemKey = input.SequenceNumber - 1;
                            if (((((MVVM.ViewModel)dg.DataContext).DoliInputCollection)[currItemKey - 1]).MoveCtrl != input.MoveCtrl)
                            {
                                return new ValidationResult(false, $"To use the \"Relative\" option, the previous command should have the same Move Ctrl as this one ");
                            }
                        }
                    }
                }

            }
            return ValidationResult.ValidResult;
        }
    }
}
