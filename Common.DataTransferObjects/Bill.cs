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
	using System.Linq;
	using System.Text;

	public class Bill
	{
		public virtual byte[] PDF
		{
			get;
			set;
		}

		public virtual DateTime Date
		{
			get;
			set;
		}

        public virtual Customer Customer
        {
            get;
            set;
        }

	}
}

