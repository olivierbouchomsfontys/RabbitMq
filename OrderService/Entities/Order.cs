﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Entities
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class Order
    {
        [Key]
        public int Id { get; init; }
        public DateTime Created { get; init; }
        public int CustomerId { get; init; }
    }
}