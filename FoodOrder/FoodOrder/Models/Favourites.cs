
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace FoodOrder.Models
{

using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Favourites
{
    [Key]
    public int FavouriteId { get; set; }

    public string UserId { get; set; }

    public int ProductId { get; set; }

    public System.DateTime CreateAt { get; set; }

    public virtual Products Products { get; set; }

    public virtual Users Users { get; set; }

}

}
