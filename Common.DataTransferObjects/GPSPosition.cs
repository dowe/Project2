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

	public class GPSPosition
	{
        [Key, Column(Order = 0)]
	    public float Latitude { get; set; }
        [Key, Column(Order = 1)]
	    public float Longitude { get; set; }

	    public GPSPosition(float latitude, float longitude)
	    {
	        Latitude = latitude;
	        Longitude = longitude;
	    }
	}
}

