using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{
    public class ShoppingCart
    {
        /// private LinqValueCalculator calc;

        /*public ShoppingCart(LinqValueCalculator calcParam)
        {
            calc = calcParam;
        }*/
        private IValueCalculator calc;

        public ShoppingCart(IValueCalculator calcParam)
        {
            calc = calcParam;
        }
        // это выше для инверсии управления, нужно для замены всего одного аргумента, не городить одинаковый код
        // по сути здесь написано наследование реализованного интерфейса

        public IEnumerable<Product> Products { get; set; }

        public decimal CalculateProductTotal()
        {
            return calc.ValueProducts(Products); // В чем целесообразность этого? - для использования несколькими классами одного метода
    //        return Products.Sum(p => p.Price);// че б так не сделать??
        }
    }
}