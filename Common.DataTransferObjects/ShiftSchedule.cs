﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Common.DataTransferObjects
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;

	public class ShiftSchedule
	{
        [Key]
        public virtual DateTime Date
        {
            get;
            set;
        }
		public virtual IList<DayEntry> DayEntry
		{
			get;
			set;
		}

	}
}

