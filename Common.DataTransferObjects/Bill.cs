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
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;

	public class Bill
	{
		public virtual string PDFPath
		{
			get;
			set;
		}

        [Key, Column(Order = 1)]
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

        [ForeignKey("Customer")]
        [Key, Column(Order = 0)]
        public virtual String CustomerUserName
        {
            get;
            set;
        }
	}
}

